using System;
using System.Collections.Generic;

namespace CollectiveData.DTOs
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public bool IsUserDefined { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ArtworkCount { get; set; }
    }

    public class TagCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public bool IsUserDefined { get; set; } = false;
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
    }

    public class TagUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Color { get; set; }
        public bool IsUserDefined { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
    }
}
