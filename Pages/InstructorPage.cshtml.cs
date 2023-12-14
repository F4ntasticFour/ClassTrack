using System.Data.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace ClassTrack.Pages;

public class InstructorPage : PageModel
{
    [BindProperty(SupportsGet = true)] public List<string> record_id { get; set; } = new List<string>();
    [BindProperty(SupportsGet = true)] public List<string> attendance_value { get; set; } = new List<string>();
    [BindProperty(SupportsGet = true)] public List<string> section_id { get; set; } = new List<string>();
    [BindProperty(SupportsGet = true)] public List<string> student_id { get; set; } = new List<string>();
    [BindProperty(SupportsGet = true)] public List<string> session_id { get; set; } = new List<string>();
    [BindProperty(SupportsGet = true)] public List<string> course_code { get; set; } = new List<string>();
    [BindProperty(SupportsGet = true)] public int instructor_id { get; set; }
    public string instructor_name { get; set; }

    public void OnGet()
    {
        string connectionString = "Server=localhost; Database=master; User Id=sa; Password=reallyStrongPwd123";
        SqlConnection con = new SqlConnection(connectionString);

        {
            string querystring2 = "select * from attendance_record";
            string querystring = "select name from instructor where instructor_id = @instructor_id";
            try
            {
                con.Open();
                SqlCommand read = new SqlCommand(querystring, con);
                read.Parameters.AddWithValue("@instructor_id", instructor_id);
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

            try
            {
                con.Open();
                SqlCommand read = new SqlCommand(querystring2, con);
                SqlDataReader reader = read.ExecuteReader();
                while (reader.Read())
                {
                    record_id.Add(reader[0].ToString());
                    attendance_value.Add(reader[1].ToString());
                    section_id.Add(reader[2].ToString());
                    student_id.Add(reader[3].ToString());
                    session_id.Add(reader[4].ToString());
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
            
            string querystring3 = "SELECT course_code FROM teach WHERE instructor_id = @instructor_id";
            try
            {
                con.Open();
                SqlCommand read = new SqlCommand(querystring3, con); // Use querystring3 instead of querystring
                read.Parameters.AddWithValue("@instructor_id", instructor_id);
                SqlDataReader reader = read.ExecuteReader();
                while (reader.Read())
                {
                    // Process each course code
                    course_code.Add(reader[0].ToString());
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
}