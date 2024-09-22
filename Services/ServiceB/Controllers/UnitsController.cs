using Microsoft.AspNetCore.Mvc;
using ServiceB.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceB.Client;
using TestTask.Context;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using ServiceA.Models;
using ServiceA.Controllers;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TestTask.Context.Entities;
using System.Text.Json;

namespace ServiceB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly StatusServiceClient _statusServiceClient;
        private readonly IConfiguration _configuration;
        private readonly IDbContextFactory<AppDbContext> dbContextFactory;

        public UnitsController(AppDbContext context, StatusServiceClient statusServiceClient, IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _context = context;
            _statusServiceClient = statusServiceClient;
            this.dbContextFactory = dbContextFactory;
        }

        [HttpGet]
        //public async Task<IEnumerable<OtherUnitModel>> G
        public async Task<IEnumerable<OtherUnitModel>> GetUnits()
        {
            using var context = await dbContextFactory.CreateDbContextAsync();
            var units = await context.Units.ToListAsync();

            var result = new List<OtherUnitModel>();

            foreach (var x in units)
            {
                var status = await _statusServiceClient.GetStatusesAsync(x.Id);
                int m = 0;
                result.Add(new OtherUnitModel()
                {
                    Id = x.Id,
                    status = status.status,
                    Name = x.Name,
                    ParentId = x.ParentId,
                });
            }
            return result;
        }

        [HttpPut("{id:int}")]
        //public async Task<IEnumerable<OtherUnitModel>> G
        public async Task Update(int id,UpdateModel model)
        {
            using var context = await dbContextFactory.CreateDbContextAsync();

            var unity = await context.Units.Where(x => x.Id == id).FirstOrDefaultAsync();

            unity.ParentId = model.ParentId;


            context.Units.Update(unity);

            await context.SaveChangesAsync();
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncUnits(IEnumerable<OtherUnitModel> models)
        {
            // Проверка входных данных
            if (models == null || !models.Any())
            {
                return BadRequest("Модели не могут быть пустыми.");
            }

            // Логируем полученные данные
            Console.WriteLine("Полученные модели: " + JsonSerializer.Serialize(models));

            // Получаем статусы из сервиса A
            var units = await GetUnits();
            //Console.WriteLine("Полученные модели: " + JsonSerializer.Serialize(models));
            Console.WriteLine("Полученные модели: " + JsonSerializer.Serialize(units));
            foreach (var m in models)
            {
                bool updateFlag = false;

                foreach (var u in units)
                {
                    // Проверка на совпадение идентификаторов и других свойств
                    if (u.Id == m.Id && u.Name == m.Name && u.status == m.status && u.ParentId != m.ParentId)
                    {
                        await Update(m.Id, new UpdateModel() { ParentId = m.ParentId });
                        updateFlag = true;
                    }
                }

                // Если не было обновлений и нет совпадений, добавляем новый объект
                if (!updateFlag && !units.Any(u => u.Id == m.Id && u.Name == m.Name))
                {
                    var un = new Unit()
                    {
                        Id = m.Id,
                        Name = m.Name,
                        ParentId = m.ParentId,
                        status = m.status,
                    };
                    _context.Units.Add(un);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
