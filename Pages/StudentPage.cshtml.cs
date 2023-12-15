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
        public int Code { get; set; }
        public string Semester { get; set; }
    }

    public class StudentPage : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Student_id { get; set; }
        public int InstructorID { get; set; }
        public int SectionID { get; set; }
        public List<Course> Courses { get; set; }

        public void OnGet()
        {
            string ConString = "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";

            using (SqlConnection con = new SqlConnection(ConString))
            {
                string querystring1 = "SELECT course.course_code, title, course.semester, instructor.instructor_id, course_section.section_id FROM course, enroll, student, teach, course_section, instructor WHERE student.student_id = @StudentId AND enroll.student_id = @StudentId AND enroll.course_code = course.course_code AND teach.course_code = course.course_code AND teach.instructor_id = instructor.instructor_id AND course_section.course_code = course.course_code";

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
                            Code = (int)reader[0],
                            Title = reader[1].ToString(),
                            Semester = reader[2].ToString()
                        };
                        Courses.Add(course);
                        InstructorID = (int)reader[3];
                        SectionID = (int)reader[4];
                    }
                }
                catch (SqlException ex)
                {
                    // Log the exception for debugging and monitoring
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        public IActionResult OnPost(string SelectedCourseCode)
        {
            return RedirectToPage("/CoursePage", new { CourseCode = SelectedCourseCode , StudentId = Student_id, InstructorId = InstructorID, SectionId = SectionID});
        }
    }
}
