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

        Student student = new()
        {
            StudentNo = model.StudentId!,
            FirstName = model.FirstName!,
        };

        try
        {
            // DuQueueType? queueType = await context.DuQueueTypes
            //     .Where(x => x.Name == model.QueueType)
            //     .FirstOrDefaultAsync();

            DuQueueType? queueType = await context.DuQueueTypes
                .Where(x => x.Name == "Campus ID Card")
                .FirstOrDefaultAsync();

            DuQueueStatus? queueStatus = await context.DuQueueStatuses
                .Where(x => x.Status == "In Queue")
                .FirstOrDefaultAsync();

            if (queueType is null || queueStatus is null)
            {
                throw new Exception("Unable to get Queue Type or Queue Status");
            }

            DuQueue queueItem = new()
            {
                Student = student,
                QueueType = queueType,
                QueueStatus = queueStatus,
                CheckinTime = DateTime.Now,
                LastUpdated = DateTime.Now
            };

            context.DuQueues.Add(queueItem);

            await context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }
}
