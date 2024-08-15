using DuQ.Models.DuQueue;
using Microsoft.EntityFrameworkCore;

namespace DuQ.Contexts;

public class DuqContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<DuQueueStatus> DuQueueStatuses { get; set; }
    public DbSet<DuQueueLocation> DuQueueLocations { get; set; }
    public DbSet<DuQueue> DuQueues { get; set; }


    //public DbSet<DuQueuePosition> DuQueuePositions { get; set; }

    public DuqContext(DbContextOptions<DuqContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("duqueue");

        // Student
        modelBuilder.Entity<Student>()
                    .HasKey(s => s.Id);

        modelBuilder.Entity<Student>()
                    .Property(s => s.Id)
                    .IsRequired();

        modelBuilder.Entity<Student>()
                    .Property(s => s.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

        modelBuilder.Entity<Student>()
                    .Property(s => s.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

        modelBuilder.Entity<Student>()
                    .Property(s => s.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

        modelBuilder.Entity<Student>()
                    .Property(s => s.ModifiedUtc)
                    .IsRequired();

        // DuQueueStatus
        modelBuilder.Entity<DuQueueStatus>()
                    .HasKey(qs => qs.Id);

        modelBuilder.Entity<DuQueueStatus>()
                    .Property(qs => qs.Id)
                    .IsRequired();

        modelBuilder.Entity<DuQueueStatus>()
                    .Property(qs => qs.Status)
                    .IsRequired()
                    .HasMaxLength(50);

        modelBuilder.Entity<DuQueueStatus>()
                    .Property(qs => qs.ModifiedUtc)
                    .IsRequired();

        // DuQueueLocation
        modelBuilder.Entity<DuQueueLocation>()
                    .HasKey(qs => qs.Id);

        modelBuilder.Entity<DuQueueLocation>()
                    .Property(qs => qs.Id)
                    .IsRequired();

        modelBuilder.Entity<DuQueueLocation>()
                    .Property(qs => qs.Location)
                    .IsRequired()
                    .HasMaxLength(50);

        modelBuilder.Entity<DuQueueLocation>()
                    .Property(qs => qs.ModifiedUtc)
                    .IsRequired();

        // DuQueue
        modelBuilder.Entity<DuQueue>()
                    .HasKey(q => q.Id);

        modelBuilder.Entity<DuQueue>()
                    .Property(q => q.Id)
                    .IsRequired();

        modelBuilder.Entity<DuQueue>()
                    .Property(q => q.ModifiedUtc)
                    .IsRequired();
    }
}
