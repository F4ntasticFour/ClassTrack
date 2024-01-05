﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClassTrack.Pages
{

    public class ShowCode : PageModel
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
        public int Instructor_ID { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SectionId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int SessionId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CourseCode { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int Week { get; set; }
        [BindProperty(SupportsGet = true)]
        public int code { get; set; }

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
                    read.Parameters.AddWithValue("@InstructorId", Instructor_ID);
                    SqlDataReader reader = read.ExecuteReader();
                    while (reader.Read())
                    {
                        int sectionId = (int)reader[0];
                        string courseCode = reader[1].ToString();
                        string instructorName = reader[2].ToString();
                        int sessionId = (int)reader[3];

                        // Add to lists only if the data is not already present
                        if (!SectionIds.Contains(sectionId))
                            SectionIds.Add(sectionId);

                        if (!CourseCodes.Contains(courseCode))
                            CourseCodes.Add(courseCode);

                        if (string.IsNullOrEmpty(InstructorName))
                            InstructorName = instructorName; // Set once, assuming it remains the same for all rows

                        if (!SessionsIds.Contains(sessionId))
                            SessionsIds.Add(sessionId);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            using (SqlConnection con = new SqlConnection(conString))
            {
                string query2 = "SELECT attendence_code FROM class_session WHERE session_id = @SessionId";
                try
                {
                    con.Open();
                    SqlCommand read = new SqlCommand(query2, con);
                    read.Parameters.AddWithValue("@SessionId", SessionId);
                    SqlDataReader reader = read.ExecuteReader();
                    while (reader.Read())
                    {
                        code = (int)reader[0];
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
            string conString =
                "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";
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
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            return RedirectToPage("/show_code", new {Instructor_ID , SessionId});
        }
        
    }
}
