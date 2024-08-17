using System.Collections.ObjectModel;
using DuQ.Contexts;
using DuQ.Core;
using DuQ.Models.Core;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Status;

public class Domain
{
    private readonly IDbContextFactory<DuqContext> _contextFactory;
    private readonly DbSaveNotifier _dbSaveNotifier;

    public Domain(IDbContextFactory<DuqContext> contextFactory, DbSaveNotifier dbSaveNotifier)
    {
        _contextFactory = contextFactory;
        _dbSaveNotifier = dbSaveNotifier;
    }

    public async Task<List<QueueStatusDto>> GetStatusItemsAsync2()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        var waitTimes = await context.DuQueueWaitTimes.Select(item => new QueueStatusDto
                                                                      {
                                                                          QueueLocation = item.Location,
                                                                          QueueNoStudents = item.QueueNoStudents,
                                                                          QueueWaitTime = item.WaitTime,
                                                                          TotalWaitTime = item.QueueNoStudents * item.WaitTime,
                                                                      }).ToListAsync();

        return waitTimes;
    }

    public async Task<List<QueueStatusDto>> GetStatusItemsAsync()
    {
        await using DuqContext context = await _contextFactory.CreateDbContextAsync();

        var numberStudents = await context.DuQueues
                                    .Include(q => q.QueueLocation)
                                    .Include(q => q.QueueStatus)
                                    .Where(q => q.QueueStatus.Status == "In Queue")
                                    .CountAsync();

        var waitTimes = await context.DuQueueWaitTimes.Select(item => new QueueStatusDto
                                                                      {
                                                                          QueueLocation = item.Location,
                                                                          QueueNoStudents = item.QueueNoStudents,
                                                                          QueueWaitTime = item.WaitTime,
                                                                          TotalWaitTime = item.QueueNoStudents * item.WaitTime,
                                                                      }).ToListAsync();

        return waitTimes;
    }
}
