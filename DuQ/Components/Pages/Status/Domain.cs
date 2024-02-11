using System.Collections.ObjectModel;
using DuQ.Data;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Status;

public class Domain
{
    public ObservableCollection<DuQueueDto> QueueItems { get; private set; } = [];

    private readonly DuqContext _context;

    public Domain(DuqContext context)
    {
        _context = context;

        _context.SavedChanges += UpdateQueueItemsHandler;

    }

    public async Task GetQueueItemsAsync()
    {
        var items = await _context.DuQueues
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

        ObservableCollection<DuQueueDto> observableItems = new(items);

        QueueItems = observableItems;

        var temp = new DuQueueDto()
        {
            QueueId = 999999,
            StudentNo = "hahah",
            StudentFirstName = "hahah",
            QueueType = "hahah",
            QueueStatus = "In Queue",
            CheckinTime = DateTime.Now,
            CheckoutTime = DateTime.Now,
            LastUpdated = DateTime.Now,
        };

        QueueItems.Add(temp);
    }

    public void UpdateQueueItemsHandler(object? sender, SavedChangesEventArgs eventArgs)
    {
        var task = GetQueueItemsAsync();

        task.Wait();
    }
}
