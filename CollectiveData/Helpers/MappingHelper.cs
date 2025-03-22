using CollectiveData.DTOs;
using CollectiveData.Models;
using System.Linq;

namespace CollectiveData.Helpers
{
    public static class MappingHelper
    {
        // ArtworkItem mappings
        public static ArtworkItemDto ToDto(this ArtworkItem artwork)
        {
            return new ArtworkItemDto
            {
                Id = artwork.Id,
                Title = artwork.Title,
                Artist = artwork.Artist,
                Description = artwork.Description,
                Medium = artwork.Medium,
                Dimensions = artwork.Dimensions,
                Year = artwork.Year,
                Location = artwork.Location,
                AcquisitionDate = artwork.AcquisitionDate,
                AcquisitionPrice = artwork.AcquisitionPrice,
                CurrentValue = artwork.CurrentValue,
                ImageUrl = artwork.ImageUrl,
                ThumbnailUrl = artwork.ThumbnailUrl,
                IsPublic = artwork.IsPublic,
                CreatedAt = artwork.CreatedAt,
                UpdatedAt = artwork.UpdatedAt,
                CollectionId = artwork.CollectionId,
                CollectionName = artwork.Collection?.Name,
                Tags = artwork.ArtworkTags?.Select(at => at.Tag?.ToTagDto()).Where(t => t != null).ToList()
            };
        }

        public static ArtworkItem ToEntity(this ArtworkItemCreateDto dto)
        {
            return new ArtworkItem
            {
                Title = dto.Title,
                Artist = dto.Artist,
                Description = dto.Description,
                Medium = dto.Medium,
                Dimensions = dto.Dimensions,
                Year = dto.Year,
                Location = dto.Location,
                AcquisitionDate = dto.AcquisitionDate,
                AcquisitionPrice = dto.AcquisitionPrice,
                CurrentValue = dto.CurrentValue,
                ImageUrl = dto.ImageUrl,
                ThumbnailUrl = dto.ThumbnailUrl,
                IsPublic = dto.IsPublic,
                CollectionId = dto.CollectionId,
                CreatedAt = System.DateTime.Now
            };
        }

        public static void UpdateFromDto(this ArtworkItem artwork, ArtworkItemUpdateDto dto)
        {
            artwork.Title = dto.Title;
            artwork.Artist = dto.Artist;
            artwork.Description = dto.Description;
            artwork.Medium = dto.Medium;
            artwork.Dimensions = dto.Dimensions;
            artwork.Year = dto.Year;
            artwork.Location = dto.Location;
            artwork.AcquisitionDate = dto.AcquisitionDate;
            artwork.AcquisitionPrice = dto.AcquisitionPrice;
            artwork.CurrentValue = dto.CurrentValue;
            artwork.ImageUrl = dto.ImageUrl;
            artwork.ThumbnailUrl = dto.ThumbnailUrl;
            artwork.IsPublic = dto.IsPublic;
            artwork.CollectionId = dto.CollectionId;
            artwork.UpdatedAt = System.DateTime.Now;
        }

        // Collection mappings
        public static CollectionDto ToDto(this Collection collection)
        {
            return new CollectionDto
            {
                Id = collection.Id,
                Name = collection.Name,
                Description = collection.Description,
                ImageUrl = collection.ImageUrl,
                IsPublic = collection.IsPublic,
                CreatedAt = collection.CreatedAt,
                UpdatedAt = collection.UpdatedAt,
                ArtworkCount = collection.ArtworkItems?.Count ?? 0,
                Artworks = collection.ArtworkItems?.Select(a => a.ToDto()).ToList()
            };
        }

        public static Collection ToEntity(this CollectionCreateDto dto)
        {
            return new Collection
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                IsPublic = dto.IsPublic,
                CreatedAt = System.DateTime.Now
            };
        }

        public static void UpdateFromDto(this Collection collection, CollectionUpdateDto dto)
        {
            collection.Name = dto.Name;
            collection.Description = dto.Description;
            collection.ImageUrl = dto.ImageUrl;
            collection.IsPublic = dto.IsPublic;
            collection.UpdatedAt = System.DateTime.Now;
        }

        // Tag mappings
        public static TagDto ToTagDto(this Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                Color = tag.Color,
                IsUserDefined = tag.IsUserDefined,
                UserId = tag.UserId,
                CategoryId = tag.CategoryId,
                CategoryName = tag.Category?.Name,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt,
                ArtworkCount = tag.ArtworkTags?.Count ?? 0
            };
        }

        public static Tag ToEntity(this TagCreateDto dto)
        {
            return new Tag
            {
                Name = dto.Name,
                Description = dto.Description,
                Color = dto.Color,
                IsUserDefined = dto.IsUserDefined,
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                CreatedAt = System.DateTime.Now
            };
        }

        public static void UpdateFromDto(this Tag tag, TagUpdateDto dto)
        {
            tag.Name = dto.Name;
            tag.Description = dto.Description;
            tag.Color = dto.Color;
            tag.CategoryId = dto.CategoryId;
            tag.UpdatedAt = System.DateTime.Now;
        }
        
        // TagCategory mappings
        public static TagCategoryDto ToTagCategoryDto(this TagCategory category)
        {
            return new TagCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsRequired = category.IsRequired,
                AllowMultiple = category.AllowMultiple,
                DisplayOrder = category.DisplayOrder,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                Tags = category.Tags?.Select(t => t.ToTagDto()).ToList(),
                Translations = category.Translations?.Select(t => t.ToTagCategoryTranslationDto()).ToList()
            };
        }
        
        public static TagCategory ToEntity(this TagCategoryCreateDto dto)
        {
            return new TagCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                IsRequired = dto.IsRequired,
                AllowMultiple = dto.AllowMultiple,
                DisplayOrder = dto.DisplayOrder,
                CreatedAt = System.DateTime.Now,
                Translations = dto.Translations?.Select(t => t.ToEntity()).ToList()
            };
        }
        
        public static void UpdateFromDto(this TagCategory category, TagCategoryUpdateDto dto)
        {
            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IsRequired = dto.IsRequired;
            category.AllowMultiple = dto.AllowMultiple;
            category.DisplayOrder = dto.DisplayOrder;
            category.UpdatedAt = System.DateTime.Now;
        }
        
        // TagCategoryTranslation mappings
        public static TagCategoryTranslationDto ToTagCategoryTranslationDto(this TagCategoryTranslation translation)
        {
            return new TagCategoryTranslationDto
            {
                Id = translation.Id,
                TagCategoryId = translation.TagCategoryId,
                LanguageCode = translation.LanguageCode,
                Name = translation.Name,
                Description = translation.Description,
                CreatedAt = translation.CreatedAt,
                UpdatedAt = translation.UpdatedAt
            };
        }
        
        public static TagCategoryTranslation ToEntity(this TagCategoryTranslationCreateDto dto)
        {
            return new TagCategoryTranslation
            {
                TagCategoryId = dto.TagCategoryId,
                LanguageCode = dto.LanguageCode,
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = System.DateTime.Now
            };
        }
        
        public static void UpdateFromDto(this TagCategoryTranslation translation, TagCategoryTranslationUpdateDto dto)
        {
            translation.LanguageCode = dto.LanguageCode;
            translation.Name = dto.Name;
            translation.Description = dto.Description;
            translation.UpdatedAt = System.DateTime.Now;
        }
    }
}
