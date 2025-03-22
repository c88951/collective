using CollectiveData.Models;

namespace CollectiveData.Repositories
{
    public interface ITagCategoryRepository : IRepository<TagCategory>
    {
        Task<IEnumerable<TagCategory>> GetTagCategoriesWithTagsAsync();
        Task<TagCategory?> GetTagCategoryWithTagsAsync(int id);
        Task<IEnumerable<TagCategory>> GetTagCategoriesWithTranslationsAsync();
        Task<TagCategory?> GetTagCategoryWithTranslationsAsync(int id);
        Task<TagCategory?> GetTagCategoryWithTagsAndTranslationsAsync(int id);
        Task<IEnumerable<TagCategoryTranslation>> GetTranslationsByLanguageCodeAsync(string languageCode);
    }
}
