using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollectiveData.Models
{
    public class ArtworkItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        public string Artist { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(50)]
        public string? Medium { get; set; }

        [StringLength(50)]
        public string? Dimensions { get; set; }

        public int? Year { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        public DateTime AcquisitionDate { get; set; } = DateTime.Now;

        public decimal? AcquisitionPrice { get; set; }

        public decimal? CurrentValue { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        [StringLength(255)]
        public string? ThumbnailUrl { get; set; }

        public bool IsPublic { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }

        // Foreign key for Collection
        public int? CollectionId { get; set; }
        public Collection? Collection { get; set; }

        // Navigation property for tags
        public List<ArtworkTag>? ArtworkTags { get; set; }
    }
}
