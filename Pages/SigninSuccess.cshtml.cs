using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassTrack.Pages;

public class User
{
    [BindProperty]
    public string UserName { get; set; }
    
    public string ID { get; set; }
}

public class SigninSuccess : PageModel
{

    public User User { get; set; }

    public IActionResult OnGet(string username, string userID)
    {
        User = new User
        {
            UserName = username,
            ID = userID
        };
        return Page();
    }
}