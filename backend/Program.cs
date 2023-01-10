global using Microsoft.OpenApi.Models;
global using Microsoft.EntityFrameworkCore;

using backend.services;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddTransient<IAdminService, AdminService>();
builder.Services.AddTransient<MarketDataService>();

// Configuring swagger
builder.Services.AddSwaggerGen(c => 
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CM Investment Analytics",
        Description = "My backend testing platform to retrieve organised market data."
    });
    //Securing swagger, requires bearer to test endpoints
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme 
    {
        Description = "Authorization using the bearer token from login. Type 'bearer {JWT token}'",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(Options => {
        Options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
);

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
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
