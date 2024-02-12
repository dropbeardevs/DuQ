using System.Collections.ObjectModel;
using DuQ.Core;
using DuQ.Data;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Status;

public partial class Index
{
    [Inject]
    private Status.Domain? Domain { get; set; }

    [Inject] private DbSaveNotifier DbSaveNotifier { get; set; } = new();
    private bool IsLoading { get; set; }
    private bool _isCancelled;

    protected override void OnInitialized()
    {
        _isCancelled = false;

        DbSaveNotifier.OnDbSave += LoadStatusItemsHandler;

        LoadStatusItems();
    }

    public void Dispose()
    {
        DbSaveNotifier.OnDbSave -= LoadStatusItemsHandler;
    }

    private void LoadStatusItems()
    {
        IsLoading = true;
        StateHasChanged();

        Domain!.GetQueueItems();

        IsLoading = false;
        StateHasChanged();
    }

    private void LoadStatusItemsHandler()
    {
        Domain!.GetQueueItems();

        this.InvokeAsync(StateHasChanged);
    }

}
