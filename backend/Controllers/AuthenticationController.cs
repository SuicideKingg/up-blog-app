using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpBlogApp.DTO;
using UpBlogApp.Extensions;
using UpBlogApp.Models;
using UpBlogApp.Services;

namespace UpBlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDBContext _appDBContext;
        private readonly ITokenService _tokenService;
        public AuthenticationController(AppDBContext appDBContext, ITokenService tokenService)
        {
            _appDBContext = appDBContext;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("clientlogin")]
        public async Task<IActionResult> ClientLogin([FromBody] LoginModel loginModel)
        {
            // TODO: It might not a good practice but in the future make login process more secure. Applying Microsoft.Identity would work.
            User? user = _appDBContext.User.Where(s => s.UserName == loginModel.UserName && s.LoginPassword == loginModel.Password).FirstOrDefault();
            
            if(user == null)
                return  Unauthorized();

            string accessToken = _tokenService.GenerateAccessToken(GetClaims(user));
            string refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(30);

            await _appDBContext.SaveChangesAsync();

            return Ok(new {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AvatarURL = user.AvaterURL,
                FullName = user.Name,
                Username = user.UserName,
                UserId = user.ID
            });
        }

        [HttpPost]
        [Route("clientregister")]
        public async Task<IActionResult> ClientRegister([FromBody] RegisterModel registerModel)
        {
            // Check if user exists,
            if(_appDBContext.User.Where(s => s.UserName == registerModel.UserName).Count() > 0){
                return StatusCode(StatusCodes.Status409Conflict, new Response("Error"," Username Already exists!"));
            }

            // Password if strong -- skip for now
            // Find a way to upload the avatar url. -- skip for now.
            
            User userToRegister = registerModel.ToUserEntity();

            string accessToken = _tokenService.GenerateAccessToken(GetClaims(userToRegister));
            string refreshToken = _tokenService.GenerateRefreshToken();

            userToRegister.RefreshToken = refreshToken;
            userToRegister.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(30);

            _appDBContext.User.Add(userToRegister);
            await _appDBContext.SaveChangesAsync();

            return Ok(new {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AvatarURL = userToRegister.AvaterURL,
                FullName = userToRegister.Name,
                Username = userToRegister.UserName,
                UserId = userToRegister.ID
            });
        }

        [HttpPost]
        [Authorize]
        [Route("clientAccountUpdate")]
        public async Task<IActionResult> ClientAccountUpdate([FromBody] AccountUpdateModel accountUpdateModel)
        {
            // Check if user exists,
            User? user = await _appDBContext.User.Where(s => s.ID == accountUpdateModel.Id).FirstOrDefaultAsync();
            if(user == null){
                return StatusCode(StatusCodes.Status404NotFound, new Response("Error"," User not found"));
            }

            // Check if the access token are match. --skip for now

            if(!string.IsNullOrWhiteSpace(accountUpdateModel.NewPassword))
            {
                // Check if the old password are correct
                if(user.LoginPassword != accountUpdateModel.OldPassword){
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response("Error"," Cannot update password. Old Password incorrect"));
                }

                // Check if the new password is not empty or not strong -- skip for now. Front end will handle it.
                user.LoginPassword = accountUpdateModel.NewPassword;
            }

            user.Name = accountUpdateModel.Name;
            user.Email = accountUpdateModel.Email;

            // New Access token
            string accessToken = _tokenService.GenerateAccessToken(GetClaims(user));

            await _appDBContext.SaveChangesAsync();
            
            return Ok(new {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
                AvatarURL = user.AvaterURL,
                FullName = user.Name,
                Username = user.UserName,
                UserId = user.ID
            });
        }

        [HttpGet]
        [Authorize]
        [Route("get-user/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            User? user = await _appDBContext.User.Where(s => s.ID == id).FirstOrDefaultAsync();
            if(user == null){
                return StatusCode(StatusCodes.Status404NotFound, new Response("Error"," User not found"));
            }

            return Ok(new {
                Id=user.ID,
                Name=user.Name,
                Email=user.Email
            });
        }

        // Private methods

        private List<Claim> GetClaims(User user)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
        }
    }
}