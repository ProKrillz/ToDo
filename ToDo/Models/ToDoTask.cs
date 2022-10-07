using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ToDo.Models
{
    public record ToDoTask
    {
        public Guid GUID { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }
        public DateTime Created { get; init; }

        public Priority TaskPriority { get; set; }

        public bool IsCompleted { get; set; }

        public enum Priority { Low = 1, Normal = 2, High = 3 }

        public ToDoTask() { }

        public ToDoTask( string title, string description, Priority priority)
        {
            GUID = Guid.NewGuid();
            Title = title;
            Description = description;
            TaskPriority = priority;
            IsCompleted = false;
        }
    }
}
