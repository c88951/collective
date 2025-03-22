using System;
using System.ComponentModel.DataAnnotations;

namespace CollectiveData.Models
{
    public class TagCategoryTranslation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string LanguageCode { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        // Foreign key
        public int TagCategoryId { get; set; }
        
        // Navigation property
        public TagCategory? TagCategory { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
}
