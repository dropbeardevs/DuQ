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

    public async Task<List<DuQueueDto>> GetQueueItemsAsync()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        context.SavedChanges += _dbSaveNotifier.NotifyDbSaved;

        List<DuQueueDto> queueItems = await context.DuQueues
            .Where(x => x.QueueStatus.Status == "In Queue")
            .OrderBy(x => x.CheckinTime)
            .Select(x => new DuQueueDto()
            {
                QueueId = x.QueueId,
                StudentNo = x.Student.StudentNo,
                StudentFirstName = x.Student.FirstName,
                QueueType = x.QueueType.Name,
                QueueStatus = x.QueueStatus.Status,
                CheckinTime = x.CheckinTime,
                CheckoutTime = x.CheckoutTime,
                LastUpdated = x.LastUpdated
            })
            .ToListAsync();

        return queueItems;
    }
}
