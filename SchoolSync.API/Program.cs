using SchoolSync.Infra.Extensions;
using SchoolSync.App.Extensions;
using AutoMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAppServices();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(SchoolSync.App.MappingProfiles.UserProfile).Assembly);

var app = builder.Build();

// Seed the database
await app.Services.SeedDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
