using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Admin;

public partial class Index
{
    [Inject]
    private Domain Domain { get; set; } = default!;
    private bool _isLoading;
    private bool _isCancelled;
    private List<DuQueueDto>? _queueItems;

    protected override async void OnInitialized()
    {
        _isLoading = false;
        _isCancelled = false;
        await LoadStatusItems();
    }

    private async Task LoadStatusItems()
    {
        _isLoading = true;
        StateHasChanged();

        _queueItems = await Domain!.GetQueueItemsAsync();

        var tempNAme = _queueItems.FirstOrDefault();

        _isLoading = false;
        StateHasChanged();
    }
}
