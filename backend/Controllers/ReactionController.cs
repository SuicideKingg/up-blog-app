using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpBlogApp.DTO;
using UpBlogApp.Extensions;
using UpBlogApp.Models;

namespace UpBlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly AppDBContext _appDbContext;

        public ReactionController(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("createReaction")]
        public async Task<IActionResult> CreateReaction([FromBody] CreateUpdateReactionDTO createUpdateReactionDTO)
        {
            try
            {
                if (_appDbContext.Set<Article>().Where(s => s.ID == createUpdateReactionDTO.ArticleID).AsNoTracking().FirstOrDefault() == null)
                    return NotFound("Article Not found.");
                if (_appDbContext.Set<User>().Where(s => s.ID == createUpdateReactionDTO.UserID).AsNoTracking().FirstOrDefault() == null)
                    return NotFound("User not found.");

                _appDbContext.Reactions.Add(createUpdateReactionDTO.ToReactionEntity());
                await _appDbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpDelete("removeReaction/{id}")]
        public async Task<IActionResult> RemoveReaction(int id)
        {
            try
            {
                Reaction? reaction = _appDbContext.Set<Reaction>().AsNoTracking().Where(s => s.ID == id).AsNoTracking().FirstOrDefault();

                if (reaction == null)
                    return NotFound("Reaction not found.");
                
                _appDbContext.Set<Reaction>().Remove(reaction);
                await _appDbContext.SaveChangesAsync();
                
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}