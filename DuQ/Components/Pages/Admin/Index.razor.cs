using DuQ.Core;
using DuQ.Models.Core;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Admin;

public partial class Index
{
    [Inject] private Domain Domain { get; set; } = default!;

    [Inject] private DbSaveNotifier? DbSaveNotifier { get; set; }

    [SupplyParameterFromForm] private AdminModel? Model { get; set; }

    private List<string> _queueLocations = [];

    private List<AdminDto> _queueItems = null!;

    private AdminDto? _currentlyServing;

    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        DbSaveNotifier!.OnDbSave += ReloadQueueItemsHandler;

        await LoadStatusItems();

        _isLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        Model ??= new AdminModel();
        _queueLocations = await Domain.GetLocationsAsync();

        // Add All
        _queueLocations.Insert(0, "All");

        Model.Location = "All";
    }

    private async Task LoadStatusItems()
    {
        _isLoading = true;
        //await InvokeAsync(StateHasChanged);

        _queueItems = await Domain.GetQueueItemsAsync();

        _isLoading = false;
        //await InvokeAsync(StateHasChanged);
    }

    private async Task Refresh()
    {
        await Domain.RefreshAdminItemsAsync();
    }

    private async Task ReloadQueueItemsHandler()
    {
        _queueItems = await Domain.GetQueueItemsAsync();

        await InvokeAsync(StateHasChanged);
    }

    private async Task ServeAsync(AdminDto item)
    {
        // Put item back in queue
        if (_currentlyServing is not null)
        {
            await Domain.SetStatusToInQueue(_currentlyServing.QueueId);
        }

        await Domain.SetStatusToServing(item.QueueId);

        _currentlyServing = item;

        await LoadStatusItems();
    }

    private async Task FinishAsync(AdminDto item)
    {
        await Domain.SetStatusToFinished(item.QueueId);

        _currentlyServing = null;

        await LoadStatusItems();
    }

    private async Task DeleteAsync(AdminDto item)
    {
        await Domain.SetStatusToDeleted(item.QueueId);
    }

    void ClearData()
    {
        Model!.Location = string.Empty;
        _currentlyServing = null;
    }

    // quick filter - filter globally across multiple columns with the same input
    private Func<AdminDto, bool> QuickFilter => x =>
    {
        if (Model!.Location is "All")
            return true;

        if (string.IsNullOrWhiteSpace(Model!.Location))
            return true;

        if (x.QueueLocation.Contains(Model!.Location, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };
}
