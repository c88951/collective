using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollectiveData.Data.Configurations
{
    public class ArtworkItemConfiguration : IEntityTypeConfiguration<ArtworkItem>
    {
        public void Configure(EntityTypeBuilder<ArtworkItem> builder)
        {
            builder.HasKey(a => a.Id);
            
            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(a => a.Artist)
                .HasMaxLength(100);
                
            builder.Property(a => a.Description)
                .HasMaxLength(1000);
                
            builder.Property(a => a.Medium)
                .HasMaxLength(50);
                
            builder.Property(a => a.Dimensions)
                .HasMaxLength(50);
                
            builder.Property(a => a.Location)
                .HasMaxLength(100);
                
            builder.Property(a => a.ImageUrl)
                .HasMaxLength(255);
                
            builder.Property(a => a.ThumbnailUrl)
                .HasMaxLength(255);
                
            builder.Property(a => a.AcquisitionPrice)
                .HasColumnType("decimal(18,2)");
                
            builder.Property(a => a.CurrentValue)
                .HasColumnType("decimal(18,2)");
                
            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
                
            // Configure relationship with Collection
            builder.HasOne(a => a.Collection)
                .WithMany(c => c.ArtworkItems)
                .HasForeignKey(a => a.CollectionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
