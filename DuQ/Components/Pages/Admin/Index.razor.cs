using DuQ.Models.Core;
using DuQ.Models.DuQueue;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Admin;

public partial class Index
{
    [Inject] private Domain? Domain { get; set; }

    [SupplyParameterFromForm] private WaitTimeModel? Model { get; set; }

    private List<DuQueueWaitTime> _waitTimes = [];

    protected override async Task OnParametersSetAsync()
    {
        Model ??= new WaitTimeModel();

        _waitTimes = await Domain!.GetWaitTimesAsync();

        Model!.HrcWaitTimeId = _waitTimes[0].Id;
        Model!.HrcNoStudents = _waitTimes[0].QueueNoStudents;
        Model!.HrcWaitTime = _waitTimes[0].WaitTime;
        Model!.ArWaitTimeId = _waitTimes[1].Id;
        Model!.ArNoStudents = _waitTimes[1].QueueNoStudents;
        Model!.ArWaitTime = _waitTimes[1].WaitTime;
    }

    private async Task SubmitAsync()
    {
        _waitTimes[0].Id = Model!.HrcWaitTimeId;
        _waitTimes[0].QueueNoStudents = Model!.HrcNoStudents;
        _waitTimes[0].WaitTime = Model!.HrcWaitTime;
        _waitTimes[1].Id = Model!.ArWaitTimeId;
        _waitTimes[1].QueueNoStudents = Model!.ArNoStudents;
        _waitTimes[1].WaitTime = Model!.ArWaitTime;

        await Domain!.UpdateWaitTimesAsync(_waitTimes);
    }
}
