global using Microsoft.OpenApi.Models;
global using Microsoft.EntityFrameworkCore;
using backend.services;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//DBContext
var dbConnectionString = builder.Configuration.GetSection("ClientConfiguration").GetValue<string>("dBContextSecret");
var serverVersion = new MySqlServerVersion(new Version("8.0.31"));
builder.Services.AddDbContext<DbContext>(
    DbContextOptions => DbContextOptions
        .UseMySql(dbConnectionString, serverVersion));

builder.Services.AddSingleton<IFeatureFlagService, FeatureFlagService>();
builder.Services.AddSingleton<IMarketDataService, MarketDataService>();
builder.Services.AddSingleton<INewsService, NewsService>();
builder.Services.AddTransient<IUserService, UserService>();

// Configuring swagger
builder.Services.AddSwaggerGen(c => 
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CM Investment Analytics",
        Description = "My backend operation to retrieve organised market data."
    });
});


//Building
var app = builder.Build();

app.UseCors(
  options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
