using ToDo.Models;

namespace ToDo.Repository
{
    public interface ITaskRepo
    {
        void AddTask(ToDoTask task);
        void CompleteTask(Guid guid);
        void DeleteCompletedTasks();
        void DeleteTask(Guid guid);
        void EditTask(ToDoTask task);
        List<ToDoTask> GetAllTasks();
        ToDoTask? GetTask(Guid guid);
    }
}
