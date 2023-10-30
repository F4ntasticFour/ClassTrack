using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassTrack.Pages
{

    public class UserModel
    {
        public string FirstName { get; set; }
        public string Password { get; set; }
        
    }

    public class SigninModel : PageModel
    {
        [BindProperty]
        public UserModel User { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage("SigninSuccess", "SigninSuccess", new { firstName = User.FirstName, lastName = User.Password });
        }
    }
}