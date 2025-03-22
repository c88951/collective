using CollectiveData.Data;
using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectiveData.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(CollectiveDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tag>> GetTagsWithArtworksAsync()
        {
            return await _dbSet
                .Include(t => t.ArtworkTags!)
                .ThenInclude(at => at.ArtworkItem)
                .Include(t => t.Category)
                .ToListAsync();
        }

        public async Task<Tag?> GetTagWithArtworksAsync(int id)
        {
            return await _dbSet
                .Include(t => t.ArtworkTags!)
                .ThenInclude(at => at.ArtworkItem)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tag>> GetTagsByArtworkAsync(int artworkId)
        {
            return await _context.Tags
                .Where(t => t.ArtworkTags != null && t.ArtworkTags.Any(at => at.ArtworkItemId == artworkId))
                .Include(t => t.Category)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Tag>> GetTagsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(t => t.CategoryId == categoryId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Tag>> GetUserDefinedTagsAsync(int? userId = null)
        {
            var query = _dbSet.Where(t => t.IsUserDefined);
            
            if (userId.HasValue)
            {
                query = query.Where(t => t.UserId == userId);
            }
            
            return await query.ToListAsync();
        }
        
        public override async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Category)
                .ToListAsync();
        }
    }
}
