using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollectiveData.Data.Configurations
{
    public class TagCategoryConfiguration : IEntityTypeConfiguration<TagCategory>
    {
        public void Configure(EntityTypeBuilder<TagCategory> builder)
        {
            builder.HasKey(tc => tc.Id);
            
            builder.Property(tc => tc.Name)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(tc => tc.Description)
                .HasMaxLength(200);
                
            builder.Property(tc => tc.IsRequired)
                .HasDefaultValue(false);
                
            builder.Property(tc => tc.AllowMultiple)
                .HasDefaultValue(true);
                
            builder.Property(tc => tc.DisplayOrder)
                .HasDefaultValue(0);
                
            builder.Property(tc => tc.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            // Relationships
            builder.HasMany(tc => tc.Tags)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.HasMany(tc => tc.Translations)
                .WithOne(tct => tct.TagCategory)
                .HasForeignKey(tct => tct.TagCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
