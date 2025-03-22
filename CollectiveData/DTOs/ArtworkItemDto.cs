using System;
using System.Collections.Generic;

namespace CollectiveData.DTOs
{
    public class ArtworkItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Medium { get; set; }
        public string? Dimensions { get; set; }
        public int? Year { get; set; }
        public string? Location { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public decimal? AcquisitionPrice { get; set; }
        public decimal? CurrentValue { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CollectionId { get; set; }
        public string? CollectionName { get; set; }
        public List<TagDto>? Tags { get; set; }
    }

    public class ArtworkItemCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Medium { get; set; }
        public string? Dimensions { get; set; }
        public int? Year { get; set; }
        public string? Location { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public decimal? AcquisitionPrice { get; set; }
        public decimal? CurrentValue { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsPublic { get; set; } = true;
        public int? CollectionId { get; set; }
        public List<int>? TagIds { get; set; }
    }

    public class ArtworkItemUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Medium { get; set; }
        public string? Dimensions { get; set; }
        public int? Year { get; set; }
        public string? Location { get; set; }
        public DateTime AcquisitionDate { get; set; }
        public decimal? AcquisitionPrice { get; set; }
        public decimal? CurrentValue { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool IsPublic { get; set; }
        public int? CollectionId { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
