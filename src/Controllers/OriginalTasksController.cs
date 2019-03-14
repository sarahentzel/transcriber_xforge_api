using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using transcriber_api.TasksService;
using transcriber_api.Models;
using Newtonsoft.Json;

namespace transcriber_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<TransTask>> Get()
        {
            return await _taskService.GetAllTasks();
        }

        [HttpGet("{id}")]
        public async Task<TransTask> GetTask(string id)
        {
            return await _taskService.GetTransTaskById(id) ?? new TransTask();
        }


        [HttpPost]
        public async Task<TransTask> Create(NewTask newTask)
        {
            TransTask addTask = new TransTask
            {
                name = newTask.name,
                book = newTask.book,
                description = newTask.description,
                passageSet = newTask.passageSet,
                taskID = newTask.taskID
            };
            var id = await _taskService.AddTransTask(addTask);

            return await _taskService.GetTransTaskById(id);
        }
        /*
                [HttpPut("{id:length(24)}")]
                public async Task<string> Update(string id, NewTask taskIn)
                {
                    TransTask task = await _taskService.GetTransTaskById(id);

                    if (task == null)
                    {
                        return NotFound();
                    }

                    _taskService.Update(id, taskIn);

                    return NoContent();
                }

                [HttpDelete("{id:length(24)}")]
                public IActionResult Delete(string id)
                {
                    var task = _taskService.Get(id);

                    if (task == null)
                    {
                        return NotFound();
                    }

                    _taskService.Remove(task.Id);

                    return NoContent();
                }
            }
             */
    }
}
