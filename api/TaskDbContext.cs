using Microsoft.EntityFrameworkCore;

namespace TaskTrackerFunctions;

public sealed class TaskDbContext(DbContextOptions<TaskDbContext> options) : DbContext(options)
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var task = modelBuilder.Entity<TaskItem>();

        task.ToTable("Tasks");
        task.HasKey(item => item.Id);
        task.Property(item => item.Id).ValueGeneratedNever();
        task.Property(item => item.Title).HasMaxLength(200).IsRequired();
        task.Property(item => item.Description).HasMaxLength(1000);
        task.Property(item => item.Status).HasMaxLength(50).IsRequired();
        task.Property(item => item.CreatedAtUtc)
            .HasConversion(
                value => value.UtcDateTime,
                value => new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Utc)))
            .HasColumnType("datetime2")
            .IsRequired();
    }
}
