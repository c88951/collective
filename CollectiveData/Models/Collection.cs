using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollectiveData.Models
{
    public class Collection
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(255)]
        public string? ImageUrl { get; set; }
        
        public bool IsPublic { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }

        // Navigation property for artworks in this collection
        public List<ArtworkItem>? ArtworkItems { get; set; }
    }
}
