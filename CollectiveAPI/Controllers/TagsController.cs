using CollectiveData.DTOs;
using CollectiveData.Helpers;
using CollectiveData.Models;
using CollectiveData.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly ITagCategoryRepository _tagCategoryRepository;
        private readonly ILogger<TagsController> _logger;

        public TagsController(
            ITagRepository tagRepository, 
            ITagCategoryRepository tagCategoryRepository,
            ILogger<TagsController> logger)
        {
            _tagRepository = tagRepository;
            _tagCategoryRepository = tagCategoryRepository;
            _logger = logger;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TagDto>>>> GetTags()
        {
            _logger.LogInformation("Getting all tags");
            var tags = await _tagRepository.GetAllAsync();
            var tagDtos = tags.Select(MappingHelper.ToTagDto);
            _logger.LogInformation("Retrieved {TagCount} tags", tags.Count());
            return Ok(ApiResponse<IEnumerable<TagDto>>.Ok(tagDtos));
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TagDto>>> GetTag(int id)
        {
            _logger.LogInformation("Getting tag with ID {TagId}", id);
            var tag = await _tagRepository.GetTagWithArtworksAsync(id);

            if (tag == null)
            {
                _logger.LogWarning("Tag with ID {TagId} not found", id);
                return NotFound(ApiResponse<TagDto>.NotFound($"Tag with ID {id} not found"));
            }

            _logger.LogInformation("Retrieved tag {TagName} with ID {TagId}", tag.Name, id);
            return Ok(ApiResponse<TagDto>.Ok(MappingHelper.ToTagDto(tag)));
        }

        // GET: api/Tags/UserDefined
        [HttpGet("UserDefined")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TagDto>>>> GetUserDefinedTags([FromQuery] int? userId = null)
        {
            _logger.LogInformation("Getting user-defined tags {UserId}", userId.HasValue ? $"for user {userId}" : "for all users");
            var tags = await _tagRepository.GetUserDefinedTagsAsync(userId);
            var tagDtos = tags.Select(MappingHelper.ToTagDto);
            _logger.LogInformation("Retrieved {TagCount} user-defined tags", tags.Count());
            return Ok(ApiResponse<IEnumerable<TagDto>>.Ok(tagDtos));
        }

        // GET: api/Tags/Artwork/5
        [HttpGet("Artwork/{artworkId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TagDto>>>> GetTagsByArtwork(int artworkId)
        {
            _logger.LogInformation("Getting tags for artwork with ID {ArtworkId}", artworkId);
            var tags = await _tagRepository.GetTagsByArtworkAsync(artworkId);
            var tagDtos = tags.Select(MappingHelper.ToTagDto);
            _logger.LogInformation("Retrieved {TagCount} tags for artwork {ArtworkId}", tags.Count(), artworkId);
            return Ok(ApiResponse<IEnumerable<TagDto>>.Ok(tagDtos));
        }

        // POST: api/Tags
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TagDto>>> CreateTag(TagCreateDto dto)
        {
            _logger.LogInformation("Creating new tag {TagName}", dto.Name);
            
            var (isValid, errors) = ValidationHelper.ValidateTagCreate(dto);
            if (!isValid)
            {
                _logger.LogWarning("Tag validation failed for {TagName}: {Errors}", dto.Name, string.Join(", ", errors));
                return BadRequest(ApiResponse<TagDto>.BadRequest("Validation failed", errors));
            }

            // Validate tag category if provided
            if (dto.CategoryId.HasValue)
            {
                _logger.LogInformation("Validating tag category {CategoryId}", dto.CategoryId.Value);
                var category = await _tagCategoryRepository.GetByIdAsync(dto.CategoryId.Value);
                if (category == null)
                {
                    _logger.LogWarning("Invalid tag category {CategoryId}", dto.CategoryId.Value);
                    return BadRequest(ApiResponse<TagDto>.BadRequest("Invalid tag category"));
                }
            }

            var tag = new Tag
            {
                Name = dto.Name,
                Description = dto.Description,
                Color = dto.Color,
                CategoryId = dto.CategoryId,
                IsUserDefined = dto.IsUserDefined,
                UserId = dto.UserId,
                CreatedAt = DateTime.Now
            };

            await _tagRepository.AddAsync(tag);
            await _tagRepository.SaveChangesAsync();

            _logger.LogInformation("Created tag {TagName} with ID {TagId}", tag.Name, tag.Id);
            var tagDto = MappingHelper.ToTagDto(tag);
            return CreatedAtAction(
                nameof(GetTag), 
                new { id = tag.Id }, 
                ApiResponse<TagDto>.Created(tagDto, $"Tag '{tag.Name}' created successfully")
            );
        }

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TagDto>>> UpdateTag(int id, TagUpdateDto dto)
        {
            _logger.LogInformation("Updating tag with ID {TagId}", id);
            
            if (id != dto.Id)
            {
                _logger.LogWarning("ID mismatch: URL ID {UrlId} does not match body ID {BodyId}", id, dto.Id);
                return BadRequest(ApiResponse<TagDto>.BadRequest("ID in URL does not match ID in request body"));
            }

            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                _logger.LogWarning("Tag with ID {TagId} not found", id);
                return NotFound(ApiResponse<TagDto>.NotFound($"Tag with ID {id} not found"));
            }

            // Validate tag category if provided
            if (dto.CategoryId.HasValue)
            {
                _logger.LogInformation("Validating tag category {CategoryId}", dto.CategoryId.Value);
                var category = await _tagCategoryRepository.GetByIdAsync(dto.CategoryId.Value);
                if (category == null)
                {
                    _logger.LogWarning("Invalid tag category {CategoryId}", dto.CategoryId.Value);
                    return BadRequest(ApiResponse<TagDto>.BadRequest("Invalid tag category"));
                }
            }

            // Update tag properties
            tag.Name = dto.Name;
            tag.Description = dto.Description;
            tag.Color = dto.Color;
            tag.CategoryId = dto.CategoryId;
            tag.UpdatedAt = DateTime.Now;

            // Only allow updating IsUserDefined and UserId if the tag is already user-defined
            // This prevents system tags from being converted to user tags
            if (tag.IsUserDefined)
            {
                tag.IsUserDefined = dto.IsUserDefined;
                tag.UserId = dto.UserId;
            }

            await _tagRepository.UpdateAsync(tag);

            try
            {
                await _tagRepository.SaveChangesAsync();
                _logger.LogInformation("Updated tag {TagName} with ID {TagId}", tag.Name, tag.Id);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error while updating tag {TagId}", id);
                if (!await TagExists(id))
                {
                    _logger.LogWarning("Tag with ID {TagId} not found after concurrency exception", id);
                    return NotFound(ApiResponse<TagDto>.NotFound($"Tag with ID {id} not found"));
                }
                else
                {
                    throw;
                }
            }

            return Ok(ApiResponse<TagDto>.Ok(MappingHelper.ToTagDto(tag), $"Tag '{tag.Name}' updated successfully"));
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteTag(int id)
        {
            _logger.LogInformation("Deleting tag with ID {TagId}", id);
            
            var tag = await _tagRepository.GetTagWithArtworksAsync(id);
            if (tag == null)
            {
                _logger.LogWarning("Tag with ID {TagId} not found", id);
                return NotFound(ApiResponse<object>.NotFound($"Tag with ID {id} not found"));
            }

            // Check if tag is in use by any artworks
            if (tag.ArtworkTags != null && tag.ArtworkTags.Any())
            {
                _logger.LogWarning("Cannot delete tag {TagName} (ID: {TagId}) because it is in use by {ArtworkCount} artworks", 
                    tag.Name, tag.Id, tag.ArtworkTags.Count);
                return BadRequest(ApiResponse<object>.BadRequest("Cannot delete tag that is in use by artworks"));
            }

            // Only allow deletion of user-defined tags or if there's an override flag
            if (!tag.IsUserDefined)
            {
                _logger.LogWarning("Cannot delete system tag {TagName} (ID: {TagId})", tag.Name, tag.Id);
                return BadRequest(ApiResponse<object>.BadRequest("System tags cannot be deleted"));
            }

            await _tagRepository.DeleteAsync(tag);
            await _tagRepository.SaveChangesAsync();

            _logger.LogInformation("Deleted tag {TagName} with ID {TagId}", tag.Name, tag.Id);
            return Ok(ApiResponse<object>.NoContent());
        }

        private async Task<bool> TagExists(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            return tag != null;
        }
    }
}
