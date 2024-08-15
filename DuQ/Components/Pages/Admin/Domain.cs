using DuQ.Contexts;
using DuQ.Core;
using DuQ.Models.Core;
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

    public async Task<List<AdminDto>> GetQueueItemsAsync()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        var items = await context.DuQueues
                                 .Include(q => q.Student)
                                 .Include(q => q.QueueLocation)
                                 .Include(q => q.QueueStatus)
                                 .Select(item => new AdminDto()
                                                 {
                                                     QueueId = item.Id,
                                                     QueueLocationId = item.QueueLocation.Id,
                                                     StudentFirstName = item.Student.FirstName,
                                                     StudentLastName = item.Student.LastName,
                                                     StudentContactDetails = item.Student.ContactDetails,
                                                     QueueStatus = item.QueueStatus.Status,
                                                     CheckinTime = item.CheckinTime,
                                                     CheckoutTime = item.CheckoutTime,
                                                     Modified = item.ModifiedUtc
                                                 }).ToListAsync();

        return items;
    }

    public async Task RefreshAdminItemsAsync()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        context.SavedChanges += _dbSaveNotifier.NotifyDbSaved;

    }
}
