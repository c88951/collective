using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CollectiveData.Models
{
    public class TagCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsRequired { get; set; } = false;

        public bool AllowMultiple { get; set; } = true;

        public int DisplayOrder { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }

        // Navigation property for tags in this category
        public List<Tag>? Tags { get; set; }

        // Navigation property for translations
        public List<TagCategoryTranslation>? Translations { get; set; }
    }
}
