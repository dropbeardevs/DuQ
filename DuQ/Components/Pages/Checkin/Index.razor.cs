using DuQ.Data;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DuQ.Components.Pages.Checkin;

public partial class Index
{
    [Inject]
    private Domain Domain { get; set; } = default!;

    [SupplyParameterFromForm]
    private CheckinModel? Model { get; set; }

    private List<string> _queueTypes = default!;
    private bool _isCheckedIn;



    protected override async void OnInitialized()
    {
        _isCheckedIn = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        Model ??= new CheckinModel();
        _queueTypes = await Domain.GetQueueTypes();
    }

    private async Task SubmitAsync()
    {
        bool success = await Domain!.SaveStudent(Model!);

        _isCheckedIn = true;
    }
}
