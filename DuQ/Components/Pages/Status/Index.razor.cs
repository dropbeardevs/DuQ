using System.Collections.ObjectModel;
using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Status;

public partial class Index
{
    [Inject]
    private Status.Domain? Domain { get; set; }
    private bool IsLoading { get; set; }
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

        await Domain!.GetQueueItemsAsync();

        IsLoading = false;
        StateHasChanged();
    }
}
