namespace back.Services
{
    public interface ITaskDbService
    {
        public Task<List<Models.Transfer.TaskEntry>> GetTasks();
        public Task<Models.Transfer.TaskEntry> GetTask(int id);
        public Task<Models.Transfer.TaskEntry> AddTask(Models.Transfer.TaskData task);

        public Task<Models.Transfer.TaskEntry> UpdateTask(int id, Models.Transfer.TaskData task);
        public Task<Models.Transfer.TaskData> DeleteTask(int id);
    }
}