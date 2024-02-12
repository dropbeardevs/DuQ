using System.Collections.ObjectModel;
using DuQ.Core;
using DuQ.Data;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Status;

public class Domain
{
    private readonly IDbContextFactory<DuqContext> _contextFactory;

    public Domain(IDbContextFactory<DuqContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<DuQueueDto>> GetQueueItemsAsync()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        var items = await context.DuQueues
            .Include(q => q.Student)
            .Include(q => q.QueueType)
            .Include(q => q.QueueStatus)
            .Select(item => new DuQueueDto()
            {
                QueueId = item.QueueId,
                StudentNo = item.Student.StudentNo,
                StudentFirstName = item.Student.FirstName,
                QueueType = item.QueueType.Name,
                QueueStatus = item.QueueStatus.Status,
                CheckinTime = item.CheckinTime,
                CheckoutTime = item.CheckoutTime,
                LastUpdated = item.LastUpdated
            }).ToListAsync();

        return items;
    }
}
