using DuQ.Contexts;
using DuQ.Core;
using DuQ.Models.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DuQ.Components.Pages.Admin;

public class Domain
{
    private readonly IDbContextFactory<DuqContext> _contextFactory;
    private readonly DbSaveNotifier _dbSaveNotifier;

    public Domain(IDbContextFactory<DuqContext> contextFactory, DbSaveNotifier dbSaveNotifier)
    {
        _contextFactory = contextFactory;
        _dbSaveNotifier = dbSaveNotifier;
    }

    public async Task<List<AdminDto>> GetQueueItemsAsync()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        var items = await context.DuQueues
                                 .Include(q => q.Student)
                                 .Include(q => q.QueueLocation)
                                 .Include(q => q.QueueStatus)
                                 .Where (q => q.QueueStatus.Status != "Finished")
                                 .Where (q => q.QueueStatus.Status != "Deleted")
                                 .OrderBy(x => x.CheckinTime)
                                 .Select(item => new AdminDto()
                                                 {
                                                     QueueId = item.Id,
                                                     QueueLocation = item.QueueLocation.Location,
                                                     StudentFirstName = item.Student.FirstName,
                                                     StudentLastName = item.Student.LastName,
                                                     StudentContactDetails = item.Student.ContactDetails,
                                                     QueueStatus = item.QueueStatus.Status,
                                                     CheckinTime = item.CheckinTime,
                                                     CheckoutTime = item.CheckoutTime,
                                                     Modified = item.ModifiedUtc
                                                 })
                                 .ToListAsync();

        return items;
    }

    public async Task RefreshAdminItemsAsync()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        context.SavedChanges += _dbSaveNotifier.NotifyDbSaved;

    }

    public async Task SetStatusToInQueue(Guid id)
    {
        try
        {
            await using DuqContext context = await _contextFactory.CreateDbContextAsync();

            var item = await context.DuQueues
                                    .Include(q => q.QueueStatus)
                                    .SingleAsync(q => q.Id == id);

            item.QueueStatus = await context.DuQueueStatuses
                                            .Where(x => x.Status == "In Queue")
                                            .SingleAsync();

            context.DuQueues.Update(item);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error changing status to In Queue");
            throw;
        }
    }

    public async Task SetStatusToServing(Guid id)
    {
        try
        {
            await using DuqContext context = await _contextFactory.CreateDbContextAsync();

            var item = await context.DuQueues
                                    .Include(q => q.QueueStatus)
                                    .SingleAsync(q => q.Id == id);

            item.QueueStatus = await context.DuQueueStatuses
                                            .Where(x => x.Status == "Serving")
                                            .SingleAsync();

            context.DuQueues.Update(item);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error changing status to Served");
            throw;
        }
    }

    public async Task SetStatusToFinished(Guid id)
    {
        try
        {
            await using DuqContext context = await _contextFactory.CreateDbContextAsync();

            var item = await context.DuQueues
                                    .Include(q => q.QueueStatus)
                                    .SingleAsync(q => q.Id == id);

            item.QueueStatus = await context.DuQueueStatuses
                                            .Where(x => x.Status == "Finished")
                                            .SingleAsync();

            context.DuQueues.Update(item);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error changing status to Finished");
            throw;
        }
    }

    public async Task SetStatusToDeleted(Guid id)
    {
        try
        {
            await using DuqContext context = await _contextFactory.CreateDbContextAsync();

            var item = await context.DuQueues
                                    .Include(q => q.QueueStatus)
                                    .SingleAsync(q => q.Id == id);

            item.QueueStatus = await context.DuQueueStatuses
                                                .Where(x => x.Status == "Deleted")
                                                .FirstAsync();

            context.DuQueues.Update(item);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error changing status to Deleted");
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
