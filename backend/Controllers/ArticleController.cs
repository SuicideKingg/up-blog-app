using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpBlogApp.DTO;
using UpBlogApp.Extensions;
using UpBlogApp.Models;

namespace UpBlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly AppDBContext _appDbContext;

        public ArticleController(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            List<RetrieveArticleDTO> list = await _appDbContext.Set<Article>().Select(article => new RetrieveArticleDTO
            {
                ID = article.ID,
                Title = article.Title, 
                Content = article.Content, 
                DatePosted = article.DatePosted,
                Author = article.User.UserName,
                AuthorID = article.User.ID,
                AuthorAvatarURL = article.User.AvaterURL,
                // Comments = article.Comments.Select(comment => new RetrieveCommentDTO
                // { 
                //     ID =  comment.ID, 
                //     Content = comment.Content, 
                //     DatePosted = comment.DatePosted,
                //     Author = comment.User.UserName,
                //     AuthorID = comment.User.ID,
                //     AuthorURLAvatar = comment.User.AvaterURL
                // }).ToList(),
                // Reactions = article.Reactions.Select(reaction => new RetrieveReactionDTO
                // {
                //     ID = reaction.ID,
                //     Author = reaction.User.UserName,
                //     AuthorID = reaction.User.ID,
                //     AuthorURLAvatar = reaction.User.AvaterURL,
                //     ReactionType = reaction.ReactionType
                // }).ToList()
            }).ToListAsync();

            return StatusCode(StatusCodes.Status200OK, list);
        }

        [HttpGet("getSingleArticle/{id}")]
        public async Task<IActionResult> GetSingleArticle(int id)
        {
            RetrieveArticleDTO? retrieveArticleDTO = await _appDbContext.Set<Article>().Select(article => new RetrieveArticleDTO
            {
                ID = article.ID,
                Title = article.Title,
                Content = article.Content,
                DatePosted = article.DatePosted,
                Author = article.User.UserName,
                AuthorID = article.User.ID,
                AuthorAvatarURL = article.User.AvaterURL,
                Comments = article.Comments.Select(comment => new RetrieveCommentDTO
                {
                    ID = comment.ID,
                    Content = comment.Content,
                    DatePosted = comment.DatePosted,
                    Author = comment.User.UserName,
                    AuthorID = comment.User.ID,
                    AuthorURLAvatar = comment.User.AvaterURL
                }).ToList(),
                Reactions = article.Reactions.Select(reaction => new RetrieveReactionDTO
                {
                    ID = reaction.ID,
                    Author = reaction.User.UserName,
                    AuthorID = reaction.User.ID,
                    AuthorURLAvatar = reaction.User.AvaterURL,
                    ReactionType = reaction.ReactionType
                }).ToList()
            }).Where(s => s.ID == id).FirstOrDefaultAsync();

            if(retrieveArticleDTO == null){
                return StatusCode(StatusCodes.Status404NotFound, "Article not found!");
            }

            return StatusCode(StatusCodes.Status200OK, retrieveArticleDTO);
        }

        [HttpPost("createArticle")]
        public async Task<IActionResult> CreateArticle([FromBody] CreateUpdateArticleDTO createUpdateArticleDTO)
        {
            try
            {
                if (_appDbContext.Set<User>().Where(s => s.ID == createUpdateArticleDTO.UserID).AsNoTracking().FirstOrDefault() == null)
                    return NotFound("User not found!");

                _appDbContext.Articles.Add(createUpdateArticleDTO.ToArticleEntity());
                await _appDbContext.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        [HttpPut("updateArticle")]
        public async Task<IActionResult> UpdateArticle([FromBody] CreateUpdateArticleDTO createUpdateArticleDTO)
        {
           try
            {
                if (_appDbContext.Set<Article>().Where(s => s.ID == createUpdateArticleDTO.ID).AsNoTracking().FirstOrDefault() == null)
                    return NotFound("Article Not found.");
                if (_appDbContext.Set<User>().Where(s => s.ID == createUpdateArticleDTO.UserID).AsNoTracking().FirstOrDefault() == null)
                    return NotFound("User not found.");

                _appDbContext.Articles.Update(createUpdateArticleDTO.ToArticleEntity());
                await _appDbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("deleteArticle/{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            try
            {
                Article? article = _appDbContext.Set<Article>().Where(s => s.ID == id).AsNoTracking().FirstOrDefault();
                if (article == null)
                {
                    return NotFound("Article not found.");
                }

                _appDbContext.Set<Article>().Remove(article);
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