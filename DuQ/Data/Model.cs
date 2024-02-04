using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DuQ.Data;

public class DuqContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<QueueType> QueueTypes { get; set; }
    public DbSet<QueueStatus> QueueStatuses { get; set; }
    public DbSet<Queue> Queue { get; set; }

    public DuqContext(DbContextOptions<DuqContext> options) : base(options)
    {
    }

    #region Required
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Student>()
            .Property(s => s.Id)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .Property(s => s.StudentId)
            .IsRequired();

        modelBuilder.Entity<Student>()
            .Property(s => s.StudentFirstName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Student>()
            .Property(s => s.StudentLastName)
            .HasMaxLength(50);

        modelBuilder.Entity<QueueType>()
            .HasKey(qt => qt.Id);

        modelBuilder.Entity<QueueType>()
            .Property(qt => qt.Id)
            .IsRequired();

        modelBuilder.Entity<QueueType>()
            .Property(qt => qt.QueueName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<QueueStatus>()
            .HasKey(qs => qs.Id);

        modelBuilder.Entity<QueueStatus>()
            .Property(qs => qs.Id)
            .IsRequired();

        modelBuilder.Entity<QueueStatus>()
            .Property(qs => qs.Status)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<Queue>()
            .HasKey(q => q.Id);

        modelBuilder.Entity<Queue>()
            .Property(q => q.Id)
            .IsRequired();
    }
    #endregion

}

public class Student
{
    public Guid Id { get; set; }
    public string StudentId { get; set; }
    public string StudentFirstName { get; set; }
    public string StudentLastName { get; set; }
}

public class QueueType
{
    public Guid Id { get; set; }
    public string? QueueName { get; set; }
}

public class QueueStatus
{
    public Guid Id { get; set; }
    public string? Status { get; set; }
}

public class Queue
{
    public long Id { get; set; }
    public QueueType? QueueType { get; set; }
    public Student? Student { get; set; }
    public QueueStatus? Status { get; set; }
}

public record CheckinModel
{
    [Required]
    public string StudentId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public Guid QueueTypeId { get; set; }
}
