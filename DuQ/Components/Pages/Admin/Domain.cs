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


    public async Task<AdminDto>? GetQueueItemAsync(Guid queueTypeId, Guid queueItemId)
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        context.SavedChanges += _dbSaveNotifier.NotifyDbSaved;

        AdminDto? adminQueueItem = await context.DuQueues
                                               .Where(x => x.QueueType.Id == queueTypeId)
                                               .Where(x => x.Id == queueItemId)
                                               .Select(x => new AdminDto()
                                                            {
                                                                StudentNo = x.Student.StudentNo,
                                                                StudentFirstName = x.Student.FirstName,
                                                            })
                                               .FirstOrDefaultAsync();

        return adminQueueItem!;
    }

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

    public async Task<Queue<DuQueueDto>> GetQueueItemsAsync()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        context.SavedChanges += _dbSaveNotifier.NotifyDbSaved;

        List<DuQueueDto> queueItems = await context.DuQueues
            .Where(x => x.QueueStatus.Status == "In Queue")
            .OrderBy(x => x.CheckinTime)
            .Select(x => new DuQueueDto()
            {
                QueueId = x.Id,
                StudentNo = x.Student.StudentNo,
                StudentFirstName = x.Student.FirstName,
                QueueType = x.QueueType.Name,
                QueueStatus = x.QueueStatus.Status,
                CheckinTime = x.CheckinTime,
                CheckoutTime = x.CheckoutTime,
                LastUpdated = x.LastUpdated
            })
            .ToListAsync();


        Queue<DuQueueDto> queue = new(queueItems);

        return queue;
    }
}
