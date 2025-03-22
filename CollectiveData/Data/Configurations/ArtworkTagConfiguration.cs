using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollectiveData.Data.Configurations
{
    public class ArtworkTagConfiguration : IEntityTypeConfiguration<ArtworkTag>
    {
        public void Configure(EntityTypeBuilder<ArtworkTag> builder)
        {
            builder.HasKey(at => at.Id);
            
            // Configure relationship with ArtworkItem
            builder.HasOne(at => at.ArtworkItem)
                .WithMany(a => a.ArtworkTags)
                .HasForeignKey(at => at.ArtworkItemId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Configure relationship with Tag
            builder.HasOne(at => at.Tag)
                .WithMany(t => t.ArtworkTags)
                .HasForeignKey(at => at.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
