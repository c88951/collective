using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollectiveData.Data.Configurations
{
    public class TagCategoryTranslationConfiguration : IEntityTypeConfiguration<TagCategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<TagCategoryTranslation> builder)
        {
            builder.HasKey(tct => tct.Id);
            
            builder.Property(tct => tct.LanguageCode)
                .IsRequired()
                .HasMaxLength(10);
                
            builder.Property(tct => tct.Name)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(tct => tct.Description)
                .HasMaxLength(200);
                
            builder.Property(tct => tct.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            // Create a unique index on the combination of TagCategoryId and LanguageCode
            builder.HasIndex(tct => new { tct.TagCategoryId, tct.LanguageCode })
                .IsUnique();
                
            // Relationship is defined in TagCategoryConfiguration
        }
    }
}
