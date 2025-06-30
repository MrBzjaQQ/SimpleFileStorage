using Scalar.AspNetCore;
using SimpleFileStorage.Web;
using SimpleFileStorage.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StorageSettings>(builder.Configuration.GetSection("StorageSettings"));
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.ConfigureServices(connectionString);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MigrateDatabase();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();