using System.Collections.ObjectModel;
using DuQ.Contexts;
using DuQ.Core;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Components.Pages.Status;

public class Domain
{
    private readonly IDbContextFactory<DuqContext> _contextFactory;

    public Domain(IDbContextFactory<DuqContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


}
