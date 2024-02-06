using DuQ.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Checkin;

public class Domain(DuqContext context)
{
    public async Task<bool> SaveStudent(CheckinModel? model)
    {
        if (model is null)
            return false;

        try
        {
            // Add logic to prevent students from adding themselves multiple times

            Student? student = await context.Students
                .FirstOrDefaultAsync(x => x.StudentNo == model.StudentId);

            if (student is not null)
            {
                // Check if they've checked in within the last hour
                // If so, then update existing record
                DuQueue? queueItem = await context.DuQueues
                    .Where(x => x.Student.StudentNo == model.StudentId)
                    .Where(x => x.CheckinTime > DateTime.Now.AddHours(-1))
                    .FirstOrDefaultAsync();

                if (queueItem is not null)
                {
                    queueItem.CheckinTime = DateTime.Now;
                    queueItem.LastUpdated = DateTime.Now;
                    queueItem.QueueType = await context.DuQueueTypes
                        .Where(x => x.Name == model.QueueType)
                        .FirstAsync();

                    await context.SaveChangesAsync();

                    return true;
                }
            }

            student ??= new Student()
            {
                StudentNo = model.StudentId!,
                FirstName = model.FirstName!,
            };

            DuQueueType queueType = await context.DuQueueTypes
                .Where(x => x.Name == model.QueueType)
                .FirstAsync();

            DuQueueStatus queueStatus = await context.DuQueueStatuses
                .Where(x => x.Status == "In Queue")
                .FirstAsync();

            DuQueue newQueueItem = new()
            {
                Student = student,
                QueueType = queueType,
                QueueStatus = queueStatus,
                CheckinTime = DateTime.Now,
                LastUpdated = DateTime.Now
            };

            context.DuQueues.Add(newQueueItem);

            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<List<string>> GetQueueTypes()
    {
        try
        {
            List<string>? queueTypes = await context.DuQueueTypes.Select(x => x.Name).ToListAsync();

            return queueTypes;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

    }
}
