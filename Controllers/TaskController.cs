using Interface;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase {
  private readonly ITaskRepository taskRepository;

  public TaskController(ITaskRepository taskRepository) {
    this.taskRepository = taskRepository;
  }

  [HttpGet]
  public async Task<IActionResult> GetOpenTasks() {
    var red = await taskRepository.FindOpen(Constants.RED_FAMILY) ??
              await taskRepository.FindLatest(Constants.RED_FAMILY) ??
              new DTO.Task() { Name = "-None-", Family = Constants.RED_FAMILY };
    var yellow =
        await taskRepository.FindOpen(Constants.YELLOW_FAMILY) ??
        await taskRepository.FindLatest(Constants.YELLOW_FAMILY) ??
        new DTO.Task() { Name = "-None-", Family = Constants.YELLOW_FAMILY };
    var green =
        await taskRepository.FindOpen(Constants.GREEN_FAMILY) ??
        await taskRepository.FindLatest(Constants.GREEN_FAMILY) ??
        new DTO.Task() { Name = "-None-", Family = Constants.GREEN_FAMILY };
    var today = new DTO.Today() { RedTask = red };

    return new JsonResult(new DTO.Today() { RedTask = red, YellowTask = yellow,
                                            GreenTask = green });
  }

  [HttpPost]
  [Route("{family}/")]
  public async Task<IActionResult> CreateOrUpdateTask(string family,
                                                      DTO.TaskPayload payload) {
    if (family != Constants.RED_FAMILY && family != Constants.YELLOW_FAMILY &&
        family != Constants.GREEN_FAMILY) {
      return BadRequest("Unknown family");
    }
    var entity = await taskRepository.FindOpen(family);
    if (entity != null) {
      entity.Name = payload.Name;
      entity.Tags = payload.Tags;
      await taskRepository.Update(family, entity);
    } else {
      entity = new DTO.Task() { Family = family, Created = DateTime.Now,
                                Completed = null, Name = payload.Name,
                                Tags = payload.Tags };
      await taskRepository.Create(entity);
    }
    return Ok(entity);
  }

  [HttpPut]
  [Route("{family}/")]
  public async Task<IActionResult> CompleteTask(string family) {
    if (family != Constants.RED_FAMILY && family != Constants.YELLOW_FAMILY &&
        family != Constants.GREEN_FAMILY) {
      return BadRequest("Unknown family");
    }
    var entity = await taskRepository.FindOpen(family);
    if (entity == null)
      return Conflict("Already done");
    entity.Completed = DateTime.Now;
    await taskRepository.Update(family, entity);
    return Ok(entity);
  }
}
