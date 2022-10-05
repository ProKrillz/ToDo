using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using ToDo.Repository;

namespace ToDo.Pages
{
    public class DeleteTaskModel : PageModel
    {
        private readonly ITaskRepo _repo;
        [Required(ErrorMessage = "Guid is required")]
        public Guid GUID { get; set; }
        public DeleteTaskModel(ITaskRepo repo)
        {
            _repo = repo;
        }

        public IActionResult OnGet(Guid guid)
        {
            try
            {
                GUID = guid;
                if (ModelState.IsValid)
                {
                    _repo.DeleteTask(GUID);
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
