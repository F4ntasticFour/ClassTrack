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
        [BindProperty (SupportsGet = true)] public int sectionid { get; set; }
        [BindProperty(SupportsGet = true)] public int record_id { get; set; }
        [BindProperty(SupportsGet = true)] public int Sessionid { get; set; }

         public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            string ConString = "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
            using (SqlConnection con = new SqlConnection(ConString))
            {
                string query = "SELECT cs.section_id FROM course_section cs JOIN teach t ON cs.course_code = t.course_code AND cs.semester = t.semester JOIN enroll e ON e.course_code = cs.course_code AND e.semester = cs.semester WHERE e.student_id = @student_id  AND cs.course_code = @course_code;";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(query, con);
                    read.Parameters.AddWithValue("@course_code", Course_Code);
                    read.Parameters.AddWithValue("@student_id", Student_Id);
                    SqlDataReader reader = read.ExecuteReader();
                    if (reader.Read())
                    {
                        sectionid = (int)reader[0];
                        Console.WriteLine("Section ID: " + sectionid);
                    }
                    else
                    {
                        // Handle the case where no rows are returned, set a default value, or handle accordingly.
                        // For example, you could set Session_id to a default value like 1.
                        sectionid = 1;
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
                string query = "SELECT session_id FROM class_session WHERE week = @SessionID AND section_id = @SectionID";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(query, con);
                    read.Parameters.AddWithValue("@SessionID", Session_id);
                    read.Parameters.AddWithValue("@SectionID", sectionid);
                    Console.WriteLine("week: " + Session_id);
                    Console.WriteLine("Section: "+ sectionid);
                    
                    SqlDataReader reader = read.ExecuteReader();
                    if (reader.Read())
                    {
                        Sessionid = (int)reader[0];
                        Console.WriteLine("Session ID: " + Sessionid);
                    }
                    else
                    {
                        // Handle the case where no rows are returned, set a default value, or handle accordingly.
                        // For example, you could set Session_id to a default value like 1.
                        Sessionid = 1;
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
                string query =
                    "SELECT cs.attendence_code, cs.section_id, (SELECT TOP 1 ar.record_id + 1 FROM attendance_record ar ORDER BY ar.record_id DESC) FROM class_session cs JOIN course_section csct ON csct.section_id = cs.section_id JOIN enroll e ON e.course_code = csct.course_code WHERE e.student_id = @StudentID AND e.course_code = @CourseCode AND cs.session_id = @SessionID GROUP BY cs.attendence_code, cs.section_id;";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(query, con);
                    read.Parameters.AddWithValue("@CourseCode", Course_Code);
                    read.Parameters.AddWithValue("@StudentID", Student_Id);
                    read.Parameters.AddWithValue("@SessionID", Sessionid);
                    Console.WriteLine("Session ID: " + Sessionid);
                    Console.WriteLine("Student ID: " + Student_Id);
                    Console.WriteLine("Course Code: " + Course_Code);
                    using (SqlDataReader reader = read.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            session_code = (int)reader[0];
                            section_id = (int)reader[1];
                            record_id = (int)reader[2];
                            Console.WriteLine("Session Code: " + session_code);
                            Console.WriteLine("Section ID: " + section_id);
                            Console.WriteLine("Record ID: " + record_id);
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
                        cmd.Parameters.AddWithValue("@SessionID", Sessionid);
                        Console.WriteLine("Record ID: " + record_id);
                        Console.WriteLine("Student ID: " + Student_Id);
                        Console.WriteLine("Section ID: " + section_id);
                        Console.WriteLine("Session ID: " + Sessionid);
                        cmd.ExecuteNonQuery();
                        return RedirectToPage("/StudentPage", new {Student_Id});
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
