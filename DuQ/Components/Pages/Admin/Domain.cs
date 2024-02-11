using DuQ.Data;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Admin;

public class Domain
{
    private readonly DuqContext _context;
    private Queue<DuQueueDto> _queueItems;

    public Domain(DuqContext context)
    {
        _context = context;
        _queueItems = GetQueueItems();
    }

    private Queue<DuQueueDto> GetQueueItems()
    {
        Queue<DuQueueDto> queue = new();

        List<DuQueueDto> queueItems = _context.DuQueues
            .Where(x => x.QueueStatus.Status == "In Queue")
            .OrderByDescending(x => x.CheckinTime)
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
            .ToList();

        foreach (var item in queueItems)
        {
            queue.Enqueue(item);
        }

        return queue;
    }

    // public async Task Task<List<DuQueueDto>> GetQueueItemsAsync()
    // {
    //     var items = await context.DuQueues
    //         .Include(q => q.Student)
    //         .Include(q => q.QueueType)
    //         .Include(q => q.QueueStatus)
    //         .Select(item => new DuQueueDto()
    //         {
    //             QueueId = item.QueueId,
    //             StudentNo = item.Student.StudentNo,
    //             StudentFirstName = item.Student.FirstName,
    //             QueueType = item.QueueType.Name,
    //             QueueStatus = item.QueueStatus.Status,
    //             CheckinTime = item.CheckinTime,
    //             CheckoutTime = item.CheckoutTime,
    //             LastUpdated = item.LastUpdated
    //         }).ToListAsync();
    // }
}
