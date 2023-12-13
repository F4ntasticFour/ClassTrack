using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace ClassTrack.Pages
{
    public class SigninModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "ID is required.")]
        public int User_ID { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(15, ErrorMessage = "Password cannot exceed 15 characters.")]
        public string passwordInput { get; set; } // Set this property with the appropriate value

        private string GetUserRoleUsingADO(int User_ID, string passwordInput)
        {
            using (SqlConnection connection =
                   new SqlConnection(
                       "Server=localhost; Database=master; User Id=sa; Password=reallyStrongPwd123"))
            {
                connection.Open();

                // Check if the user is in the Student table.
                string studentQuery =
                    "SELECT student_id FROM student WHERE student_id = @UserId AND password = @Password";
                using (SqlCommand studentCommand = new SqlCommand(studentQuery, connection))
                {
                    studentCommand.Parameters.AddWithValue("@UserId", User_ID);
                    studentCommand.Parameters.AddWithValue("@Password", passwordInput);

                    if (studentCommand.ExecuteScalar() != null)
                    {
                        return "student";
                    }
                }

                // Check if the user is in the Instructor table.
                string instructorQuery =
                    "SELECT instructor_id FROM instructor WHERE instructor_id = @UserId AND password = @Password";
                using (SqlCommand instructorCommand = new SqlCommand(instructorQuery, connection))
                {
                    instructorCommand.Parameters.AddWithValue("@UserId", User_ID);
                    instructorCommand.Parameters.AddWithValue("@Password", passwordInput);

                    if (instructorCommand.ExecuteScalar() != null)
                    {
                        return "instructor";
                    }
                }

                // Handle the case where the user is not found in either table.
                return string.Empty;
            }
        }

        public IActionResult OnPost()
        {
            // Set User_ID with the appropriate value

            string userRole = GetUserRoleUsingADO(User_ID, passwordInput);

            if (userRole == "student")
            {
                return RedirectToPage("/StudentPage", new { Student_id = User_ID });
            }
            else if (userRole == "instructor")
            {
                return RedirectToPage("/InstructorPage", new { instructor_id = User_ID });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }
        }
    }

}
