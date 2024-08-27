using DuQ.Models.Core;
using DuQ.Models.DuQueue;
using Microsoft.AspNetCore.Components;

namespace DuQ.Components.Pages.Admin;

public partial class Index
{
    [Inject] private Domain? Domain { get; set; }

    [SupplyParameterFromForm] private WaitTimeAdminModel? Model { get; set; }



    protected override async Task OnParametersSetAsync()
    {
        Model ??= new WaitTimeAdminModel();

        List<WaitTimeDto> waitTimes = await Domain!.GetWaitTimesAsync();

        WaitTimeDto? hrcWaitTime = waitTimes!.FirstOrDefault(w => w.Location == "Hornets Resource Center");

        WaitTimeDto? arWaitTimeModel = waitTimes.FirstOrDefault(w => w.Location == "Admissions & Records Campus ID");

        WaitTimeDto? arWindowWaitTimeModel = waitTimes.FirstOrDefault(w => w.Location == "Admissions & Records Window");

        Model!.HrcWaitTimeId = hrcWaitTime!.Id;
        Model!.HrcNumStudents = hrcWaitTime.NumberStudents;
        Model!.HrcWaitTime = hrcWaitTime.WaitTime;
        Model!.HrcIsOpen = hrcWaitTime.IsOpen;
        Model!.ArWaitTimeId = arWaitTimeModel!.Id;
        Model!.ArNumStudents = arWaitTimeModel.NumberStudents;
        Model!.ArWaitTime = arWaitTimeModel.WaitTime;
        Model!.ArIsOpen = arWaitTimeModel.IsOpen;
        Model!.ArWindowWaitTimeId = arWindowWaitTimeModel!.Id;
        Model!.ArWindowNumStudents = arWindowWaitTimeModel.NumberStudents;
        Model!.ArWindowWaitTime = arWindowWaitTimeModel.WaitTime;
        Model!.ArWindowIsOpen = arWindowWaitTimeModel.IsOpen;

        // Model!.HrcWaitTimeId = _waitTimes[0].Id;
        //     Model!.HrcNoStudents = _waitTimes[0].QueueNoStudents;
        //     Model!.HrcWaitTime = _waitTimes[0].WaitTime;
        //     Model!.HrcIsOpen = _waitTimes[0].IsOpen;
        //     Model!.ArWaitTimeId = _waitTimes[1].Id;
        //     Model!.ArNoStudents = _waitTimes[1].QueueNoStudents;
        //     Model!.ArWaitTime = _waitTimes[1].WaitTime;
        //     Model!.ArIsOpen = _waitTimes[1].IsOpen;
        //     Model!.ArWindowWaitTimeId = _waitTimes[2].Id;
        //     Model!.ArWindowNoStudents = _waitTimes[2].QueueNoStudents;
        //     Model!.ArWindowWaitTime = _waitTimes[2].WaitTime;
        //     Model!.ArWindowIsOpen = _waitTimes[2].IsOpen;
    }

    private async Task SubmitAsync()
    {
        List<WaitTimeDto> waitTimes =
        [
            new WaitTimeDto
            {
                Id = Model!.HrcWaitTimeId,
                NumberStudents = Model.HrcNumStudents,
                WaitTime = Model.HrcWaitTime,
                IsOpen = Model.HrcIsOpen
            },
            new WaitTimeDto
            {
                Id = Model!.ArWaitTimeId,
                NumberStudents = Model.ArNumStudents,
                WaitTime = Model.ArWaitTime,
                IsOpen = Model.ArIsOpen
            },
            new WaitTimeDto
            {
                Id = Model!.ArWindowWaitTimeId,
                NumberStudents = Model.ArWindowNumStudents,
                WaitTime = Model.ArWindowWaitTime,
                IsOpen = Model.ArWindowIsOpen
            }
        ];

        // _waitTimes[0].Id = Model!.HrcWaitTimeId;
        // _waitTimes[0].QueueNoStudents = Model!.HrcNoStudents;
        // _waitTimes[0].WaitTime = Model!.HrcWaitTime;
        // _waitTimes[0].IsOpen = Model!.HrcIsOpen;
        // _waitTimes[1].Id = Model!.ArWaitTimeId;
        // _waitTimes[1].QueueNoStudents = Model!.ArNoStudents;
        // _waitTimes[1].WaitTime = Model!.ArWaitTime;
        // _waitTimes[1].IsOpen = Model!.ArIsOpen;
        // _waitTimes[2].Id = Model!.ArWindowWaitTimeId;
        // _waitTimes[2].QueueNoStudents = Model!.ArWindowNoStudents;
        // _waitTimes[2].WaitTime = Model!.ArWindowWaitTime;
        // _waitTimes[2].IsOpen = Model!.ArWindowIsOpen;

        await Domain!.UpdateWaitTimesAsync(waitTimes);
    }
}
