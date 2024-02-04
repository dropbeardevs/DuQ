using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.FluentUI.AspNetCore.Components;

namespace DuQ.Data;

public class DuqContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<QueueType> QueueTypes { get; set; }
    public DbSet<QueueStatus> QueueStatuses { get; set; }
    public DbSet<DuQueue> DuQueues { get; set; }

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

        modelBuilder.Entity<DuQueue>()
            .HasKey(q => q.Id);

        modelBuilder.Entity<DuQueue>()
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
    public string? StudentLastName { get; set; }
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

public class DuQueue
{
    public long Id { get; set; }
    public QueueType? QueueType { get; set; }
    public Student? Student { get; set; }
    public QueueStatus? Status { get; set; }
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
