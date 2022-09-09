using Interface;
using MongoDB.Driver;

namespace Repository;

public class TaskRepository : ITaskRepository {
  private readonly IMongoCollection<Model.TaskRoot> collection;

  public TaskRepository(IMongoClient client) {
    var database = client.GetDatabase("tasker");
    collection = database.GetCollection<Model.TaskRoot>(nameof(Model.TaskRoot));
  }

  public async System.Threading.Tasks.Task Create(DTO.Task task) {
    await collection.InsertOneAsync(new Model.TaskRoot() { Root = task });
  }

  public async Task<DTO.Task?> FindOpen(string family) {
    var filter = Builders<Model.TaskRoot>.Filter.And(
        Builders<Model.TaskRoot>.Filter.Eq(r => r.Root.Completed, null),
        Builders<Model.TaskRoot>.Filter.Eq(r => r.Root.Family, family));

    var docs = collection.Find(filter);
    var result = await docs.FirstOrDefaultAsync();
    return result?.Root;
  }

  public async Task<DTO.Task?> FindLatest(string family) {
    var sort = Builders<Model.TaskRoot>.Sort.Descending(r => r.Root.Completed);
    var filter = Builders<Model.TaskRoot>.Filter.And(
        Builders<Model.TaskRoot>.Filter.Ne(r => r.Root.Completed, null),
        Builders<Model.TaskRoot>.Filter.Eq(r => r.Root.Family, family));
    var result =
        await collection.Find(filter).Sort(sort).Limit(1).FirstOrDefaultAsync();
    return result?.Root;
  }

  public async Task Update(string family, DTO.Task task) {
    var filter = Builders<Model.TaskRoot>.Filter.And(
        Builders<Model.TaskRoot>.Filter.Eq(r => r.Root.Completed, null),
        Builders<Model.TaskRoot>.Filter.Eq(r => r.Root.Family, family));
    var update = Builders<Model.TaskRoot>.Update.Set(r => r.Root, task);

    await collection.UpdateOneAsync(filter, update);
  }
}
