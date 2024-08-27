using DuQ.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Contexts;

public class DuQIdentityDbContext(DbContextOptions<DuQIdentityDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}
