using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SQLite;
using webapi.Data.Interfaces;
using webapi.DomainServices.Interfaces;
using webapi.DomainServices.Interfaces.ApiServices;
using webapi.Models;


namespace webapi.Controllers
{
    public class BaseController<T> : ControllerBase where T: class
    {
        protected readonly IConfiguration _config;
        protected readonly HttpContext _httpContext;
        protected readonly ICocktailApiManagerService _cocktailApiService;
        protected readonly ISqlLiteContext _sqLiteService;
        protected readonly ILogger<T> _logger;
        protected readonly IAzureBlobStorageService _blobStorageService;
        protected readonly ClaimsPrincipal _currentUser;
        protected readonly IMongoContext _mongoContext;

        public BaseController(IConfiguration config,
            IHttpContextAccessor httpContext,
            ICocktailApiManagerService cocktailApiService,
            ISqlLiteContext sqliteService,
            ILogger<T> logger,
            IAzureBlobStorageService blobStorageService
            )
        {
            _config = config;
            _httpContext = httpContext.HttpContext;
            _cocktailApiService = cocktailApiService;
            _sqLiteService = sqliteService;
            _logger = logger;
            _blobStorageService = blobStorageService;
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                _currentUser = _httpContext?.User; 
            }
        }

    }
}
