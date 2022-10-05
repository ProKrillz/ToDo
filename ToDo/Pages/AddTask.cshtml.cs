using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ToDo.Models;
using ToDo.Repository;
using static ToDo.Models.ToDoTask;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ToDo.Pages
{
    public class AddTaskModel : PageModel
    {
        private readonly ITaskRepo _repo;
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Priority is required"), Range(0, 2), DefaultValue(1)]
        public ToDoTask.Priority Priority { get; set; }
        public AddTaskModel(ITaskRepo repo)
        {
            _repo = repo;
        }

        public IActionResult OnGet(string title, string description, int priority)
        {
            try
            {
                Title = title;
                Description = description;
                Priority = (ToDoTask.Priority)priority;
                if (ModelState.IsValid)
                {
                    _repo.AddTask(new(Title, Description, Priority));
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
