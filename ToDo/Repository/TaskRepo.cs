using ToDo.Models;
using ToDo.DAL;

namespace ToDo.Repository
{
    public class TaskRepo : ITaskRepo
    {
        private readonly List<ToDoTask> _toDoTasks;

        public TaskRepo()
        {
            ConnectionDatabase conDb = new();
            _toDoTasks = conDb.GetAllTask();
        }

        /// <summary>
        /// Adds a task
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(ToDoTask task) => _toDoTasks.Add(task);

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

        /// <summary>
        /// Updates the task with the given values
        /// </summary>
        /// <param name="toDoTask"></param>
        public void EditTask(ToDoTask toDoTask)
        {
            ToDoTask? task = GetTask(toDoTask.GUID);
            task.Title = !string.IsNullOrWhiteSpace(toDoTask.Title) ? toDoTask.Title : task.Title;
            task.Description = !string.IsNullOrWhiteSpace(toDoTask.Description) ? toDoTask.Description : task.Description;
            task.TaskPriority = toDoTask.TaskPriority;
            task.IsCompleted = toDoTask.IsCompleted;
        }

        /// <summary>
        /// Removes the task by guid
        /// </summary>
        /// <param name="guid"></param>
        public void DeleteTask(Guid guid) => _toDoTasks.Remove(GetTask(guid));

        /// <summary>
        /// Removes all completed tasks
        /// </summary>
        public void DeleteCompletedTasks() => _toDoTasks.RemoveAll(task => task.IsCompleted);

        /// <summary>
        /// Complete a task by guid
        /// </summary>
        /// <param name="guid"></param>
        public void CompleteTask(Guid guid) => GetTask(guid).IsCompleted = true;
    }
}
