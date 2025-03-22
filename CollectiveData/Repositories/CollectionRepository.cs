using CollectiveData.Data;
using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectiveData.Repositories
{
    public class CollectionRepository : Repository<Collection>, ICollectionRepository
    {
        public CollectionRepository(CollectiveDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Collection>> GetCollectionsWithArtworksAsync()
        {
            return await _dbSet
                .Include(c => c.ArtworkItems)
                .ToListAsync();
        }

        public async Task<Collection?> GetCollectionWithArtworksAsync(int id)
        {
            return await _dbSet
                .Include(c => c.ArtworkItems)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
