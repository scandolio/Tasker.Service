using Interface;
using MongoDB.Driver;
using Repository;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(33001));

builder.Services.AddControllers();
builder.Services.AddSingleton<IMongoClient, MongoClient>();
builder.Services.AddSingleton<ITaskRepository, TaskRepository>();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();
