using DuQ.Models.Core;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DuQ.Components.Pages.Checkin;

public partial class Index
{
    [Inject]
    private Domain Domain { get; set; } = default!;

    [SupplyParameterFromForm]
    private CheckinModel? Model { get; set; }

    private List<string> _queueLocations = [];
    private bool _isCheckedIn;

    protected override async Task OnInitializedAsync()
    {
        _isCheckedIn = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        Model ??= new CheckinModel();
        _queueLocations = await Domain.GetLocationsAsync();
    }

    private async Task SubmitAsync()
    {
        bool success = await Domain.SaveStudentAsync(Model!);

        _isCheckedIn = true;
    }
}
