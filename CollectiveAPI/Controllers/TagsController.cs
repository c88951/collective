using CollectiveData.DTOs;
using CollectiveData.Helpers;
using CollectiveData.Models;
using CollectiveData.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CollectiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly ITagCategoryRepository _tagCategoryRepository;

        public TagsController(ITagRepository tagRepository, ITagCategoryRepository tagCategoryRepository)
        {
            _tagRepository = tagRepository;
            _tagCategoryRepository = tagCategoryRepository;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
        {
            var tags = await _tagRepository.GetAllAsync();
            return Ok(tags.Select(MappingHelper.ToTagDto));
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TagDto>> GetTag(int id)
        {
            var tag = await _tagRepository.GetTagWithArtworksAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return Ok(MappingHelper.ToTagDto(tag));
        }

        // GET: api/Tags/UserDefined
        [HttpGet("UserDefined")]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetUserDefinedTags([FromQuery] int? userId = null)
        {
            var tags = await _tagRepository.GetUserDefinedTagsAsync(userId);
            return Ok(tags.Select(MappingHelper.ToTagDto));
        }

        // GET: api/Tags/Artwork/5
        [HttpGet("Artwork/{artworkId}")]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTagsByArtwork(int artworkId)
        {
            var tags = await _tagRepository.GetTagsByArtworkAsync(artworkId);
            return Ok(tags.Select(MappingHelper.ToTagDto));
        }

        // POST: api/Tags
        [HttpPost]
        public async Task<ActionResult<TagDto>> CreateTag(TagCreateDto dto)
        {
            var (isValid, errors) = ValidationHelper.ValidateTagCreate(dto);
            if (!isValid)
            {
                return BadRequest(new { Errors = errors });
            }

            // Validate tag category if provided
            if (dto.CategoryId.HasValue)
            {
                var category = await _tagCategoryRepository.GetByIdAsync(dto.CategoryId.Value);
                if (category == null)
                {
                    return BadRequest(new { Error = "Invalid tag category" });
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

            return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, MappingHelper.ToTagDto(tag));
        }

        // PUT: api/Tags/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTag(int id, TagUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            // Validate tag category if provided
            if (dto.CategoryId.HasValue)
            {
                var category = await _tagCategoryRepository.GetByIdAsync(dto.CategoryId.Value);
                if (category == null)
                {
                    return BadRequest(new { Error = "Invalid tag category" });
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
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!await TagExists(id))
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

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tag = await _tagRepository.GetTagWithArtworksAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            // Check if tag is in use by any artworks
            if (tag.ArtworkTags != null && tag.ArtworkTags.Any())
            {
                return BadRequest(new { Error = "Cannot delete tag that is in use by artworks" });
            }

            // Only allow deletion of user-defined tags or if there's an override flag
            if (!tag.IsUserDefined)
            {
                return BadRequest(new { Error = "System tags cannot be deleted" });
            }

            await _tagRepository.DeleteAsync(tag);
            await _tagRepository.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> TagExists(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            return tag != null;
        }
    }
}
