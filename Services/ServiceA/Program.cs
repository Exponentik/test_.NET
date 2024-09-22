using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestTask.Context;

var builder = WebApplication.CreateBuilder(args);

// ��������� CORS ��������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

// ��������� �����������
builder.Services.AddControllers();
builder.Services.AddDbContextFactory<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ��������� CORS ��������
app.UseCors("AllowAll");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();