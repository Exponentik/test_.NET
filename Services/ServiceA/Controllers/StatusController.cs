using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceA.Models;
using ServiceA.Models;
using System.Collections.Generic;
using TestTask.Context;

namespace ServiceA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public StatusController(IConfiguration configuration, IDbContextFactory<AppDbContext> dbContextFactory)
        {
            this._configuration = configuration;
            this.dbContextFactory = dbContextFactory;
        }
        [HttpGet]
        public async Task<UnitStatusModel> GetStatusById([FromHeader] string apiKey, int Id)
        {
            var allowedApiKey = _configuration.GetValue<string>("AllowedApiKey");
            Console.WriteLine($"Received API Key: {apiKey}, Allowed API Key: {allowedApiKey}");

            if (apiKey != allowedApiKey)
            {
                Console.WriteLine("Invalid API Key.");
                return null; // Возвращаем 401 Unauthorized
            }

            using var context = await dbContextFactory.CreateDbContextAsync();
            var units = await context.Units.ToListAsync();
            var u = units.FirstOrDefault(x => x.Id == Id);

            if (u == null)
            {
                Console.WriteLine($"Unit with Id {Id} not found.");
                return null; // Возвращаем 404 Not Found
            }
            return new UnitStatusModel(){ status = u.status};
        }
        //[HttpGet]
        //public async Task<IEnumerable<UnitStatusModel>> GetAllStatuses([FromHeader] string apiKey)
        //{
        //    var allowedApiKey = _configuration.GetValue<string>("AllowedApiKey");

        //    if (apiKey != allowedApiKey)
        //    {
        //        return null;
        //    }
        //    using var context = await dbContextFactory.CreateDbContextAsync();
        //    var units = await context.Units.ToListAsync();

        //    var result = units.Select(x => new UnitStatusModel(){
        //        Id = x.Id,
        //        Status = x.Status,
        //    });

        //    return result;
        //}
        //[HttpPost]
        //public async Task<UnitStatusModel> Create(UnitStatusModel model)
        //{
        //    using var context = await dbContextFactory.CreateDbContextAsync();



        //}
    }
}