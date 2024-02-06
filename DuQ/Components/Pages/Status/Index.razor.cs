using DuQ.Data;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace DuQ.Components.Pages.Status;

public partial class Index
{
    private bool IsLoading { get; set; }

    [Inject] private Status.Domain? Domain { get; set; }

    private List<DuQueue>? _queueItems;
    private bool _isCancelled;

    protected override async void OnInitialized()
    {
        _isCancelled = false;
        await LoadStatusItems();
    }

    private async Task LoadStatusItems()
    {
        IsLoading = true;
        StateHasChanged();

        _queueItems = await Domain!.GetQueueItemsAsync();

        var tempNAme = _queueItems.FirstOrDefault();

        IsLoading = false;
        StateHasChanged();
    }
}
