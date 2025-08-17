using SchoolSync.Infra.Extensions;
using SchoolSync.App.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Infra layer
builder.Services.AddInfrastructureServices(builder.Configuration);
// App layer
builder.Services.AddAppServices();
// AutoMapper config for dtos
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(SchoolSync.App.MappingProfiles.UserProfile).Assembly);


var app = builder.Build();

await app.Services.SeedDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
