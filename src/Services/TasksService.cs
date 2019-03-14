using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using SIL.Transcriber.Models;
using Newtonsoft.Json;


namespace transcriber_api.TasksService
{
    public interface ITaskService
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }

        Task<IEnumerable<TransTask>> GetAllTasks();

        //
        Task<TransTask> GetTransTaskById(string id);
        Task<IEnumerable<TransTask>> GetTransTask(string name);

        // query after multiple parameters
        //Task<IEnumerable<TransTask>> GetTransTask(string bodyText, DateTime updatedFrom, long headerSizeLimit);

        // add new task document
        Task<string> AddTransTask(TransTask item);

        // remove a single document / note
        Task<bool> RemoveTransTask(string id);

        // update just a single document / note
        Task<bool> UpdateTransTask(string id, TransTask updTask);

        // demo interface - full document update
        Task<bool> UpdateTransTaskDocument(string id, string body);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllTransTasks();
    }
    public class TaskService : ITaskService
    {
        private string connStr;
        private string dB;

        private readonly IMongoCollection<TransTask> _tasks;

        public string DatabaseName { get => dB; set => dB = value; }
        public string ConnectionString { get => connStr; set => connStr = value; }

        public TaskService(IOptions<Settings> settings)
        {
            ConnectionString = settings.Value.ConnectionString;
            DatabaseName = settings.Value.Database;

            MongoClient client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);
            _tasks = database.GetCollection<TransTask>("tasks");
        }

        public async Task<IEnumerable<TransTask>> GetAllTasks()
        {
            try
            {
                //return await _tasks
                //        .Find(_ => true).Project<TransTask>("{name:1,taskID:1}").ToListAsync();
                return await _tasks.Find(_ => true).ToListAsync();

            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after Id or InternalId (BSonId value)
        //
        public async Task<TransTask> GetTransTaskById(string id)
        {
            try
            {
                return await _tasks
                                .Find(task => task.taskID == id || task._id == GetInternalId(id))
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after body text, updated time, and header image size
        //
        public async Task<IEnumerable<TransTask>> GetTransTask(string name)
        {
            try
            {
                var query = _tasks.Find(task => task.name.Contains(name));

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public async Task<string> AddTransTask(TransTask item)
        {
            try
            {
                await _tasks.InsertOneAsync(item);
                return item._id.ToString();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveTransTask(string id)
        {
            try
            {
                DeleteResult actionResult
                    = await _tasks.DeleteOneAsync(
                        Builders<TransTask>.Filter.Eq("taskID", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateTransTask(string id, TransTask item)
        {
            try
            {
                ReplaceOneResult actionResult
                    = await _tasks
                                    .ReplaceOneAsync(n => n.taskID.Equals(id) || n._id == GetInternalId(id)
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // Demo function - full document update
        public async Task<bool> UpdateTransTaskDocument(string id, string name)
        {
            var item = await GetTransTaskById(id) ?? new TransTask();
            item.name = name;
            //item.UpdatedOn = DateTime.Now;

            return await UpdateTransTask(id, item);
        }

        public async Task<bool> RemoveAllTransTasks()
        {
            try
            {
                DeleteResult actionResult
                    = await _tasks.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }



    }
    public interface ITaskServiceJSON
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }

        Task<List<TransTask>> GetAllTasks();

        //
        Task<string> GetTransTaskById(string id);
        Task<IEnumerable<TransTask>> GetTransTask(string name);

        // query after multiple parameters
        //Task<IEnumerable<TransTask>> GetTransTask(string bodyText, DateTime updatedFrom, long headerSizeLimit);

        // add new task document
        Task<string> AddTransTask(TransTask item);

        // remove a single document / note
        Task<bool> RemoveTransTask(string id);

        // update just a single document / note
        Task<bool> UpdateTransTask(string id, TransTask updTask);

        // demo interface - full document update
        Task<bool> UpdateTransTaskDocument(string id, string body);

        // should be used with high cautious, only in relation with demo setup
        Task<bool> RemoveAllTransTasks();
    }
    public class TaskServiceJSON : ITaskServiceJSON
    {
        private string connStr;
        private string dB;

        private readonly IMongoCollection<TransTask> _tasks;

        public string DatabaseName { get => dB; set => dB = value; }
        public string ConnectionString { get => connStr; set => connStr = value; }

        public TaskServiceJSON(IOptions<Settings> settings)
        {
            ConnectionString = settings.Value.ConnectionString;
            DatabaseName = settings.Value.Database;

            MongoClient client = new MongoClient(ConnectionString);
            var database = client.GetDatabase(DatabaseName);
            _tasks = database.GetCollection<TransTask>("tasks");
        }

        public async Task<List<TransTask>> GetAllTasks()
        {
            try
            {
                //return await _tasks
                //        .Find(_ => true).Project<TransTask>("{name:1,taskID:1}").ToListAsync();
                return await _tasks.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after Id or InternalId (BSonId value)
        //
        public async Task<string> GetTransTaskById(string id)
        {
            try
            {
                var thistask = await _tasks
                                .Find(task => task.taskID == id || task._id == GetInternalId(id))
                                .FirstOrDefaultAsync();
                return JsonConvert.SerializeObject(thistask);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // query after body text, updated time, and header image size
        //
        public async Task<IEnumerable<TransTask>> GetTransTask(string name)
        {
            try
            {
                var query = _tasks.Find(task => task.name.Contains(name));

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

        public async Task<string> AddTransTask(TransTask item)
        {
            try
            {
                await _tasks.InsertOneAsync(item);
                return item._id.ToString();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveTransTask(string id)
        {
            try
            {
                DeleteResult actionResult
                    = await _tasks.DeleteOneAsync(
                        Builders<TransTask>.Filter.Eq("taskID", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateTransTask(string id, TransTask item)
        {
            try
            {
                ReplaceOneResult actionResult
                    = await _tasks
                                    .ReplaceOneAsync(n => n.taskID.Equals(id) || n._id == GetInternalId(id)
                                            , item
                                            , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        // Demo function - full document update
        public async Task<bool> UpdateTransTaskDocument(string id, string name)
        {
            string json = await GetTransTaskById(id);
            var item =  JsonConvert.DeserializeObject<TransTask>(json) ?? new TransTask();
            item.name = name;
            //item.UpdatedOn = DateTime.Now;

            return await UpdateTransTask(id, item);
        }

        public async Task<bool> RemoveAllTransTasks()
        {
            try
            {
                DeleteResult actionResult
                    = await _tasks.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }



    }
}
