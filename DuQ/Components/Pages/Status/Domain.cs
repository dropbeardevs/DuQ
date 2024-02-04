using System.Collections;
using System.Runtime.CompilerServices;
using DuQ.Data;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Status;

public class Domain(DuqContext context)
{
    public async Task<List<DuQueue>> GetQueueItemsAsync()
    {
        var items = await context.DuQueues
            .Include(q => q.Student)
            .Include(q => q.QueueType)
            .Include(q => q.QueueStatus)
            .ToListAsync();

        return items;
    }
}
