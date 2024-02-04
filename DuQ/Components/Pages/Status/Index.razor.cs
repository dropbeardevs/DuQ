using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Status;

public partial class Index
{
    [Inject]
    private Status.Domain? Domain { get; set; }

    private IEnumerable<DuQueue>? _queueItems;


    protected override async void OnInitialized()
    {
        _queueItems = await Domain!.GetQueueItemsAsync();
    }
}
