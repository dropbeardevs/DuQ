using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DuQ.Data;

public class DuqContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<DuQueueType> DuQueueTypes { get; set; }
    public DbSet<DuQueueStatus> DuQueueStatuses { get; set; }
    public DbSet<DuQueue> DuQueues { get; set; }

    public DuqContext(DbContextOptions<DuqContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Student>()
            .Property(s => s.Id)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .Property(s => s.StudentNo)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Student>()
            .Property(s => s.LastName)
            .HasMaxLength(50);

        modelBuilder.Entity<Student>()
            .Property(s => s.LastUpdated)
            .IsRequired();

        modelBuilder.Entity<DuQueueType>()
            .HasKey(qt => qt.Id);

        modelBuilder.Entity<DuQueueType>()
            .Property(qt => qt.Id)
            .IsRequired();

        modelBuilder.Entity<DuQueueType>()
            .Property(qt => qt.Name)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<DuQueueType>()
            .Property(qt => qt.LastUpdated)
            .IsRequired();

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
            .Property(qs => qs.LastUpdated)
            .IsRequired();

        modelBuilder.Entity<DuQueue>()
            .HasKey(q => q.Id);

        modelBuilder.Entity<DuQueue>()
            .Property(q => q.Id)
            .IsRequired();

        modelBuilder.Entity<DuQueue>()
            .Property(q => q.LastUpdated)
            .IsRequired();

        // modelBuilder.Entity<DuQueue>()
        //     .Property(q => q.QueueType)
        //     .IsRequired();

        // modelBuilder.Entity<DuQueue>()
        //     .Property(q => q.QueueStatus)
        //     .IsRequired();
    }
}

public class Student
{
    public Guid Id { get; set; }
    public required string StudentNo { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime LastUpdated { get; set; }

    public List<DuQueue>? DuQueues { get; } = [];
}

public class DuQueueType
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid? Previous { get; set; }
    public Guid? Current { get; set; }
    public Guid? Next { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<DuQueue>? DuQueues { get; } = [];
}

public class DuQueueStatus
{
    public Guid Id { get; set; }
    public required string Status { get; set; }
    public DateTime LastUpdated { get; set; }

    public List<DuQueue>? DuQueues { get; } = [];
}

public class DuQueue
{
    public Guid Id { get; set; }
    public required Student Student { get; set; }
    public required DuQueueType QueueType { get; set; }
    public required DuQueueStatus QueueStatus { get; set; }
    public DateTime CheckinTime { get; set; }
    public DateTime CheckoutTime { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class CheckinModel
{
    [Required(ErrorMessage = "Student ID is required")]
    [MinLength(3, ErrorMessage = "Student ID is too short")]
    [StringLength(20, ErrorMessage = "Student ID is too long (20 character limit).")]
    public string? StudentId { get; set; }
    [Required (ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name too long (50 character limit).")]
    public string? FirstName { get; set; }
    [Required (ErrorMessage = "Service Type is required")]
    public string? QueueType { get; set; }
}

public class DuQueueDto
{
    public Guid QueueId { get; set; }
    public required string StudentNo { get; set; }
    public required string StudentFirstName { get; set; }
    public required string QueueType { get; set; }
    public required string QueueStatus { get; set; }
    public DateTime CheckinTime { get; set; }
    public DateTime CheckoutTime { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class AdminDto
{
    public Guid Id { get; set; }
    public required string StudentNo { get; set; }
    public required string StudentFirstName { get; set; }
}
