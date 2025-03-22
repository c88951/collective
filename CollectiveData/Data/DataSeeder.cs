using CollectiveData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectiveData.Data
{
    public static class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new CollectiveDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<CollectiveDbContext>>());

            // Look for any existing data
            if (context.ArtworkItems.Any() || context.Collections.Any() || context.Tags.Any())
            {
                return; // Database has been seeded
            }

            // Seed tag categories
            var tagCategories = new List<TagCategory>
            {
                new TagCategory
                {
                    Name = "Style",
                    Description = "Art style or movement",
                    IsRequired = true,
                    AllowMultiple = true,
                    DisplayOrder = 1,
                    CreatedAt = DateTime.Now
                },
                new TagCategory
                {
                    Name = "Subject",
                    Description = "Main subject of the artwork",
                    IsRequired = false,
                    AllowMultiple = true,
                    DisplayOrder = 2,
                    CreatedAt = DateTime.Now
                },
                new TagCategory
                {
                    Name = "Medium",
                    Description = "Material used to create the artwork",
                    IsRequired = true,
                    AllowMultiple = false,
                    DisplayOrder = 3,
                    CreatedAt = DateTime.Now
                },
                new TagCategory
                {
                    Name = "Period",
                    Description = "Historical period of the artwork",
                    IsRequired = false,
                    AllowMultiple = false,
                    DisplayOrder = 4,
                    CreatedAt = DateTime.Now
                }
            };

            context.TagCategories.AddRange(tagCategories);
            context.SaveChanges();

            // Seed tag category translations
            var tagCategoryTranslations = new List<TagCategoryTranslation>
            {
                // Style translations
                new TagCategoryTranslation
                {
                    TagCategoryId = tagCategories[0].Id,
                    LanguageCode = "fr",
                    Name = "Style",
                    Description = "Style ou mouvement artistique",
                    CreatedAt = DateTime.Now
                },
                new TagCategoryTranslation
                {
                    TagCategoryId = tagCategories[0].Id,
                    LanguageCode = "es",
                    Name = "Estilo",
                    Description = "Estilo o movimiento artístico",
                    CreatedAt = DateTime.Now
                },
                
                // Subject translations
                new TagCategoryTranslation
                {
                    TagCategoryId = tagCategories[1].Id,
                    LanguageCode = "fr",
                    Name = "Sujet",
                    Description = "Sujet principal de l'œuvre d'art",
                    CreatedAt = DateTime.Now
                },
                new TagCategoryTranslation
                {
                    TagCategoryId = tagCategories[1].Id,
                    LanguageCode = "es",
                    Name = "Tema",
                    Description = "Tema principal de la obra de arte",
                    CreatedAt = DateTime.Now
                },
                
                // Medium translations
                new TagCategoryTranslation
                {
                    TagCategoryId = tagCategories[2].Id,
                    LanguageCode = "fr",
                    Name = "Support",
                    Description = "Matériel utilisé pour créer l'œuvre d'art",
                    CreatedAt = DateTime.Now
                },
                new TagCategoryTranslation
                {
                    TagCategoryId = tagCategories[2].Id,
                    LanguageCode = "es",
                    Name = "Medio",
                    Description = "Material utilizado para crear la obra de arte",
                    CreatedAt = DateTime.Now
                },
                
                // Period translations
                new TagCategoryTranslation
                {
                    TagCategoryId = tagCategories[3].Id,
                    LanguageCode = "fr",
                    Name = "Période",
                    Description = "Période historique de l'œuvre d'art",
                    CreatedAt = DateTime.Now
                },
                new TagCategoryTranslation
                {
                    TagCategoryId = tagCategories[3].Id,
                    LanguageCode = "es",
                    Name = "Período",
                    Description = "Período histórico de la obra de arte",
                    CreatedAt = DateTime.Now
                }
            };

            context.TagCategoryTranslations.AddRange(tagCategoryTranslations);
            context.SaveChanges();

            // Seed collections
            var collections = new List<Collection>
            {
                new Collection
                {
                    Name = "Impressionist Works",
                    Description = "A collection of impressionist paintings from the 19th century",
                    ImageUrl = "https://images.unsplash.com/photo-1579783902614-a3fb3927b6a5",
                    IsPublic = true,
                    CreatedAt = DateTime.Now
                },
                new Collection
                {
                    Name = "Modern Art",
                    Description = "Contemporary art pieces from the 20th and 21st centuries",
                    ImageUrl = "https://images.unsplash.com/photo-1536924940846-227afb31e2a5",
                    IsPublic = true,
                    CreatedAt = DateTime.Now
                },
                new Collection
                {
                    Name = "Photography",
                    Description = "Fine art photography collection",
                    ImageUrl = "https://images.unsplash.com/photo-1554048612-b6a482bc67e5",
                    IsPublic = true,
                    CreatedAt = DateTime.Now
                }
            };

            context.Collections.AddRange(collections);
            context.SaveChanges();

            // Seed tags
            var tags = new List<Tag>
            {
                new Tag { 
                    Name = "Impressionism", 
                    Description = "Art characterized by small, thin brush strokes",
                    Color = "#58A4B0",
                    CategoryId = tagCategories[0].Id, // Style
                    IsUserDefined = false,
                    CreatedAt = DateTime.Now
                },
                new Tag { 
                    Name = "Surrealism", 
                    Description = "Art that features elements of surprise and unexpected juxtapositions",
                    Color = "#A2D729",
                    CategoryId = tagCategories[0].Id, // Style
                    IsUserDefined = false,
                    CreatedAt = DateTime.Now
                },
                new Tag { 
                    Name = "Abstract", 
                    Description = "Art that does not attempt to represent an accurate depiction of visual reality",
                    Color = "#FF5A5F",
                    CategoryId = tagCategories[0].Id, // Style
                    IsUserDefined = false,
                    CreatedAt = DateTime.Now
                },
                new Tag { 
                    Name = "Landscape", 
                    Description = "Artwork depicting natural scenery",
                    Color = "#3C91E6",
                    CategoryId = tagCategories[1].Id, // Subject
                    IsUserDefined = false,
                    CreatedAt = DateTime.Now
                },
                new Tag { 
                    Name = "Portrait", 
                    Description = "Artwork depicting a person or group",
                    Color = "#9D8DF1",
                    CategoryId = tagCategories[1].Id, // Subject
                    IsUserDefined = false,
                    CreatedAt = DateTime.Now
                },
                new Tag { 
                    Name = "Oil on canvas", 
                    Description = "Painting technique using oil-based paints on canvas",
                    Color = "#F4B942",
                    CategoryId = tagCategories[2].Id, // Medium
                    IsUserDefined = false,
                    CreatedAt = DateTime.Now
                },
                new Tag { 
                    Name = "19th Century", 
                    Description = "Artwork created during the 19th century (1801-1900)",
                    Color = "#8B5FBF",
                    CategoryId = tagCategories[3].Id, // Period
                    IsUserDefined = false,
                    CreatedAt = DateTime.Now
                },
                new Tag { 
                    Name = "20th Century", 
                    Description = "Artwork created during the 20th century (1901-2000)",
                    Color = "#D81159",
                    CategoryId = tagCategories[3].Id, // Period
                    IsUserDefined = false,
                    CreatedAt = DateTime.Now
                },
                new Tag { 
                    Name = "Favorite", 
                    Description = "User's favorite artworks",
                    Color = "#FFD700",
                    IsUserDefined = true,
                    UserId = 1, // Example user ID
                    CreatedAt = DateTime.Now
                }
            };

            context.Tags.AddRange(tags);
            context.SaveChanges();

            // Seed artworks
            var artworks = new List<ArtworkItem>
            {
                new ArtworkItem
                {
                    Title = "Starry Night",
                    Artist = "Vincent van Gogh",
                    Description = "The Starry Night is an oil on canvas painting by Dutch Post-Impressionist painter Vincent van Gogh.",
                    Medium = "Oil on canvas",
                    Dimensions = "73.7 cm × 92.1 cm",
                    Year = 1889,
                    Location = "Main Gallery",
                    AcquisitionDate = new DateTime(2020, 3, 15),
                    AcquisitionPrice = 15000000,
                    CurrentValue = 17500000,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ea/Van_Gogh_-_Starry_Night_-_Google_Art_Project.jpg/1200px-Van_Gogh_-_Starry_Night_-_Google_Art_Project.jpg",
                    ThumbnailUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ea/Van_Gogh_-_Starry_Night_-_Google_Art_Project.jpg/300px-Van_Gogh_-_Starry_Night_-_Google_Art_Project.jpg",
                    IsPublic = true,
                    CreatedAt = DateTime.Now,
                    CollectionId = collections[0].Id
                },
                new ArtworkItem
                {
                    Title = "The Persistence of Memory",
                    Artist = "Salvador Dalí",
                    Description = "The Persistence of Memory is a 1931 painting by artist Salvador Dalí and one of the most recognizable works of Surrealism.",
                    Medium = "Oil on canvas",
                    Dimensions = "24 cm × 33 cm",
                    Year = 1931,
                    Location = "Modern Art Room",
                    AcquisitionDate = new DateTime(2019, 7, 22),
                    AcquisitionPrice = 8500000,
                    CurrentValue = 10000000,
                    ImageUrl = "https://uploads6.wikiart.org/images/salvador-dali/the-persistence-of-memory-1931.jpg",
                    ThumbnailUrl = "https://uploads6.wikiart.org/00142/images/salvador-dali/the-persistence-of-memory-1931.jpg!PinterestSmall.jpg",
                    IsPublic = true,
                    CreatedAt = DateTime.Now,
                    CollectionId = collections[1].Id
                },
                new ArtworkItem
                {
                    Title = "Water Lilies",
                    Artist = "Claude Monet",
                    Description = "Water Lilies is a series of approximately 250 oil paintings by French Impressionist Claude Monet.",
                    Medium = "Oil on canvas",
                    Dimensions = "200 cm × 180 cm",
                    Year = 1906,
                    Location = "Impressionist Gallery",
                    AcquisitionDate = new DateTime(2021, 1, 10),
                    AcquisitionPrice = 12000000,
                    CurrentValue = 14000000,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a0/Claude_Monet_-_Water_Lilies_-_1906%2C_Ryerson.jpg",
                    ThumbnailUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a0/Claude_Monet_-_Water_Lilies_-_1906%2C_Ryerson.jpg/320px-Claude_Monet_-_Water_Lilies_-_1906%2C_Ryerson.jpg",
                    IsPublic = true,
                    CreatedAt = DateTime.Now,
                    CollectionId = collections[0].Id
                }
            };

            context.ArtworkItems.AddRange(artworks);
            context.SaveChanges();

            // Seed artwork tags
            var artworkTags = new List<ArtworkTag>
            {
                // Starry Night tags
                new ArtworkTag { ArtworkItemId = artworks[0].Id, TagId = tags[0].Id }, // Impressionism
                new ArtworkTag { ArtworkItemId = artworks[0].Id, TagId = tags[3].Id }, // Landscape
                new ArtworkTag { ArtworkItemId = artworks[0].Id, TagId = tags[5].Id }, // Oil on canvas
                new ArtworkTag { ArtworkItemId = artworks[0].Id, TagId = tags[6].Id }, // 19th Century
                
                // The Persistence of Memory tags
                new ArtworkTag { ArtworkItemId = artworks[1].Id, TagId = tags[1].Id }, // Surrealism
                new ArtworkTag { ArtworkItemId = artworks[1].Id, TagId = tags[5].Id }, // Oil on canvas
                new ArtworkTag { ArtworkItemId = artworks[1].Id, TagId = tags[7].Id }, // 20th Century
                new ArtworkTag { ArtworkItemId = artworks[1].Id, TagId = tags[8].Id }, // Favorite (user-defined)
                
                // Water Lilies tags
                new ArtworkTag { ArtworkItemId = artworks[2].Id, TagId = tags[0].Id }, // Impressionism
                new ArtworkTag { ArtworkItemId = artworks[2].Id, TagId = tags[3].Id }, // Landscape
                new ArtworkTag { ArtworkItemId = artworks[2].Id, TagId = tags[5].Id }, // Oil on canvas
                new ArtworkTag { ArtworkItemId = artworks[2].Id, TagId = tags[7].Id }  // 20th Century
            };

            context.ArtworkTags.AddRange(artworkTags);
            context.SaveChanges();
        }
    }
}
