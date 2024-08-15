using DuQ.Models.Core;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Admin;

public partial class Index
{
    [Inject]
    private Domain Domain { get; set; } = default!;

    private List<AdminDto> _queueItems = null!;

    private bool _isLoading;

    protected override async void OnInitialized()
    {
        await LoadStatusItems();
        _isLoading = false;
    }

    private async Task LoadStatusItems()
    {
        _isLoading = true;
        StateHasChanged();

        //_queueItems = await Domain.GetQueueItemsAsync();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task Refresh()
    {
        //await Domain.RefreshAdminItemsAsync();
    }
}
