using CollectiveData.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectiveData.Helpers
{
    public static class ValidationHelper
    {
        public static (bool IsValid, List<string> Errors) ValidateArtworkCreate(ArtworkItemCreateDto dto)
        {
            var errors = new List<string>();

            // Required fields
            if (string.IsNullOrWhiteSpace(dto.Title))
                errors.Add("Title is required");

            if (string.IsNullOrWhiteSpace(dto.Artist))
                errors.Add("Artist is required");

            // Length validations
            if (dto.Title?.Length > 100)
                errors.Add("Title cannot exceed 100 characters");

            if (dto.Artist?.Length > 100)
                errors.Add("Artist cannot exceed 100 characters");

            if (dto.Description?.Length > 1000)
                errors.Add("Description cannot exceed 1000 characters");

            if (dto.Medium?.Length > 50)
                errors.Add("Medium cannot exceed 50 characters");

            if (dto.Dimensions?.Length > 50)
                errors.Add("Dimensions cannot exceed 50 characters");

            if (dto.Location?.Length > 100)
                errors.Add("Location cannot exceed 100 characters");

            if (dto.ImageUrl?.Length > 255)
                errors.Add("Image URL cannot exceed 255 characters");

            if (dto.ThumbnailUrl?.Length > 255)
                errors.Add("Thumbnail URL cannot exceed 255 characters");

            // Logical validations
            if (dto.Year.HasValue && (dto.Year < 0 || dto.Year > DateTime.Now.Year))
                errors.Add($"Year must be between 0 and {DateTime.Now.Year}");

            if (dto.AcquisitionPrice.HasValue && dto.AcquisitionPrice < 0)
                errors.Add("Acquisition price cannot be negative");

            if (dto.CurrentValue.HasValue && dto.CurrentValue < 0)
                errors.Add("Current value cannot be negative");

            if (dto.AcquisitionDate > DateTime.Now)
                errors.Add("Acquisition date cannot be in the future");

            return (errors.Count == 0, errors);
        }

        public static (bool IsValid, List<string> Errors) ValidateCollectionCreate(CollectionCreateDto dto)
        {
            var errors = new List<string>();

            // Required fields
            if (string.IsNullOrWhiteSpace(dto.Name))
                errors.Add("Name is required");

            // Length validations
            if (dto.Name?.Length > 100)
                errors.Add("Name cannot exceed 100 characters");

            if (dto.Description?.Length > 500)
                errors.Add("Description cannot exceed 500 characters");

            if (dto.ImageUrl?.Length > 255)
                errors.Add("Image URL cannot exceed 255 characters");

            return (errors.Count == 0, errors);
        }

        public static (bool IsValid, List<string> Errors) ValidateTagCreate(TagCreateDto dto)
        {
            var errors = new List<string>();

            // Required fields
            if (string.IsNullOrWhiteSpace(dto.Name))
                errors.Add("Name is required");

            // Length validations
            if (dto.Name?.Length > 50)
                errors.Add("Name cannot exceed 50 characters");

            if (dto.Description?.Length > 200)
                errors.Add("Description cannot exceed 200 characters");

            if (dto.Color?.Length > 50)
                errors.Add("Color cannot exceed 50 characters");

            return (errors.Count == 0, errors);
        }
        
        public static (bool IsValid, List<string> Errors) ValidateTagCategoryCreate(TagCategoryCreateDto dto)
        {
            var errors = new List<string>();

            // Required fields
            if (string.IsNullOrWhiteSpace(dto.Name))
                errors.Add("Name is required");

            // Length validations
            if (dto.Name?.Length > 50)
                errors.Add("Name cannot exceed 50 characters");

            if (dto.Description?.Length > 200)
                errors.Add("Description cannot exceed 200 characters");
            
            // Validate translations if provided
            if (dto.Translations != null && dto.Translations.Any())
            {
                // Check for duplicate language codes
                var languageCodes = dto.Translations.Select(t => t.LanguageCode.ToLower()).ToList();
                var duplicateLanguages = languageCodes
                    .GroupBy(lc => lc)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();
                    
                if (duplicateLanguages.Any())
                {
                    errors.Add($"Duplicate translations found for language(s): {string.Join(", ", duplicateLanguages)}");
                }
                
                // Validate each translation
                foreach (var translation in dto.Translations)
                {
                    var (isValid, translationErrors) = ValidateTagCategoryTranslationCreate(translation);
                    if (!isValid)
                    {
                        errors.AddRange(translationErrors.Select(e => $"Translation ({translation.LanguageCode}): {e}"));
                    }
                }
            }

            return (errors.Count == 0, errors);
        }
        
        public static (bool IsValid, List<string> Errors) ValidateTagCategoryTranslationCreate(TagCategoryTranslationCreateDto dto)
        {
            var errors = new List<string>();

            // Required fields
            if (string.IsNullOrWhiteSpace(dto.LanguageCode))
                errors.Add("Language code is required");
                
            if (string.IsNullOrWhiteSpace(dto.Name))
                errors.Add("Name is required");

            // Length validations
            if (dto.LanguageCode?.Length > 10)
                errors.Add("Language code cannot exceed 10 characters");
                
            if (dto.Name?.Length > 50)
                errors.Add("Name cannot exceed 50 characters");

            if (dto.Description?.Length > 200)
                errors.Add("Description cannot exceed 200 characters");

            return (errors.Count == 0, errors);
        }
    }
}
