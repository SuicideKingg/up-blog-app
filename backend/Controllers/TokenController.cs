using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpBlogApp.DTO;
using UpBlogApp.Services;

namespace UpBlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase{

        private readonly AppDBContext _appDBContext;
        private readonly ITokenService _tokenService;
        public TokenController(AppDBContext appDBContext, ITokenService tokenService)
        {
            _appDBContext = appDBContext;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity?.Name; //this is mapped to the Name claim by default

            var user = _appDBContext.User.Where(s => s.UserName == username).FirstOrDefault();

            if (user == null || user.RefreshToken != refreshToken)
                return BadRequest("Invalid client request");
    
            if(user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            // Update new token details of the current user.
            await _appDBContext.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                AvatarURL = user.AvaterURL,
                FullName = user.Name,
                Username = user.UserName,
                UserId = user.ID
            });
        }

        [HttpPost]
        [Route("revokeTokens")]
        public async Task<IActionResult> RevokeTokens(TokenModel tokenApiModel)
        {
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity?.Name; //this is mapped to the Name claim by default

            var user = _appDBContext.User.Where(s => s.UserName == username).FirstOrDefault();

            if (user == null || user.RefreshToken != refreshToken)
                return BadRequest("Invalid client request");
    
            if(user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            // Update new token details of the current user.
            await _appDBContext.SaveChangesAsync();

            return Ok();
        }
    }
}