using System.Data.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Dynamic;
using System.Net.Sockets;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.Pages
{

    public class Student
    {
        public string StudentName { get; set; }
        public int StudentId { get; set; }
        public bool AttendanceValue { get; set; }
    }
    public class InstructorPage : PageModel
    {
        [BindProperty(SupportsGet = true)] public List<int> sectionIds { get; set; }
        [BindProperty(SupportsGet = true)] public List<string> coursecodes { get; set; }

        [BindProperty(SupportsGet = true)] public List<int> sessionIds { get; set; }

        [BindProperty(SupportsGet = true)] public string coursecode { get; set; }
        [BindProperty(SupportsGet = true)] public int sectionId { get; set; }
        [BindProperty(SupportsGet = true)] public int instructorId { get; set; }

        [BindProperty(SupportsGet = true)] public int sessionId { get; set; }

        [BindProperty(SupportsGet = true)] public bool AttendanceValue { get; set; }
        
        [BindProperty(SupportsGet = true)] public List<Student> Students { get; set; }
        
        [BindProperty(SupportsGet = true)] public string InstructorName { get; set; }

        public void OnGet()
        {
            string ConString =
                "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";

            using (SqlConnection con = new SqlConnection(ConString))
            {
                string query1 ="SELECT cs.section_id, c.course_code, i.name AS course_title FROM course_section cs JOIN course c ON cs.course_code = c.course_code AND cs.semester = c.semester JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code AND cs.semester = t.semester inner join instructor i on i.instructor_id = t.instructor_id WHERE t.instructor_id = @InstructorId";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(query1, con);
                    read.Parameters.AddWithValue("@InstructorId", instructorId);
                    SqlDataReader reader = read.ExecuteReader();
                    while (reader.Read())
                    {
                        sectionIds.Add((int)reader[0]);
                        coursecodes.Add(reader[1].ToString());
                        InstructorName = reader[2].ToString();
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        public IActionResult OnPost()
        {
            string ConString =
                "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
            if (AttendanceValue == null)
            {
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    string query2 = "SELECT s.student_id, s.name AS student_name, ar.attendance_value FROM student s JOIN attendance_record ar ON s.student_id = ar.student_id JOIN course_section cs ON ar.section_id = cs.section_id JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code WHERE t.instructor_id = @instructorid AND cs.section_id = @sectionid AND cs.course_code = @coursecode";
                    try
                    {
                        con.Open();
                        SqlCommand read = new SqlCommand(query2, con); 
                        read.Parameters.AddWithValue("@instructorid", instructorId);
                        read.Parameters.AddWithValue("@sectionid", sectionId);
                        read.Parameters.AddWithValue("@coursecode", coursecode);
                        SqlDataReader reader = read.ExecuteReader();
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                StudentName = reader[0].ToString(),
                                StudentId = (int)reader[1],
                                AttendanceValue = (bool)reader[2]
                            };
                            Students.Add(student);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    string query3 = "SELECT s.student_id, s.name AS student_name, ar.attendance_value FROM student s JOIN attendance_record ar ON s.student_id = ar.student_id JOIN course_section cs ON ar.section_id = cs.section_id JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code WHERE t.instructor_id = @instructorid AND cs.section_id = @sectionid AND cs.course_code = @coursecode AND ar.attendance_value = @attendancevalue";
                    try
                    {
                        con.Open();
                        SqlCommand read = new SqlCommand(query3, con); 
                        read.Parameters.AddWithValue("@instructorid", instructorId);
                        read.Parameters.AddWithValue("@sectionid", sectionId);
                        read.Parameters.AddWithValue("@coursecode", coursecode);
                        read.Parameters.AddWithValue("@attendancevalue", AttendanceValue);
                        SqlDataReader reader = read.ExecuteReader();
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                StudentName = reader[0].ToString(),
                                StudentId = (int)reader[1],
                                AttendanceValue = (bool)reader[2]
                            };
                            Students.Add(student);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return Page();
        }
    }
}