using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using ToDo.Models;
using ToDo.Repository;

namespace ToDo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITaskRepo _repo;
        public List<ToDoTask> ToDoTasks { get; set; }

        [BindProperty, MaxLength(25, ErrorMessage = "Title can't contain more than 25 characters")]
        public string? Title { get; set; }

        [BindProperty, MaxLength(25, ErrorMessage = "Description can't contain more than 25 characters")]
        public string? Description { get; set; }

        [BindProperty]
        public Guid GUID { get; set; }


        [BindProperty]
        public ToDoTask.Priority Priority { get; set; }

        [BindProperty]
        public bool IsCompleted { get; set; }

        public string? Error { get; set; }

        public IndexModel(ITaskRepo repo)
        {
            _repo = repo;
            ToDoTasks = repo.GetAllTasks();
        }

        public void OnGet(string? error)
        {
            Error = error;
        }

        public IActionResult OnPostAdd()
        {
            if (ModelState.IsValid)
            {
                return RedirectToPage("AddTask", new { title = Title, description = Description, priority = (int)Priority });
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return RedirectToPage("Index", new { error = messages });
            }

        }

        public IActionResult OnPostEdit(string guid)
        {
            //if (!(string.IsNullOrWhiteSpace(Title) | string.IsNullOrWhiteSpace(Description)) && (Title?.Length <= 25 && Description?.Length <= 25))
            //{
            //    _repo.EditTask(GUID, Title, Description, Priority, IsCompleted);
            //}
            //else if (string.IsNullOrWhiteSpace(Title) && string.IsNullOrWhiteSpace(Description))
            //{
            //    _repo.EditTask(GUID, Title, Description, Priority, IsCompleted);

            //}
            //else if (!string.IsNullOrWhiteSpace(Title) && Title.Length <= 25)
            //{
            //    _repo.EditTask(GUID, Title, Description, Priority, IsCompleted);
            //}
            //else if (!string.IsNullOrWhiteSpace(Description) && Description.Length <= 25)
            //{
            //    _repo.EditTask(GUID, Title, Description, Priority, IsCompleted);
            //}
            if (ModelState.IsValid)
            {
                return RedirectToPage("UpdateTask", new { guid, title = Title, description = Description, priority = (int)Priority, isCompleted = IsCompleted });
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return RedirectToPage("Index", new { error = messages });
            }
        }


        public IActionResult OnPostComplete()
        {
            return RedirectToPage("CompleteTask", new { guid = GUID });
        }

        public IActionResult OnPostDelete()
        {
            return RedirectToPage("DeleteTask", new { guid = GUID });
        }

        public IActionResult OnPostDeleteCompletedTasks()
        {
            _repo.DeleteCompletedTasks();
            return RedirectToPage();
        }
    }
}