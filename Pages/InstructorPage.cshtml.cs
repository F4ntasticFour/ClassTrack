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
        public List<int> Weeks { get; set; }

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
        [BindProperty(SupportsGet = true)]
        public string Room { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Week { get; set; }
        
        [TempData]
        public string LectureCode { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int Count { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Section_Id { get; set; }
        [BindProperty(SupportsGet = true)]
        public int week { get; set; }
        
        Random rnd = new Random();

        public void OnGet()
        {
            string conString =
                "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
            using (SqlConnection con = new SqlConnection(conString))
            {
                string query1 =
                    "SELECT cs.section_id, c.course_code, i.name, css.week FROM course_section cs JOIN course c ON cs.course_code = c.course_code AND cs.semester = c.semester JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code AND cs.semester = t.semester inner join instructor i on i.instructor_id = t.instructor_id inner join class_session css on css.section_id = cs.section_id WHERE t.instructor_id = @InstructorId";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(query1, con);
                    read.Parameters.AddWithValue("@InstructorId", InstructorId);
                    SqlDataReader reader = read.ExecuteReader();
                    while (reader.Read())
                    {
                        int sectionId = (int)reader[0];
                        string courseCode = reader[1].ToString();
                        string instructorName = reader[2].ToString();
                        int sessionId = (int)reader[3];

                        if (!SectionIds.Contains(sectionId))
                            SectionIds.Add(sectionId);

                        if (!CourseCodes.Contains(courseCode))
                            CourseCodes.Add(courseCode);

                        if (string.IsNullOrEmpty(InstructorName))
                            InstructorName = instructorName; // Set once, assuming it remains the same for all rows

                        if (!Weeks.Contains(sessionId))
                            Weeks.Add(sessionId);
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
                        "SELECT DISTINCT s.student_id, s.name AS student_name, ar.attendance_value FROM student s JOIN attendance_record ar ON s.student_id = ar.student_id JOIN course_section cs ON ar.section_id = cs.section_id JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code JOIN class_session css on css.section_id = cs.section_id WHERE t.instructor_id = @InstructorId AND cs.section_id = @SectionId AND cs.course_code = @CourseCode AND ar.session_id = @sessionid";
                    try
                    {
                        con.Open();
                        SqlCommand read = new SqlCommand(query2, con);
                        read.Parameters.AddWithValue("@InstructorId", InstructorId);
                        read.Parameters.AddWithValue("@SectionId", SectionId);
                        read.Parameters.AddWithValue("@CourseCode", CourseCode); 
                        read.Parameters.AddWithValue("@sessionid", SessionId);
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
                        "SELECT DISTINCT s.student_id, s.name AS student_name, ar.attendance_value FROM student s JOIN attendance_record ar ON s.student_id = ar.student_id JOIN course_section cs ON ar.section_id = cs.section_id JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code JOIN class_session css on css.section_id = cs.section_id WHERE t.instructor_id = @InstructorId AND cs.section_id = @SectionId AND cs.course_code = @CourseCode AND ar.session_id = @sessionid AND ar.attendance_value = 1";
                    try
                    {
                        con.Open();
                        SqlCommand read = new SqlCommand(query2, con);
                        read.Parameters.AddWithValue("@InstructorId", InstructorId);
                        read.Parameters.AddWithValue("@SectionId", SectionId);
                        read.Parameters.AddWithValue("@CourseCode", CourseCode);
                        read.Parameters.AddWithValue("@sessionid", SessionId);
                        
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
                        "SELECT DISTINCT s.student_id, s.name AS student_name, ar.attendance_value FROM student s JOIN attendance_record ar ON s.student_id = ar.student_id JOIN course_section cs ON ar.section_id = cs.section_id JOIN teach t ON cs.instructor_id = t.instructor_id AND cs.course_code = t.course_code JOIN class_session css on css.section_id = cs.section_id WHERE t.instructor_id = @InstructorId AND cs.section_id = @SectionId AND cs.course_code = @CourseCode AND ar.session_id = @sessionid AND ar.attendance_value = 0";
                    try
                    {
                        con.Open();
                        SqlCommand read = new SqlCommand(query2, con);
                        read.Parameters.AddWithValue("@InstructorId", InstructorId);
                        read.Parameters.AddWithValue("@SectionId", SectionId);
                        read.Parameters.AddWithValue("@CourseCode", CourseCode); 
                        read.Parameters.AddWithValue("@sessionid", SessionId);
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
            LectureCode = rnd.Next(10000,99999).ToString();
            Console.WriteLine("onget"+LectureCode);
        }

        

        public IActionResult OnPost()
        {
            
            string conString =
                "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
            using (SqlConnection con = new SqlConnection(conString))
            {
                string countQuery = "SELECT TOP 1 cs.session_id + 1 FROM class_session cs ORDER BY cs.session_id DESC";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(countQuery, con);
                    SqlDataReader reader = read.ExecuteReader();
                    while (reader.Read())
                    {
                        Count = (int)reader[0];
                    }
                    Console.WriteLine("count is " +Count.ToString());
                }   
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            using (SqlConnection con = new SqlConnection(conString))
            {
                string Query = "SELECT session_id FROM class_session WHERE section_id = @SectionId AND week = @Week";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(Query, con);
                    read.Parameters.AddWithValue("@SectionId", SectionId);
                    read.Parameters.AddWithValue("@Week", Week);
                    SqlDataReader reader = read.ExecuteReader();
                    while (reader.Read())
                    {
                        SessionId = (int)reader[0];
                    } 
                    Console.WriteLine("session" + SessionId.ToString());
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            
            using (SqlConnection con = new SqlConnection(conString))
            {
                TempData.Peek(LectureCode);
                string query4 =
                    "INSERT INTO class_session (session_id,room, week, section_id, attendence_code) VALUES (@Count,@room, @Week, @Sectionid,@LectureCode);";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(query4, con);
                    read.Parameters.AddWithValue("@Count", Count);
                    read.Parameters.AddWithValue("@room", Room);
                    read.Parameters.AddWithValue("@Week", week);
                    read.Parameters.AddWithValue("@Sectionid", Section_Id);
                    read.Parameters.AddWithValue("@LectureCode", LectureCode);
                    Console.WriteLine("onpost"+LectureCode);
                    read.ExecuteNonQuery();
                    Console.WriteLine("Count2"+Count.ToString());
                    Console.WriteLine("Room"+Room);
                    Console.WriteLine("Week"+week.ToString());
                    Console.WriteLine("Section"+Section_Id.ToString());
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Count2"+Count.ToString());
                }
            }
            return RedirectToPage("/InstructorPage", new { InstructorId, SectionId, SessionId, CourseCode, AttendanceValue, Week, Count });
        }
        
    }
}
