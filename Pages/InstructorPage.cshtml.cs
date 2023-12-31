using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        [BindProperty(SupportsGet = true)]
        public List<int> SectionIds { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<int> SessionsIds { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<string> CourseCodes { get; set; }

        [BindProperty(SupportsGet = true)]
        public string InstructorName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int InstructorId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SectionId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int SessionId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CourseCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? AttendanceValue { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<Student> Students { get; set; }

        public void OnGet()
        {
            string conString =
                "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";

            using (SqlConnection con = new SqlConnection(conString))
            {
                string query1 =
                    "SELECT cs.section_id, c.course_code, i.name, css.session_id AS course_title FROM course_section cs JOIN course c ON cs.course_code = c.course_code AND cs.semester = c.semester JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code AND cs.semester = t.semester inner join instructor i on i.instructor_id = t.instructor_id inner join class_session css on css.section_id = cs.section_id WHERE t.instructor_id = @InstructorId";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(query1, con);
                    read.Parameters.AddWithValue("@InstructorId", InstructorId);
                    SqlDataReader reader = read.ExecuteReader();
                    while (reader.Read())
                    {
                        SectionIds.Add((int)reader[0]);
                        CourseCodes.Add(reader[1].ToString());
                        InstructorName = reader[2].ToString();
                        SessionsIds.Add((int)reader[3]);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (AttendanceValue == null)
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    string query2 =
                        "SELECT DISTINCT s.student_id, s.name AS student_name, ar.attendance_value FROM student s JOIN attendance_record ar ON s.student_id = ar.student_id JOIN course_section cs ON ar.section_id = cs.section_id JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code WHERE t.instructor_id = @InstructorId AND cs.section_id = @SectionId AND cs.course_code = @CourseCode";
                    try
                    {
                        con.Open();
                        SqlCommand read = new SqlCommand(query2, con);
                        read.Parameters.AddWithValue("@InstructorId", InstructorId);
                        read.Parameters.AddWithValue("@SectionId", SectionId);
                        read.Parameters.AddWithValue("@CourseCode", CourseCode);
                        SqlDataReader reader = read.ExecuteReader();
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                StudentName = reader["student_name"].ToString(),
                                StudentId = (int)reader["student_id"],
                                AttendanceValue = (bool)reader["attendance_value"]
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
            else if (AttendanceValue == true)
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    string query2 =
                        "SELECT DISTINCT s.student_id, s.name AS student_name, ar.attendance_value FROM student s JOIN attendance_record ar ON s.student_id = ar.student_id JOIN course_section cs ON ar.section_id = cs.section_id JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code WHERE t.instructor_id = @InstructorId AND cs.section_id = @SectionId AND cs.course_code = @CourseCode AND ar.attendance_value = 1";
                    try
                    {
                        con.Open();
                        SqlCommand read = new SqlCommand(query2, con);
                        read.Parameters.AddWithValue("@InstructorId", InstructorId);
                        read.Parameters.AddWithValue("@SectionId", SectionId);
                        read.Parameters.AddWithValue("@CourseCode", CourseCode);
                        read.Parameters.AddWithValue("@AttendanceValue", AttendanceValue);
                        SqlDataReader reader = read.ExecuteReader();
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                StudentName = reader["student_name"].ToString(),
                                StudentId = (int)reader["student_id"],
                                AttendanceValue = (bool)reader["attendance_value"]
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
            else if (AttendanceValue == false)
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    string query2 =
                        "SELECT DISTINCT s.student_id, s.name AS student_name, ar.attendance_value FROM student s JOIN attendance_record ar ON s.student_id = ar.student_id JOIN course_section cs ON ar.section_id = cs.section_id JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code WHERE t.instructor_id = @InstructorId AND cs.section_id = @SectionId AND cs.course_code = @CourseCode AND ar.attendance_value = 0";
                    try
                    {
                        con.Open();
                        SqlCommand read = new SqlCommand(query2, con);
                        read.Parameters.AddWithValue("@InstructorId", InstructorId);
                        read.Parameters.AddWithValue("@SectionId", SectionId);
                        read.Parameters.AddWithValue("@CourseCode", CourseCode);
                        read.Parameters.AddWithValue("@AttendanceValue", AttendanceValue);
                        SqlDataReader reader = read.ExecuteReader();
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                StudentName = reader["student_name"].ToString(),
                                StudentId = (int)reader["student_id"],
                                AttendanceValue = (bool)reader["attendance_value"]
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
        }


        public IActionResult OnPost()
        {
            return RedirectToPage("/InstructorPage", new { InstructorId, SectionId, SessionId, CourseCode, AttendanceValue });
        }
    }
}
