using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security;
using ToDo.Models;
using ToDo.Repository;
using ToDo.SessionHelper;

namespace ToDo.Pages
{
    public class LoginSiteModel : PageModel
    {
        readonly private IUserRepo _userRepo;

        [BindProperty, Required]
        public string Username { get; set; }
        [BindProperty, Required]
        public string Password { get; set; }
        public LoginSiteModel(IUserRepo repo)
        {
            _userRepo = repo; //Remeber todo depensyinjection
        }
        public void OnGet()
        {
   
        }
        public IActionResult OnPostLoginUser() 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = _userRepo.LoginUser(Username, Password);
                    if (user != null)
                    {
                        HttpContext.Session.SetSessionString(user.Id.ToString(), "User");
                        return RedirectToPage("UserSite");
                    }
                }
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
