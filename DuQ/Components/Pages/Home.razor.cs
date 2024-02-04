using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages;

public partial class Home
{
    [Inject]
    private DuqContext? Context { get; set; }

    protected override void OnParametersSet()
    {
        // Student tempStudent = new()
        // {
        //     StudentId = "1",
        //     StudentFirstName = "Homer",
        //     StudentLastName = "Nay"
        // };
        //
        // Context.Students.Add(tempStudent);
        //
        // Context.SaveChanges();
    }
}
