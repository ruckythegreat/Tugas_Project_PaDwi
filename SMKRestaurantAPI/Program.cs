using SMKRestaurantAPI.Data;
using SMKRestaurantAPI.Middleware;
using SMKRestaurantAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Dependency Injection — Data & Services
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();

var app = builder.Build();

// Global error handling — harus paling atas
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
