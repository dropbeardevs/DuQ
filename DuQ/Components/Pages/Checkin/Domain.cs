using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Checkin;

public class Domain(DuqContext context)
{
    public async Task SaveStudent(CheckinModel model)
    {
        context.Students.Add(new Student()
        {
            StudentFirstName = model.FirstName
        });
    }
}
