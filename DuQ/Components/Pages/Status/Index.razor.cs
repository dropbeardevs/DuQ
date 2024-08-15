using System.Collections.ObjectModel;
using DuQ.Core;
using DuQ.Models.Core;
using Microsoft.AspNetCore.Components;


namespace DuQ.Components.Pages.Status;

public partial class Index : IDisposable
{
    [Inject] private Status.Domain Domain { get; set; } = null!;
    [Inject] private DbSaveNotifier DbSaveNotifier { get; set; } = new();

    private List<QueueStatusDto> _statusItems = [];
    private bool IsLoading { get; set; }

    protected override async Task OnInitializedAsync()
    {
        DbSaveNotifier.OnDbSave += ReloadStatusItemsHandler;

        await LoadStatusItemsAsync();
    }

    public void Dispose()
    {
        DbSaveNotifier.OnDbSave -= ReloadStatusItemsHandler;
    }

    private async Task LoadStatusItemsAsync()
    {
        IsLoading = true;
        await this.InvokeAsync(StateHasChanged);

        //_statusItems = await Domain.GetQueueItemsAsync();

        IsLoading = false;
        await this.InvokeAsync(StateHasChanged);
    }

    private async Task ReloadStatusItemsHandler()
    {
        //_statusItems = await Domain.GetQueueItemsAsync();

        await this.InvokeAsync(StateHasChanged);

    }

}
