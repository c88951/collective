using CollectiveData.Models;

namespace CollectiveData.Repositories
{
    public interface IArtworkRepository : IRepository<ArtworkItem>
    {
        Task<IEnumerable<ArtworkItem>> GetArtworksByCollectionAsync(int collectionId);
        Task<IEnumerable<ArtworkItem>> GetArtworksByTagAsync(int tagId);
        Task<IEnumerable<ArtworkItem>> GetArtworksWithDetailsAsync();
        Task<ArtworkItem?> GetArtworkWithDetailsAsync(int id);
    }
}
