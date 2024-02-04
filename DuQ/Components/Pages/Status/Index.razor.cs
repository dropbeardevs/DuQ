using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Status;

public partial class Index
{
    [Parameter] public string Name { get; set; } = default!;

    private bool IsLoading { get; set; }

    [Inject]
    private Status.Domain? Domain { get; set; }

    private List<DuQueue>? _queueItems;

    protected override async void OnInitialized()
    {
        IsLoading = true;
        StateHasChanged();

        _queueItems = await Domain!.GetQueueItemsAsync();

        var tempNAme = _queueItems.FirstOrDefault();

        Name = tempNAme!.Student.FirstName;

        IsLoading = false;
        StateHasChanged();
    }
}
