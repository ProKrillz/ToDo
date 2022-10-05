using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ToDo.Models;
using ToDo.Repository;
using static ToDo.Models.ToDoTask;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace ToDo.Pages
{
    public class UpdateTaskModel : PageModel
    {
        private readonly ITaskRepo _repo;

        [MaxLength(25, ErrorMessage = "Title cannot contain more than 25 characters"), AllowNull]
        public string? Title { get; set; }
        [MaxLength(25, ErrorMessage = "Description cannot contain more than 25 characters"), AllowNull]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Guid is required")]
        public Guid GUID { get; set; }

        [Required(ErrorMessage = "Priority is required"), Range(0, 2, ErrorMessage = "Priority was out of scope"), DefaultValue(1)]
        public ToDoTask.Priority Priority { get; set; }

        [Required(ErrorMessage = "Field Completed is required")]
        public bool IsCompleted { get; set; }

        public UpdateTaskModel(ITaskRepo repo)
        {
            _repo = repo;
        }

        public IActionResult OnGet(Guid guid, string? title, string? description, int priority, bool isCompleted)
        {
            try
            {
                GUID = guid;
                Title = title;
                Description = description;
                Priority = (ToDoTask.Priority)priority;
                IsCompleted = isCompleted;
                if (ModelState.IsValid)
                {
                    _repo.EditTask(new()
                    {
                        GUID = GUID,
                        Title = Title,
                        Description = Description,
                        TaskPriority = Priority,
                        IsCompleted = IsCompleted
                    });
                    return RedirectToPage("Index");
                }
                else
                {
                    string messages = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                    return RedirectToPage("Index", new { error = messages });
                }
            }
            catch (Exception)
            {
                return RedirectToPage("Error");
            }
        }

    }
}
