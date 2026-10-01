var builder = WebApplication.CreateBuilder(args);

// Add controller services
builder.Services.AddControllers();

// Add API explorer
builder.Services.AddEndpointsApiExplorer();

// Build application
var app = builder.Build();

// Map controller endpoints
app.MapControllers();

// Run API
app.Run();