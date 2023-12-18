using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace ClassTrack.Pages
{

     public class AttendanceSubmission : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [Required(ErrorMessage = "Code is required.")]
        public int Code { get; set; }

        [BindProperty(SupportsGet = true)] public string Course_Code { get; set; }
        [BindProperty(SupportsGet = true)] public int Student_Id { get; set; }
        [BindProperty(SupportsGet = true)] public int Session_id { get; set; }
        [BindProperty(SupportsGet = true)] public int session_code { get; set; }
        [BindProperty(SupportsGet = true)] public int section_id { get; set; }
        [BindProperty(SupportsGet = true)] public int record_id { get; set; }

         public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            string ConString = "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                string query =
                    "SELECT cs.attendence_code, cs.section_id, (SELECT TOP 1 ar.record_id + 1 FROM attendance_record ar ORDER BY ar.record_id DESC) FROM class_session cs JOIN course_section csct ON csct.section_id = cs.section_id JOIN enroll e ON e.course_code = csct.course_code WHERE e.student_id = @StudentID AND e.course_code = @CourseCode AND cs.session_id = @SessionID GROUP BY cs.attendence_code, cs.section_id;";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(query, con);
                    read.Parameters.AddWithValue("@CourseCode", Course_Code);
                    read.Parameters.AddWithValue("@StudentID", Student_Id);
                    read.Parameters.AddWithValue("@SessionID", Session_id);
                    using (SqlDataReader reader = read.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            session_code = (int)reader[0];
                            section_id = (int)reader[1];
                            record_id = (int)reader[2];
                        }
                        else
                        {
                            // Handle the case where no rows are returned, set a default value, or handle accordingly.
                            // For example, you could set record_id to a default value like 1.
                            record_id = 1;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Handle the exception, log or display an error message to the user
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            using (SqlConnection con = new SqlConnection(ConString))
            {
                if (Code == session_code)
                {
                    try
                    {
                        con.Open();
                        string query2 = "INSERT INTO attendance_record (record_id, student_id, section_id, session_id, attendance_value) VALUES (@RecordID, @StudentID, @SectionID, @SessionID, 1);";
                        SqlCommand cmd = new SqlCommand(query2, con);
                        cmd.Parameters.AddWithValue("@RecordID", record_id);
                        cmd.Parameters.AddWithValue("@StudentID", Student_Id);
                        cmd.Parameters.AddWithValue("@SectionID", section_id);
                        cmd.Parameters.AddWithValue("@SessionID", Session_id);
                        cmd.ExecuteNonQuery();
                        return RedirectToPage("/StudentPage", new {Student_Id = Student_Id});
                    }
                    catch (SqlException ex)
                    {
                        // Handle the exception, log or display an error message to the user
                        Console.WriteLine($"Error: {ex.Message}");
                        return Page();

                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(Code), "Incorrect attendance code. Please enter the correct code.");
                    return Page();

                }

            }
        }

    }
}
