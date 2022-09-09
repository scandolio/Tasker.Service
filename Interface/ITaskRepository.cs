namespace Interface;

public interface ITaskRepository {
  Task Create(DTO.Task task);
	Task<DTO.Task?> FindOpen(string family);
	Task<DTO.Task?> FindLatest(string family);
	Task Update(string family, DTO.Task task);
}
