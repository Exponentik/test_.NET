using ServiceB.Client;
using ServiceB.Models;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using TestTask.Context;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Добавляем CORS политику
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// Добавляем контроллеры
builder.Services.AddControllers();

// Добавляем DbContext
builder.Services.AddDbContextFactory<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация HttpClient для StatusServiceClient
builder.Services.AddHttpClient<StatusServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5250"); 
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.ExecuteSqlRaw("CREATE SCHEMA IF NOT EXISTS public;");
    context.Database.Migrate();
}


// Middleware
app.UseCors("AllowAll");
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();