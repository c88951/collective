namespace CollectiveData.Models
{
    public class ArtworkTag
    {
        public int Id { get; set; }
        
        // Foreign key for ArtworkItem
        public int ArtworkItemId { get; set; }
        public ArtworkItem? ArtworkItem { get; set; }
        
        // Foreign key for Tag
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
