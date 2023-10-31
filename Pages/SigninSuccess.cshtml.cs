using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassTrack.Pages;

public class User
{
    public string FirstName { get; set; }
    public string Password { get; set; }
}

public class SigninSuccess : PageModel
{

    public User User { get; set; }

    public IActionResult OnGet(string firstName, string password)
    {
        User = new User
        {
            FirstName = firstName,
            Password = password
        };
        return Page();
    }
}