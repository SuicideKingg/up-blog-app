using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpBlogApp.DTO;
using UpBlogApp.Extensions;
using UpBlogApp.Models;

namespace UpBlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDBContext _appDbContext;

        public CommentController(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("createComment")]
        public async Task<IActionResult> CreateComment([FromBody] CreateUpdateCommentDTO createUpdateCommentDTO)
        {
            try
            {
                if (_appDbContext.Set<Article>().Where(s => s.ID == createUpdateCommentDTO.ArticleID).AsNoTracking().FirstOrDefault() == null)
                    return NotFound("Article Not found.");
                if (_appDbContext.Set<User>().Where(s => s.ID == createUpdateCommentDTO.UserID).AsNoTracking().FirstOrDefault() == null)
                    return NotFound("User not found.");

                _appDbContext.Comments.Add(createUpdateCommentDTO.ToCommentEntity());
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