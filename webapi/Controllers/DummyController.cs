using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using webapi.DTOs;
using webapi.DomainServices.Interfaces.ApiServices;
using Microsoft.Extensions.Configuration;
using webapi.Data.Interfaces;
using webapi.DomainServices.Interfaces;
using Microsoft.AspNetCore.Http;
using webapi.Data.Exceptions;
using webapi.Data.Entities;
using System.Threading.Tasks;
using MongoDB.Driver;
using webapi.Data;

namespace webapi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Administrator")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DummyController : BaseController<DummyController>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public DummyController(IConfiguration config,
                        IHttpContextAccessor httpContext,
                   ICocktailApiManagerService cocktailApiService,
                   ISqlLiteContext sqliteService,
                   ILogger<DummyController> logger,
            IAzureBlobStorageService blobStorageService) : base(config, httpContext, cocktailApiService, sqliteService, logger, blobStorageService)
        {
        }

        /// <summary>
        ///  Method to return a list of weather elements
        /// </summary>
        /// <param name="id">the id</param>
        /// <returns>A list with the elements</returns>
        [HttpGet]
        [Route("{id?}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IEnumerable<DummyDTO>), (int)HttpStatusCode.OK)]

        public async Task<IActionResult> Get(int id)
        {
            try
            {
                //Two ways of working with Mongo Context. inyecting it to controller or creating a new one with some of Factory methods
                //var repository1 = MongoFactories.CreateRepository<IMongoContext, Cocktail>(mongoContext);
                using (var context = MongoFactories.CreateContext<MongoContext>(_config))
                {
                    var repository = context.CreateRepository<DummyDTO>();
                    repository.Dispose();
                }

                using (var context = MongoFactories.CreateContext<MongoContext>(_config))
                using (var repository = MongoFactories.CreateRepository<IMongoContext, Cocktail>(context))
                {
                    repository.Add(new Cocktail());
                }
            }
            catch (Exception ex)
            {

            }

            var kk = new { Name = "", Name2 = "", Name3 = 1 };
            var ll = kk.Name3;

            var rng = new Random();
            return Ok(Enumerable.Range(1, 5).Select(index => new DummyDTO
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
                .ToArray());
        }
    }
}
