namespace DTO;

public class Task {
  public DateTime Created { get; set; }
  public DateTime? Completed { get; set; }
  public string Family { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public IEnumerable<string> Tags { get; set; } = new string[0];
}
