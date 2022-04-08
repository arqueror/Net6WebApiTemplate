using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using webapi.Controllers;
using webapi.Data;
using webapi.Data.Entities;
using webapi.Data.Exceptions;
using webapi.Data.Interfaces;
using webapi.DomainServices.Interfaces;
using webapi.DomainServices.Interfaces.ApiServices;
using webapi.Models;

namespace webapi
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController<AuthController>
    {

        public AuthController(IConfiguration config,
            IHttpContextAccessor httpContext,
            ICocktailApiManagerService cocktailApiService,
            ISqlLiteContext sqliteService,
            ILogger<AuthController> logger,
            IAzureBlobStorageService blobStorageService) : base(config, httpContext, cocktailApiService, sqliteService, logger, blobStorageService)
        {

            var response = _cocktailApiService.GetRandomCocktail().ContinueWith(async x =>
            {

            });

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserModel login)
        {
            if (ModelState.IsValid)
            {

            }

            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok($"Bearer {tokenString}");
            }
            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtConfig:secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Name, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, "localhost:44314"),
                new Claim(JwtRegisteredClaimNames.Iat, "localhost:44314"),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            var token = new JwtSecurityToken("localhost:44314",
                _config["localhost:44314"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;
            //Validate the User Credentials    
            //Demo Purpose, I have Passed HardCoded User Information    
            if (login.Username == "aaa")
            {
                user = new UserModel { Username = "Jignesh Trivedi", EmailAddress = "test.btest@gmail.com" };
            }

            return user;
        }
    }
}
