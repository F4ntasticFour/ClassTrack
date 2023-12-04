using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassTrack.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnPostSignIn()
    {
        return RedirectToPage("/Signin");
    }

    public IActionResult OnPostRegister()
    {
        return RedirectToPage("/Register");
    }
}
