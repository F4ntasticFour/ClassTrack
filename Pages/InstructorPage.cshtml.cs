using System.Data.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.Pages;

public class InstructorPage : PageModel
{
    [BindProperty(SupportsGet = true)] public int RecordId { get; set; } = -1;
    [BindProperty(SupportsGet = true)] public List<string?> AttendanceValue { get; set; } = new List<string?>();
    [BindProperty(SupportsGet = true)] public List<string?> SectionId { get; set; } = new List<string?>();
    [BindProperty(SupportsGet = true)] public List<string?> StudentId { get; set; } = new List<string?>();
    [BindProperty(SupportsGet = true)] public List<string?> StudentName { get; set; } = new List<string?>();
    [BindProperty(SupportsGet = true)] public List<string?> SessionId { get; set; } = new List<string?>();
    [BindProperty(SupportsGet = true)] public List<string> CourseCode { get; set; } = new List<string>();
    
    [BindProperty(SupportsGet = true)] public string CourseCodeInput { get; set; }
    
    [BindProperty(SupportsGet = true)] public string SectionIdInput { get; set; }
    
    [BindProperty(SupportsGet = true)] public int SessionIdInput { get; set; }
    
    [BindProperty(SupportsGet = true)] public bool StudentAttendanceValue { get; set; }
    [BindProperty(SupportsGet = true)] public int InstructorId { get; set; }
    public string? instructor_name { get; set; }

    public void OnGet()
    {
        string connectionString = "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
        SqlConnection con = new SqlConnection(connectionString);

        {
            string querystring = "SELECT name FROM instructor WHERE instructor_id = @instructor_id";
            try
            {
                con.Open();
                SqlCommand read = new SqlCommand(querystring, con);
                read.Parameters.AddWithValue("@instructor_id", InstructorId);
                SqlDataReader reader = read.ExecuteReader();
                while (reader.Read())
                {
                    instructor_name = reader[0].ToString();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            string querystring2 = "SELECT * FROM attendance_record " +
                                  "INNER JOIN student ON attendance_record.student_id = student.student_id";
            try
            {
                con.Open();
                SqlCommand read = new SqlCommand(querystring2, con);
                SqlDataReader reader = read.ExecuteReader();
                while (reader.Read())
                {
                    RecordId = Convert.ToInt32(reader[0].ToString());
                    AttendanceValue.Add(reader[1].ToString());
                    SectionId.Add(reader[2].ToString());
                    StudentId.Add(reader[3].ToString());
                    SessionId.Add(reader[4].ToString());
                    StudentName.Add(reader[6].ToString());
                    
                    Console.WriteLine(
                        $"Added user: {reader[0].ToString()}, {reader[1].ToString()}, {reader[2].ToString()}, {reader[3].ToString()}, {reader[4].ToString()}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }

            string querystring3 = "SELECT DISTINCT teach.course_code,course_section.section_id FROM teach " +
                                  "INNER JOIN course_section ON teach.course_code = course_section.course_code " +
                                  "WHERE teach.instructor_id = @instructor_id";
            try
            {
                con.Open();
                SqlCommand read = new SqlCommand(querystring3, con); // Use querystring3 instead of querystring
                read.Parameters.AddWithValue("@instructor_id", InstructorId);
                SqlDataReader reader = read.ExecuteReader();
                while (reader.Read())
                {
                    // Process each course code
                    CourseCode.Add(reader[0].ToString());
                    CourseCodeInput = CourseCode[0];
                    SectionIdInput = reader[1].ToString();
                    SectionIdInput = SectionId[0];
                    
                    Console.WriteLine($"Added course code: {reader[0].ToString()}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
    }
    public IActionResult OnPostSelection()
    {
        string connectionString = "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
        SqlConnection con = new SqlConnection(connectionString);
        
        string SelectionQuery = "SELECT * FROM attendance_record " +
                                "INNER JOIN student ON attendance_record.student_id = student.student_id " +
                                "INNER JOIN course_section ON attendance_record.section_id = course_section.section_id " +
                                "WHERE course_section.course_code = @CourseCodeInput AND attendance_record.section_id = @SectionIdInput";
        
        try
        {
            con.Open();
            SqlCommand read = new SqlCommand(SelectionQuery, con);
            
            read.Parameters.AddWithValue("@CourseCodeInput", CourseCodeInput);
            read.Parameters.AddWithValue("@SectionIdInput", SectionIdInput);
            read.Parameters.AddWithValue("@SessionIdInput", SessionIdInput);
            read.Parameters.AddWithValue("@StudentAttendanceValue", StudentAttendanceValue);
            
            SqlDataReader reader = read.ExecuteReader();
            while (reader.Read())
            {
                RecordId = Convert.ToInt32(reader[0].ToString());
                AttendanceValue.Add(reader[1].ToString());
                SectionId.Add(reader[2].ToString());
                StudentId.Add(reader[3].ToString());
                SessionId.Add(reader[4].ToString());
                StudentName.Add(reader[6].ToString());
                    
                
                Console.WriteLine(
                    $"Added user: {reader[0].ToString()}, {reader[1].ToString()}, {reader[2].ToString()}, {reader[3].ToString()}, {reader[4].ToString()}");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            con.Close();
        }
        
        return RedirectToPage("/InstructorPage", new { CourseCode, CourseCodeInput, InstructorId, SectionId, SectionIdInput});
    }

    public IActionResult OnPostFilter()
    {
        string connectionString = "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
        SqlConnection con = new SqlConnection(connectionString);
        
        string filterQuery = "SELECT * FROM attendance_record " +
                             "INNER JOIN student ON attendance_record.student_id = student.student_id " +
                             "INNER JOIN course_section ON attendance_record.section_id = course_section.section_id " +
                             "WHERE course_section.course_code = @CourseCodeInput AND attendance_record.section_id = @SectionIdInput AND attendance_record.session_id = @SessionIdInput AND attendance_record.attendance_value = @StudentAttendanceValue";
        
        try
        {
            con.Open();
            SqlCommand read = new SqlCommand(filterQuery, con);
            
            read.Parameters.AddWithValue("@CourseCodeInput", CourseCodeInput);
            read.Parameters.AddWithValue("@SectionIdInput", SectionIdInput);
            read.Parameters.AddWithValue("@SessionIdInput", SessionIdInput);
            read.Parameters.AddWithValue("@StudentAttendanceValue", StudentAttendanceValue);
            
            SqlDataReader reader = read.ExecuteReader();
            while (reader.Read())
            {
                RecordId = Convert.ToInt32(reader[0].ToString());
                AttendanceValue.Add(reader[1].ToString());
                SectionId.Add(reader[2].ToString());
                StudentId.Add(reader[3].ToString());
                SessionId.Add(reader[4].ToString());
                StudentName.Add(reader[6].ToString());
                    
                
                Console.WriteLine(
                    $"Added user: {reader[0].ToString()}, {reader[1].ToString()}, {reader[2].ToString()}, {reader[3].ToString()}, {reader[4].ToString()}");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            con.Close();
        }
        return RedirectToPage("/InstructorPage", new { CourseCode, SectionId, InstructorId, CourseCodeInput, SectionIdInput, SessionIdInput });
    }

}