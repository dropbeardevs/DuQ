using DuQ.Core;
using DuQ.Data;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Admin;

public class Domain
{
    private readonly IDbContextFactory<DuqContext> _contextFactory;
    private readonly DbSaveNotifier _dbSaveNotifier;

    public Domain(IDbContextFactory<DuqContext> contextFactory, DbSaveNotifier dbSaveNotifier)
    {
        _contextFactory = contextFactory;
        _dbSaveNotifier = dbSaveNotifier;
    }


    // public async Task<AdminDto>? GetQueueItemAsync(Guid queueTypeId, Guid queueItemId)
    // {
    //     await using DuqContext context = await _contextFactory.CreateDbContextAsync();
    //
    //     context.SavedChanges += _dbSaveNotifier.NotifyDbSaved;
    //
    //     AdminDto? adminQueueItem = await context.DuQueues
    //                                            .Where(x => x.QueueType.Id == queueTypeId)
    //                                            .Where(x => x.Id == queueItemId)
    //                                            .Select(x => new AdminDto()
    //                                                         {
    //                                                             StudentNo = x.Student.StudentNo,
    //                                                             StudentFirstName = x.Student.FirstName,
    //                                                         })
    //                                            .FirstOrDefaultAsync();
    //
    //     return adminQueueItem!;
    // }

    /// <summary>
    ///
    /// </summary>
    /// <param name="queueItemId">Current ID of queue item</param>
    /// <returns>Guid of previous item</returns>
    // public async Task<Guid> GetNextQueueGuid(Guid queueItemId)
    // {
    //
    // }

    /// <summary>
    ///
    /// </summary>
    /// <param name="queueItemId">ID of previous queue item</param>
    // public async Task GetPreviousQueueItem(Guid queueItemId)
    // {
    //
    // }

    public async Task<List<AdminDto>> GetAdminItemsAsync()
    {
        //await RefreshAdminItemsAsync();

        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        List<AdminDto>? adminItems = await context.DuQueuePositions.Select(x => new AdminDto
                                                                           {
                                                                               StudentNo = x.Current.Student.StudentNo,
                                                                               StudentFirstName = x.Current.Student.FirstName,
                                                                               QueueName = x.Current.QueueType.Name
                                                                      }).ToListAsync();

        return adminItems;
    }

    public async Task RefreshAdminItemsAsync()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        context.SavedChanges += _dbSaveNotifier.NotifyDbSaved;

        // List<DuQueue> currentQueueItems = await context.DuQueues
        //                                                .Where(x => x.QueueStatus.Status == "In Queue")
        //                                                .Select(x => x)
        //                                                .ToListAsync();


        DuQueuePosition campusIdCardCurrent = await context.DuQueuePositions
                                                           .Where(x => x.QueueType.Name == "Campus ID Card")
                                                           .Select(x => x)
                                                           .FirstAsync();

        Guid campusIdCardCurrentId = await context.DuQueues
                                        .Where(x => x.QueueType.Name == "Campus ID Card")
                                        .Where(x => x.QueueStatus.Status == "Serving")
                                        .Select(x => x.Id)
                                        .FirstAsync();

        campusIdCardCurrent.Current.Id = campusIdCardCurrentId;

        Guid capAndGownCurrentId = await context.DuQueues
                                                .Where(x => x.QueueType.Name == "Cap and Gown")
                                                .Where(x => x.QueueStatus.Status == "Serving")
                                                .Select(x => x.Id)
                                                .FirstAsync();

        Guid otherCurrentId = await context.DuQueues
                                           .Where(x => x.QueueType.Name == "Other")
                                           .Where(x => x.QueueStatus.Status == "Serving")
                                           .Select(x => x.Id)
                                           .FirstAsync();


        List<DuQueuePosition> queuePositions = await context.DuQueuePositions.Select(x => x).ToListAsync();



    }
}
