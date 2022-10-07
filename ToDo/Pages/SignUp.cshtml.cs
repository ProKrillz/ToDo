using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security;
using ToDo.Models;
using ToDo.Repository;

namespace ToDo.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly IUserRepo _userRepo;
        [BindProperty, Required]
        public string Firstname { get; set; }
        [BindProperty, Required]
        public string Lastname { get; set; }
        [BindProperty, Required]
        public string Username { get; set; }
        [BindProperty, Required]
        public string Password { get; set; }
        public SignUpModel(IUserRepo repo)
        {
            _userRepo = repo;
        }
        public void OnGet()
        {
       
        }
        public IActionResult OnPostSignUp()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userRepo.AddUser(new User(0, Firstname, Lastname, Username, Password));
                    return RedirectToPage("Index");
                }
                else
                    return Page();
            }
            catch (Exception)
            {
                return RedirectToPage("Error");
                throw;
            }
        }
    }
}
