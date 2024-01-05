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
        public int UserId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password Must exceed 8 characters.")]
        public required string PasswordInput { get; set; }
        
        private string GetUserRoleUsingADO(int userId, string passwordInput)
        {
            using (SqlConnection connection =
                   new SqlConnection(
                       "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS"))
            {
                connection.Open();

                // Check if the user is in the Student table.
                string studentQuery =
                    "SELECT student_id FROM student WHERE student_id = @UserId AND password = @Password";
                using (SqlCommand studentCommand = new SqlCommand(studentQuery, connection))
                {
                    studentCommand.Parameters.AddWithValue("@UserId", userId);
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
                    instructorCommand.Parameters.AddWithValue("@UserId", userId);
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

            string userRole = GetUserRoleUsingADO(UserId, PasswordInput);

            if (userRole == "student")
            {
                return RedirectToPage("/StudentPage", new { Student_id = UserId });
            }
            else if (userRole == "instructor")
            {
                return RedirectToPage("/InstructorPage", new { instructorId = UserId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }
        }
    }

}
