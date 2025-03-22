using CollectiveData.Data;
using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectiveData.Repositories
{
    public class TagCategoryRepository : Repository<TagCategory>, ITagCategoryRepository
    {
        public TagCategoryRepository(CollectiveDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TagCategory>> GetTagCategoriesWithTagsAsync()
        {
            return await _dbSet
                .Include(tc => tc.Tags)
                .OrderBy(tc => tc.DisplayOrder)
                .ToListAsync();
        }

        public async Task<TagCategory?> GetTagCategoryWithTagsAsync(int id)
        {
            return await _dbSet
                .Include(tc => tc.Tags)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<IEnumerable<TagCategory>> GetTagCategoriesWithTranslationsAsync()
        {
            return await _dbSet
                .Include(tc => tc.Translations)
                .OrderBy(tc => tc.DisplayOrder)
                .ToListAsync();
        }

        public async Task<TagCategory?> GetTagCategoryWithTranslationsAsync(int id)
        {
            return await _dbSet
                .Include(tc => tc.Translations)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<TagCategory?> GetTagCategoryWithTagsAndTranslationsAsync(int id)
        {
            return await _dbSet
                .Include(tc => tc.Tags)
                .Include(tc => tc.Translations)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<IEnumerable<TagCategoryTranslation>> GetTranslationsByLanguageCodeAsync(string languageCode)
        {
            return await _context.TagCategoryTranslations
                .Where(tct => tct.LanguageCode.ToLower() == languageCode.ToLower())
                .Include(tct => tct.TagCategory)
                .ToListAsync();
        }
    }
}
