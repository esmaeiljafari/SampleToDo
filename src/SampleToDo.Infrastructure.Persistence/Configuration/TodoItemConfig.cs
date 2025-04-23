using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleToDo.Domain.Entities;

namespace SampleToDo.Infrastructure.Persistence.Configuration;

public class TodoItemConfig : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable(nameof(TodoItem));
        builder.Property(q => q.Title).HasMaxLength(50).IsRequired(false);
        builder.Property(q => q.Description).HasMaxLength(50).IsRequired(false);
    }
}