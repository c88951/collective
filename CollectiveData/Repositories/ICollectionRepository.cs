using CollectiveData.Models;

namespace CollectiveData.Repositories
{
    public interface ICollectionRepository : IRepository<Collection>
    {
        Task<IEnumerable<Collection>> GetCollectionsWithArtworksAsync();
        Task<Collection?> GetCollectionWithArtworksAsync(int id);
    }
}
