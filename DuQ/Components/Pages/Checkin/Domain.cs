using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Checkin;

public class Domain(DuqContext context)
{
    public async Task<bool> SaveStudent(CheckinModel? model)
    {
        if (model is null)
            return false;

        var testItem = new DuQueue()
        {
            QueueType = new QueueType()
            {
                QueueName = "Test"
            },
            Status = new QueueStatus()
            {
                Status = "Waiting"
            },
            Student = new Student()
            {
                StudentFirstName = "Homer",
                StudentId = "12345"
            }
        };

        context.DuQueues.Add(testItem);

        await context.SaveChangesAsync();

        return true;
    }
}
