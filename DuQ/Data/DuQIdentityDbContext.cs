using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Data;

public class DuQIdentityDbContext(DbContextOptions<DuQIdentityDbContext> options) : IdentityDbContext<DuQIdentityUser>(options)
{
}
