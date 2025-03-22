using CollectiveData.DTOs;
using CollectiveData.Helpers;
using CollectiveData.Models;
using CollectiveData.Repositories;
using CollectiveData.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollectiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagCategoriesController : ControllerBase
    {
        private readonly ITagCategoryRepository _tagCategoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly CollectiveDbContext _dbContext;

        public TagCategoriesController(
            ITagCategoryRepository tagCategoryRepository, 
            ITagRepository tagRepository,
            CollectiveDbContext dbContext)
        {
            _tagCategoryRepository = tagCategoryRepository;
            _tagRepository = tagRepository;
            _dbContext = dbContext;
        }

        // GET: api/TagCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagCategoryDto>>> GetTagCategories()
        {
            var tagCategories = await _tagCategoryRepository.GetTagCategoriesWithTagsAsync();
            return Ok(tagCategories.Select(MappingHelper.ToTagCategoryDto));
        }

        // GET: api/TagCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TagCategoryDto>> GetTagCategory(int id)
        {
            var tagCategory = await _tagCategoryRepository.GetTagCategoryWithTagsAndTranslationsAsync(id);

            if (tagCategory == null)
            {
                return NotFound();
            }

            return Ok(MappingHelper.ToTagCategoryDto(tagCategory));
        }

        // GET: api/TagCategories/5/Tags
        [HttpGet("{id}/Tags")]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTagsByCategoryId(int id)
        {
            var tagCategory = await _tagCategoryRepository.GetByIdAsync(id);
            if (tagCategory == null)
            {
                return NotFound();
            }

            var tags = await _tagRepository.GetTagsByCategoryAsync(id);
            return Ok(tags.Select(MappingHelper.ToTagDto));
        }

        // GET: api/TagCategories/Translations/{languageCode}
        [HttpGet("Translations/{languageCode}")]
        public async Task<ActionResult<IEnumerable<TagCategoryTranslationDto>>> GetTranslationsByLanguageCode(string languageCode)
        {
            var translations = await _tagCategoryRepository.GetTranslationsByLanguageCodeAsync(languageCode);
            return Ok(translations.Select(MappingHelper.ToTagCategoryTranslationDto));
        }

        // POST: api/TagCategories
        [HttpPost]
        public async Task<ActionResult<TagCategoryDto>> CreateTagCategory(TagCategoryCreateDto dto)
        {
            var (isValid, errors) = ValidationHelper.ValidateTagCategoryCreate(dto);
            if (!isValid)
            {
                return BadRequest(new { Errors = errors });
            }

            var tagCategory = new TagCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                IsRequired = dto.IsRequired,
                AllowMultiple = dto.AllowMultiple,
                DisplayOrder = dto.DisplayOrder,
                CreatedAt = DateTime.Now
            };

            await _tagCategoryRepository.AddAsync(tagCategory);
            await _tagCategoryRepository.SaveChangesAsync();

            // Add translations if provided
            if (dto.Translations != null && dto.Translations.Any())
            {
                foreach (var translationDto in dto.Translations)
                {
                    var translation = new TagCategoryTranslation
                    {
                        TagCategoryId = tagCategory.Id,
                        LanguageCode = translationDto.LanguageCode,
                        Name = translationDto.Name,
                        Description = translationDto.Description,
                        CreatedAt = DateTime.Now
                    };

                    await _dbContext.TagCategoryTranslations.AddAsync(translation);
                }
                await _dbContext.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetTagCategory), new { id = tagCategory.Id }, MappingHelper.ToTagCategoryDto(tagCategory));
        }

        // PUT: api/TagCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTagCategory(int id, TagCategoryUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var tagCategory = await _tagCategoryRepository.GetTagCategoryWithTranslationsAsync(id);
            if (tagCategory == null)
            {
                return NotFound();
            }

            // Update tag category properties
            tagCategory.Name = dto.Name;
            tagCategory.Description = dto.Description;
            tagCategory.IsRequired = dto.IsRequired;
            tagCategory.AllowMultiple = dto.AllowMultiple;
            tagCategory.DisplayOrder = dto.DisplayOrder;
            tagCategory.UpdatedAt = DateTime.Now;

            await _tagCategoryRepository.UpdateAsync(tagCategory);

            // Handle translations
            if (dto.Translations != null && dto.Translations.Any())
            {
                // Get existing translations
                var existingTranslations = tagCategory.Translations?.ToList() ?? new List<TagCategoryTranslation>();
                
                foreach (var translationDto in dto.Translations)
                {
                    var existingTranslation = existingTranslations.FirstOrDefault(t => 
                        t.LanguageCode.Equals(translationDto.LanguageCode, StringComparison.OrdinalIgnoreCase));
                    
                    if (existingTranslation != null)
                    {
                        // Update existing translation
                        existingTranslation.Name = translationDto.Name;
                        existingTranslation.Description = translationDto.Description;
                        existingTranslation.UpdatedAt = DateTime.Now;
                        
                        _dbContext.Entry(existingTranslation).State = EntityState.Modified;
                    }
                    else
                    {
                        // Add new translation
                        var newTranslation = new TagCategoryTranslation
                        {
                            TagCategoryId = id,
                            LanguageCode = translationDto.LanguageCode,
                            Name = translationDto.Name,
                            Description = translationDto.Description,
                            CreatedAt = DateTime.Now
                        };
                        
                        await _dbContext.TagCategoryTranslations.AddAsync(newTranslation);
                    }
                }
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TagCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/TagCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTagCategory(int id)
        {
            var tagCategory = await _tagCategoryRepository.GetByIdAsync(id);
            if (tagCategory == null)
            {
                return NotFound();
            }

            // Check if any tags are using this category
            var tagsUsingCategory = await _tagRepository.GetTagsByCategoryAsync(id);
            if (tagsUsingCategory.Any())
            {
                return BadRequest(new { Error = "Cannot delete category that is in use by tags" });
            }

            await _tagCategoryRepository.DeleteAsync(tagCategory);
            await _tagCategoryRepository.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> TagCategoryExists(int id)
        {
            var tagCategory = await _tagCategoryRepository.GetByIdAsync(id);
            return tagCategory != null;
        }
    }
}
