using AegisInt.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Reimbit.Application;
using Reimbit.Application.Jwt;
using Reimbit.Infrastructure;
using Reimbit.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// EncryptedInt services
builder.Services.AddAegisInt(options =>
{
    options.Salt = "my-super-secret-production-salt"; // Required
    options.Alphabet = "aGbIJK3VWXYcfgmn14opL!MNO#Sqr2D-FEstuhBC9ijk67lvPQRw5dTUexy8zAHZ0"; // optional
    options.MinLength = 6; // optional
});

// Add services to the container.
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Authentication/Authorization
var jwt = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
var key = Encoding.UTF8.GetBytes(jwt.Secret);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Register services from all layers
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddWebComponentsService(builder.Configuration);

// Discover controllers from the Web assembly
builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(WebConfiguration).Assembly);

// OpenAPI JSON
builder.Services.AddOpenApi();

var app = builder.Build();

// Expose OpenAPI JSON in all environments
app.MapOpenApi();

app.UseHttpsRedirection();

// Authentication before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();