using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Contexts;

public class DuQIdentityDbContext(DbContextOptions<DuQIdentityDbContext> options) : IdentityDbContext<DuQIdentityUser>(options)
{
}
