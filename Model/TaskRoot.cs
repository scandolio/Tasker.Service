using MongoDB.Bson;

namespace Model;

public class TaskRoot {
  public ObjectId _id { get; set; }
  public DTO.Task Root { get; set; } = new DTO.Task();
}
