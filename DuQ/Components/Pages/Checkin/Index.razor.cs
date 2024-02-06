using DuQ.Data;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DuQ.Components.Pages.Checkin;

public partial class Index
{
    [Inject]
    private Domain? Domain { get; set; }

    [SupplyParameterFromForm]
    private CheckinModel? Model { get; set; }

    private bool _isCheckedIn;

    protected override async void OnInitialized()
    {
        Model ??= new CheckinModel();
        _isCheckedIn = false;
    }

    protected override async Task OnParametersSetAsync()
    {

    }

    private async Task SubmitAsync()
    {
        bool success = await Domain!.SaveStudent(Model!);

        _isCheckedIn = true;
    }
}
