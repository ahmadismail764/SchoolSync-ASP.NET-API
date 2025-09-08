using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolSync.App.Extensions;
using SchoolSync.Domain.Entities;
using SchoolSync.Infra.Extensions;
using SchoolSync.API.Middleware;
using SchoolSync.Infra.Persistence;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings["Key"];
if (string.IsNullOrEmpty(jwtKey))
    throw new InvalidOperationException("JWT Key is missing in configuration.");
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "    JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
// Infra layer
builder.Services.AddInfrastructureServices(builder.Configuration);
// Application-Use Case layer
builder.Services.AddAppServices();
// Add password hasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(SchoolSync.App.MappingProfiles.UserProfile).Assembly);

var app = builder.Build();

// Handle database recreation in development
var isDevelopment = app.Environment.IsDevelopment();
var recreateOnStartup = app.Configuration.GetValue<bool>("Database:RecreateOnStartup", false);
var seedData = app.Configuration.GetValue<bool>("Database:SeedData", true);

if (isDevelopment && recreateOnStartup)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DBContext>();

    // Delete and recreate database
    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();

    Console.WriteLine("Database recreated for development environment.");
}

// Seed data if enabled
if (seedData)
{
    await app.Services.SeedDatabaseAsync();
}

// Add exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
