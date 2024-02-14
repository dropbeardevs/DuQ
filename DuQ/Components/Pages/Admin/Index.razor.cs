using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Admin;

public partial class Index
{
    [Inject]
    private Domain Domain { get; set; } = default!;

    private DuQueueDto? CampusIdCard { get; set; } = default!;
    private DuQueueDto? CapAndGown { get; set; } = default!;
    private DuQueueDto? Other { get; set; } = default!;

    private List<DuQueueDto> _queueItems = null!;

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

        _queueItems = await Domain.GetQueueItemsAsync();

        CampusIdCard = _queueItems
            .FirstOrDefault(x => x.QueueType == "Campus ID Card");

        CapAndGown = _queueItems
            .FirstOrDefault(x => x.QueueType == "Cap and Gown");

        Other = _queueItems
            .FirstOrDefault(x => x.QueueType == "Other");

        _isLoading = false;
        StateHasChanged();
    }
}
