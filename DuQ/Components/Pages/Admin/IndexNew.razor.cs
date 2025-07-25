using DuQ.Core;
using DuQ.Models.Core;
using Microsoft.AspNetCore.Components;
using NodaTime;

namespace DuQ.Components.Pages.Admin;

public partial class IndexNew
{
    [Inject] private Domain Domain { get; set; } = default!;

    [Inject] private DbSaveNotifier? DbSaveNotifier { get; set; }

    [SupplyParameterFromForm] private AdminModel? Model { get; set; }

    [SupplyParameterFromQuery(Name = "loc")]
    public string? _location { get; set; }

    private List<string> _queueLocations = [];

    private List<AdminDto> _queueItems = null!;

    private AdminDto? _currentlyServing;

    private bool _isLoading;

    protected override async Task OnInitializedAsync()
    {
        DbSaveNotifier!.OnDbSave += ReloadQueueItemsHandler;

        await LoadStatusItems();

        _isLoading = false;
    }

    protected override async Task OnParametersSetAsync()
    {
        Model ??= new AdminModel();
        _queueLocations = await Domain.GetLocationsAsync();

        // Add All
        _queueLocations.Insert(0, "All");

        switch (_location)
        {
            case "AR":
                Model.Location = "A&R";
                break;
            case "HRC":
                Model.Location = "HRC";

                break;
            default:
                Model.Location = "All";
                break;
        }
    }

    private async Task LoadStatusItems()
    {
        _isLoading = true;
        //await InvokeAsync(StateHasChanged);

        _queueItems = await Domain.GetQueueItemsAsync();

        _isLoading = false;
        //await InvokeAsync(StateHasChanged);
    }

    private async Task ReloadQueueItemsHandler()
    {
        _queueItems = await Domain.GetQueueItemsAsync();

        await InvokeAsync(StateHasChanged);
    }

    private async Task ServeAsync(AdminDto item)
    {
        // Put item back in queue
        if (_currentlyServing is not null)
        {
            await Domain.SetStatusToInQueue(_currentlyServing.QueueId);
        }

        await Domain.SetStatusToServing(item.QueueId);

        _currentlyServing = item;

        await LoadStatusItems();
    }

    private async Task FinishAsync(AdminDto item)
    {
        await Domain.SetStatusToFinished(item.QueueId);

        _currentlyServing = null;

        await LoadStatusItems();
    }

    private async Task DeleteAsync(AdminDto item)
    {
        await Domain.SetStatusToDeleted(item.QueueId);

        await LoadStatusItems();
    }

    void ClearData()
    {
        Model!.Location = string.Empty;
        _currentlyServing = null;
    }

    // quick filter - filter globally across multiple columns with the same input
    private Func<AdminDto, bool> QuickFilter => x =>
    {
        if (Model!.Location is "All")
            return true;

        if (string.IsNullOrWhiteSpace(Model!.Location))
            return true;

        if (x.QueueLocation.Contains(Model!.Location, StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    };

    private string GetLocalTime(DateTime utcTime)
    {
        var timeZone = DateTimeZoneProviders.Tzdb["America/Los_Angeles"];

        var instant = Instant.FromDateTimeUtc(DateTime.SpecifyKind(utcTime, DateTimeKind.Utc));

        string result = instant.InZone(timeZone).ToDateTimeUnspecified().ToString("h:mm tt");

        return result;
    }
}
