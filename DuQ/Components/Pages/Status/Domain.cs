using System.Collections;
using DuQ.Data;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Status;

public class Domain(DuqContext context)
{
    public async Task<List<DuQueue>> GetQueueItemsAsync()
    {
        var items = await context.DuQueues.ToListAsync();





        return items;
    }
}
