
using SchoolSync.Infra.Extensions;
using SchoolSync.App.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAppServices();

var app = builder.Build();

// Seed the database
await app.Services.SeedDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
