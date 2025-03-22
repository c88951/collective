using CollectiveData.Models;

namespace CollectiveData.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<IEnumerable<Tag>> GetTagsWithArtworksAsync();
        Task<Tag?> GetTagWithArtworksAsync(int id);
        Task<IEnumerable<Tag>> GetTagsByArtworkAsync(int artworkId);
        Task<IEnumerable<Tag>> GetTagsByCategoryAsync(int categoryId);
        Task<IEnumerable<Tag>> GetUserDefinedTagsAsync(int? userId = null);
    }
}
