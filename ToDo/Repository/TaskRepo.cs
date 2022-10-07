using ToDo.Models;
using ToDo.DAL;

namespace ToDo.Repository
{
    public class TaskRepo : ITaskRepo
    {
        private readonly List<ToDoTask> _toDoTasks;
        private readonly ConnectionDatabase _connectionDb;

        public TaskRepo()
        {
            _connectionDb = new();
            _toDoTasks = new List<ToDoTask>();
            _toDoTasks = _connectionDb.GetAllTask();
        }

        /// <summary>
        /// Adds a task
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(ToDoTask task)
        {
            _toDoTasks.Add(task);
            _connectionDb.CreateTask(task);
        }
        public void AddTaskToUser(string guid, int id) => _connectionDb.AddUserToTask(id, guid);
        
        /// <summary>
        /// Gets a task by a guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>The task if there is only one</returns>
        public ToDoTask? GetTask(Guid guid) => _toDoTasks.SingleOrDefault(task => task.GUID == guid);

        /// <summary>
        /// Gets the whole list of tasks
        /// </summary>
        /// <returns>Returns the list of tasks</returns>
        public List<ToDoTask> GetAllTasks() => _toDoTasks.OrderBy(task => task.Created).ToList();
        public List<ToDoTask> GetAllTaskByUserId(int id)
        {
            return _connectionDb.GetAllTaskByUserId(id);
        }
        /// <summary>
        /// Updates the task with the given values
        /// </summary>
        /// <param name="toDoTask"></param>
        public void EditTask(ToDoTask toDoTask)
        {
            ToDoTask? task = GetTask(toDoTask.GUID);
            _connectionDb.UpdateTask(toDoTask);
            task.Title = !string.IsNullOrWhiteSpace(toDoTask.Title) ? toDoTask.Title : task.Title;
            task.Description = !string.IsNullOrWhiteSpace(toDoTask.Description) ? toDoTask.Description : task.Description;
            task.TaskPriority = toDoTask.TaskPriority;
            task.IsCompleted = toDoTask.IsCompleted;
        }

        /// <summary>
        /// Removes the task by guid
        /// </summary>
        /// <param name="guid"></param>
        public void DeleteTask(Guid guid)
        {
            _toDoTasks.Remove(GetTask(guid));
            _connectionDb.DeleteTask(guid.ToString());
        }

        /// <summary>
        /// Removes all completed tasks
        /// </summary>
        public void DeleteCompletedTasks()
        {
            _toDoTasks.RemoveAll(task => task.IsCompleted);
            _connectionDb.DeleteAllCompletedTasks();
        } 

        /// <summary>
        /// Complete a task by guid
        /// </summary>
        /// <param name="guid"></param>
        public void CompleteTask(Guid guid)
        {
            GetTask(guid).IsCompleted = true;
            _connectionDb.CompletTask(guid.ToString());
        }
    }
}
