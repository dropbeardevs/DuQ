using System.Collections.ObjectModel;
using DuQ.Core;
using DuQ.Data;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Status;

public class Domain
{
    public ObservableCollection<DuQueueDto> QueueItems { get; set; } = [];

    private readonly IDbContextFactory<DuqContext> _contextFactory;
    private DbSaveNotifier _dbSaveNotifier;

    public Domain(IDbContextFactory<DuqContext> contextFactory, DbSaveNotifier dbSaveNotifier)
    {
        _contextFactory = contextFactory;
        _dbSaveNotifier = dbSaveNotifier;

        //_dbSaveNotifier.OnDbSave += UpdateQueueItemsHandler;

    }

    public void GetQueueItems()
    {
        using DuqContext context = _contextFactory.CreateDbContext();

        var items = context.DuQueues
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
            }).ToList();

        ObservableCollection<DuQueueDto> observableItems = new(items);

        QueueItems = observableItems;
    }

    private void UpdateQueueItemsHandler()
    {
        //GetQueueItems();

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

        //task.Wait();

        //var items = context.DuQueueTypes.Select(x => x).ToList();
    }
}
