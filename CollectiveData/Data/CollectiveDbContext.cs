using CollectiveData.Data.Configurations;
using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectiveData.Data
{
    public class CollectiveDbContext : DbContext
    {
        public CollectiveDbContext(DbContextOptions<CollectiveDbContext> options)
            : base(options)
        {
        }

        public DbSet<ArtworkItem> ArtworkItems { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArtworkTag> ArtworkTags { get; set; }
        public DbSet<TagCategory> TagCategories { get; set; }
        public DbSet<TagCategoryTranslation> TagCategoryTranslations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new ArtworkItemConfiguration());
            modelBuilder.ApplyConfiguration(new CollectionConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new ArtworkTagConfiguration());
            modelBuilder.ApplyConfiguration(new TagCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new TagCategoryTranslationConfiguration());
        }
    }
}
