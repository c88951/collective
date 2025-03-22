using CollectiveData.Data;
using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectiveData.Repositories
{
    public class ArtworkRepository : Repository<ArtworkItem>, IArtworkRepository
    {
        public ArtworkRepository(CollectiveDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ArtworkItem>> GetArtworksByCollectionAsync(int collectionId)
        {
            return await _dbSet
                .Where(a => a.CollectionId == collectionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtworkItem>> GetArtworksByTagAsync(int tagId)
        {
            return await _dbSet
                .Where(a => a.ArtworkTags != null && a.ArtworkTags.Any(at => at.TagId == tagId))
                .ToListAsync();
        }

        public async Task<IEnumerable<ArtworkItem>> GetArtworksWithDetailsAsync()
        {
            return await _dbSet
                .Include(a => a.Collection)
                .Include(a => a.ArtworkTags!)
                .ThenInclude(at => at.Tag)
                .ToListAsync();
        }

        public async Task<ArtworkItem?> GetArtworkWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(a => a.Collection)
                .Include(a => a.ArtworkTags!)
                .ThenInclude(at => at.Tag)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
