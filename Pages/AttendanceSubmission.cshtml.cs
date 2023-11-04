using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassTrack.Pages;

public class AttendanceSubmission : PageModel
{
    [BindProperty(SupportsGet = true)] 
    public List<string> Student_Name_Input {  get; set; }
    [BindProperty(SupportsGet = true)]
    public List<string> Student_Id_Input {get; set;}
    [BindProperty(SupportsGet = true)]
    public List<String> Time_of_Submission_Input {get; set;}
    
    [BindProperty] 
    public string  Name { get; set; }
    [BindProperty]
    public string Id {get; set;}
    
    public static DateTime currentDateTime = DateTime.Now;

    [BindProperty]
    public string Time {get; set;} = currentDateTime.ToString("dddd, dd MMMM yyyy HH:mm:ss");
    

    public void OnGet()
    {
        if (Student_Id_Input == null)
            Student_Id_Input = new List<string>();

        if (Student_Name_Input == null)
            Student_Name_Input = new List<string>();

        if (Time_of_Submission_Input == null)
            Time_of_Submission_Input = new List<string>();

        Student_Id_Input.Add(Id);
        Student_Name_Input.Add(Name);
        Time_of_Submission_Input.Add(Time);
    }


    public IActionResult OnPost()
    {
        Student_Id_Input.Add(Id);
        Student_Name_Input.Add(Name);
        Time_of_Submission_Input.Add(Time);
        return RedirectToPage("/AttendanceReport",
            new
            {
                Student_Name = Student_Name_Input,
                Student_Id = Student_Id_Input,
                Time_of_Submission = Time_of_Submission_Input
            });
    }
}