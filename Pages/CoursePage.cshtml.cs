using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Data.SqlClient;

namespace ClassTrack.Pages
{
    public class Session
    {
        public int SessionID { get; set; }
        
    }
    public class CoursePage : PageModel
    {
        [BindProperty(SupportsGet = true)] 
        public string CourseCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }
        public List<Session> Sessions { get; set; } = new List<Session>(); // Initialize the list

        public void OnGet()
        {
            string ConString = "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                string querystring1 = "SELECT cs.session_id FROM class_session cs JOIN course_section csct ON csct.section_id = cs.section_id JOIN enroll e ON e.course_code = csct.course_code WHERE e.student_id = @StudentID AND e.course_code = @CourseCode";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(querystring1, con);
                    read.Parameters.AddWithValue("@CourseCode", CourseCode);
                    read.Parameters.AddWithValue("@StudentID", StudentId);
                    SqlDataReader reader = read.ExecuteReader();
                    while (reader.Read())
                    {
                        var session = new Session
                        {
                            SessionID = (int)reader[0]
                        };
                        Sessions.Add(session); // Now this will work since Sessions is initialized
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        public IActionResult OnPost(int SelectedSession)
        {
            return RedirectToPage("/AttendanceSubmission", new { Course_Code = CourseCode, Student_Id = StudentId , Session_id = SelectedSession});
        }
    }
}