using Interface;
using MongoDB.Driver;
using Repository;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(33001));

builder.Services.AddControllers();
builder.Services.AddSingleton<IMongoClient, MongoClient>();
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();
builder.Services.AddCors();

var app = builder.Build();

app.UseAuthorization();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.MapControllers();

app.Run();
