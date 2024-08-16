using System.Collections.ObjectModel;
using DuQ.Core;
using DuQ.Models.Core;
using Microsoft.AspNetCore.Components;


namespace DuQ.Components.Pages.Status;

public partial class Index : IDisposable
{
    [Inject] private Domain? Domain { get; set; }
    [Inject] private DbSaveNotifier? DbSaveNotifier { get; set; }

    private List<QueueStatusDto> _statusItems = [];
    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        DbSaveNotifier!.OnDbSave += ReloadStatusItemsHandler;

        await LoadStatusItemsAsync();
    }

    public void Dispose()
    {
        DbSaveNotifier.OnDbSave -= ReloadStatusItemsHandler;
    }

    private async Task LoadStatusItemsAsync()
    {
        _isLoading = true;
        await this.InvokeAsync(StateHasChanged);

        _statusItems = await Domain.GetStatusItemsAsync();

        _isLoading = false;
        await this.InvokeAsync(StateHasChanged);
    }

    private async Task ReloadStatusItemsHandler()
    {
        _statusItems = await Domain.GetStatusItemsAsync();

        await this.InvokeAsync(StateHasChanged);
    }

    private string MinutesText(int minutes)
    {
        return minutes.ToString() + " minutes";
    }
}
