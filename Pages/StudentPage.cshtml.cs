using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ClassTrack.Pages
{
    public class Course

    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Semester { get; set; }
        
        public string Instructor { get; set; }
        
        public int Section { get; set; }
        public int InstructorID { get; set; }

        
    }

    public class StudentPage : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Student_id { get; set; }
        public List<Course> Courses { get; set; }

        public void OnGet()
        {
            string ConString = "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";

            using (SqlConnection con = new SqlConnection(ConString))
            {
                string querystring1 = "SELECT c.course_code, c.title, c.semester, i.name,i.instructor_id, cs.section_id  FROM  student s INNER JOIN enroll e ON s.student_id = e.student_id INNER JOIN course c ON e.course_code = c.course_code INNER JOIN teach t ON c.course_code = t.course_code INNER JOIN instructor i ON t.instructor_id = i.instructor_id INNER JOIN course_section cs ON c.course_code = cs.course_code WHERE s.student_id = @StudentId";

                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(querystring1, con);
                    read.Parameters.AddWithValue("@StudentId", Student_id);
                    SqlDataReader reader = read.ExecuteReader();
                    Courses = new List<Course>();
                    while (reader.Read())
                    {
                        var course = new Course
                        {
                            Code = reader[0].ToString(),
                            Title = reader[1].ToString(),
                            Instructor = reader[3].ToString(),
                            Semester = reader[2].ToString(),
                            InstructorID = (int)reader[4],
                            Section = (int)reader[5]
                        };
                        Courses.Add(course);
                    }
                }
                catch (SqlException ex)
                {
                    // Log the exception for debugging and monitoring
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        public IActionResult OnPost(string SelectedCourse)
        {
            return RedirectToPage("/CoursePage", new { CourseCode = SelectedCourse, StudentId = Student_id });
        }

    }
}
