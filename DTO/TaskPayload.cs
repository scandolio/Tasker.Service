namespace DTO;

public class TaskPayload {
  public string Name { get; set; } = string.Empty;
  public IEnumerable<string> Tags { get; set; } = new string[0];
}
