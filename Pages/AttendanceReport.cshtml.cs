using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassTrack.Pages
{
    public class AttendanceReport : PageModel
    {
        [BindProperty(SupportsGet = true)] 
        public List<string> Student_Name { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public List<int> Student_Id {get; set;}
        
        [BindProperty(SupportsGet = true)]
        public List<DateTime> Time_of_Submission {get; set;}
        
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/AttendanceSubmission",new {
                Student_Name_Input = Student_Name,
                Student_Id_input = Student_Id,
                Time_of_Submission_Input = Time_of_Submission });

        }
    }
}