using System;
using System.Collections.Generic;

namespace CollectiveData.DTOs
{
    public class TagCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsRequired { get; set; }
        public bool AllowMultiple { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<TagDto>? Tags { get; set; }
        public List<TagCategoryTranslationDto>? Translations { get; set; }
    }

    public class TagCategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsRequired { get; set; } = false;
        public bool AllowMultiple { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
        public List<TagCategoryTranslationCreateDto>? Translations { get; set; }
    }

    public class TagCategoryUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsRequired { get; set; }
        public bool AllowMultiple { get; set; }
        public int DisplayOrder { get; set; }
        public List<TagCategoryTranslationUpdateDto>? Translations { get; set; }
    }

    public class TagCategoryTranslationDto
    {
        public int Id { get; set; }
        public int TagCategoryId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class TagCategoryTranslationCreateDto
    {
        public int TagCategoryId { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class TagCategoryTranslationUpdateDto
    {
        public int Id { get; set; }
        public string LanguageCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
