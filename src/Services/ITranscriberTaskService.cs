using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonApiDotNetCore.Services;
using SIL.XForge.Models;
using SIL.Transcriber.Models;

namespace SIL.Transcriber.Services
{
    public interface ITranscriberTaskService: IResourceService<TranscriberTaskResource, string>
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }

        Task<IEnumerable<TranscriberTaskEntity>> GetAllTasks();

        //
        Task<TranscriberTaskEntity> GetTaskById(string id);
        Task<IEnumerable<TranscriberTaskEntity>> GetTask(string name);

        // query after multiple parameters
        //Task<IEnumerable<TransTask>> GetTransTask(string bodyText, DateTime updatedFrom, long headerSizeLimit);

        // add new task document
        Task<string> AddTask(TranscriberTaskEntity item);

        // remove a single document / note
        Task<bool> RemoveTask(string id);

        // update just a single document / note
        Task<bool> UpdateTransTask(string id, TranscriberTaskEntity updTask);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllTasks();
    }

}
