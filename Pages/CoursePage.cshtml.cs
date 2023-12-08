using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassTrack.Pages;

public class CoursePage : PageModel
{
    [BindProperty (SupportsGet = true)]
    public int CourseCode { get; set; }
    public int StudentId { get; set; }
    public void OnGet()
    {
        
    }
}