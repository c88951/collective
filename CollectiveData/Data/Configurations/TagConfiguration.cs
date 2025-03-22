using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollectiveData.Data.Configurations
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(t => t.Id);
            
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(t => t.Description)
                .HasMaxLength(200);
                
            builder.Property(t => t.Color)
                .HasMaxLength(50);
                
            builder.Property(t => t.IsUserDefined)
                .HasDefaultValue(false);
                
            builder.Property(t => t.UserId);
                
            builder.Property(t => t.CategoryId);
                
            builder.Property(t => t.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            // Create an index on CategoryId to improve query performance
            builder.HasIndex(t => t.CategoryId);
            
            // Create an index on Name for faster lookups
            builder.HasIndex(t => t.Name);
            
            // Create a composite index for user-defined tags
            builder.HasIndex(t => new { t.UserId, t.IsUserDefined });
        }
    }
}
