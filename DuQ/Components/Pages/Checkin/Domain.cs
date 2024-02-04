using DuQ.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace DuQ.Components.Pages.Checkin;

public class Domain(DuqContext context)
{
    public async Task<bool> SaveStudent(CheckinModel? model)
    {
        if (model is null)
            return false;

        Student student = new()
        {
            FirstName = "Homer",
            StudentNo = "Test1234",
            LastUpdated = DateTime.Now
        };

        DuQueueType queueType = new()
        {
            Name = "TEST",
            LastUpdated = DateTime.Now
        };

        DuQueueStatus queueStatus = new()
        {
            Status = "Finito",
            LastUpdated = DateTime.Now
        };

        DuQueue testItem = new()
        {
            Student = student,
            QueueType = queueType,
            QueueStatus = queueStatus,
            LastUpdated = DateTime.Now
        };

        context.DuQueues.Add(testItem);

        await context.SaveChangesAsync();

        return true;
    }
}
