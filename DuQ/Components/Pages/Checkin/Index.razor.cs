using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Checkin;

public partial class Index
{
    [Inject]
    private Domain? Domain { get; set; }

    [SupplyParameterFromForm]
    public CheckinModel? Model { get; set; }

    private string? _myName;
    private bool _isCheckedIn;

    protected override void OnInitialized()
    {
        Model ??= new CheckinModel();
        _isCheckedIn = false;
    }

    private async Task SubmitAsync()
    {
        bool success = await Domain!.SaveStudent(Model!);

        _isCheckedIn = true;
    }
}
