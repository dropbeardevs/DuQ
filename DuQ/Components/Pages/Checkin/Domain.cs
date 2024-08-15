using DuQ.Contexts;
using DuQ.Core;
using DuQ.Models.Core;
using DuQ.Models.DuQueue;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog;

namespace DuQ.Components.Pages.Checkin;

public class Domain
{
    private readonly IDbContextFactory<DuqContext> _contextFactory;
    private readonly DbSaveNotifier _dbSaveNotifier;

    public Domain(IDbContextFactory<DuqContext> contextFactory, DbSaveNotifier dbSaveNotifier)
    {
        _contextFactory = contextFactory;
        _dbSaveNotifier = dbSaveNotifier;
    }

    public async Task<bool> SaveStudentAsync(CheckinModel? model)
    {
        if (model is null)
            return false;

        try
        {
            await using DuqContext context = await _contextFactory.CreateDbContextAsync();

            context.SavedChanges += _dbSaveNotifier.NotifyDbSaved;

            // Add logic to prevent students from adding themselves multiple times
            Student? student = await context.Students
                .FirstOrDefaultAsync(x => x.FirstName == model.FirstName && x.LastName == model.LastName);

            if (student is not null)
            {
                // Check if they've checked in within the last hour
                // If so, then update existing record
                DuQueue? queueItem = await context.DuQueues
                    .Where(x => x.Student.FirstName == model.FirstName && x.Student.LastName == model.LastName)
                    .Where(x => x.CheckinTime > DateTime.UtcNow.AddHours(-1))
                    .FirstOrDefaultAsync();

                if (queueItem is not null)
                {
                    queueItem.CheckinTime = DateTime.UtcNow;
                    queueItem.ModifiedUtc = DateTime.UtcNow;
                    queueItem.QueueLocation = await context.DuQueueLocations
                        .Where(x => x.Location == model.Location)
                        .FirstAsync();
                    queueItem.QueueStatus = await context.DuQueueStatuses
                                                           .Where(x => x.Status == "In Queue")
                                                           .FirstAsync();

                    await context.SaveChangesAsync();

                    return true;
                }
            }

            student ??= new Student()
            {
                FirstName = model.FirstName!,
                LastName = model.LastName!,
                ContactDetails = model.ContactDetails!,
            };

            DuQueueLocation queueLocation = await context.DuQueueLocations
                .Where(x => x.Location == model.Location)
                .FirstAsync();

            DuQueueStatus queueStatus = await context.DuQueueStatuses
                .Where(x => x.Status == "In Queue")
                .FirstAsync();

            DuQueue newQueueItem = new()
            {
                Student = student,
                QueueLocation = queueLocation,
                QueueStatus = queueStatus,
                CheckinTime = DateTime.UtcNow,
                ModifiedUtc = DateTime.UtcNow
            };

            context.DuQueues.Add(newQueueItem);

            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error saving student");
            throw;
        }
    }

    public async Task<List<string>> GetLocationsAsync()
    {
        try
        {
            await using DuqContext context = await _contextFactory.CreateDbContextAsync();

            List<string>? queueLocations = await context.DuQueueLocations.Select(x => x.Location).ToListAsync();

            return queueLocations;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting locations");
            throw;
        }
    }
}
