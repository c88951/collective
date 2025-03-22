using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollectiveData.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }
        
        [StringLength(50)]
        public string? Color { get; set; }
        
        public bool IsUserDefined { get; set; } = false;
        
        public int? UserId { get; set; }
        
        public int? CategoryId { get; set; }
        
        public TagCategory? Category { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }

        // Navigation property for artworks with this tag
        public List<ArtworkTag>? ArtworkTags { get; set; }
    }
}
