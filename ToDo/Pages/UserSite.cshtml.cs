using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using ToDo.Models;
using ToDo.Repository;
using ToDo.SessionHelper;
using static ToDo.Models.ToDoTask;

namespace ToDo.Pages
{
    public class UserSiteModel : PageModel
    {
        readonly private IUserRepo _userRepo;
        private readonly ITaskRepo _taskRepo;
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
        public User UserLogin { get; set; }
        public UserSiteModel(IUserRepo repo, ITaskRepo trepo)
        {
            _userRepo = repo;
            _taskRepo = trepo;
        }
        public void OnGet()
        {
            try
            {
                string id = HttpContext.Session.GetString("User");
                UserLogin = _userRepo.GetUserById(Convert.ToInt32(id));
                ToDoTasks = _taskRepo.GetAllTaskByUserId(Convert.ToInt32(id));
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IActionResult OnPostAdd()
        {
            if (ModelState.IsValid)
            {
                string id = HttpContext.Session.GetString("User");
                return RedirectToPage("AddTask", new { title = Title, description = Description, priority = (int)Priority , id = Convert.ToInt32(id)});
            }
            else
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return RedirectToPage("Index", new { error = messages });
            }

        }
        public IActionResult OnPostDelete()
        {
            string id = HttpContext.Session.GetString("User");
            _userRepo.DeleteUser(Convert.ToInt32(id));
            return RedirectToPage("Index");
        }
        public IActionResult OnPostDeleteTask()
        {
            return RedirectToPage("DeleteTask", new { guid = GUID });
        }
        public IActionResult OnPostComplete()
        {
            return RedirectToPage("CompleteTask", new { guid = GUID });
        }
        public IActionResult OnPostEdit(string guid)
        {
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
    }
}
