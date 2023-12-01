using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace ClassTrack.Pages
{
    public class SigninModel : PageModel
    {
        //UserName Input
        [BindProperty]
        [Required(ErrorMessage = "Name is required.")]
        public string userNameInput { get; set; }

        // Password Input
        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(15, ErrorMessage = "Password cannot exceed 15 characters.")]
        public string passwordInput { get; set; }

        // User ID Variable
        public int User_ID { get; set; }

        public string GetUserRoleUsingADO(int User_ID, string passwordInput)
        {
            using (SqlConnection connection = new SqlConnection("Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS"))
            {
                connection.Open();

                // Check if the user is in the Student table.
                string studentQuery = "SELECT password FROM student WHERE student_id = @UserId AND password = @Password";
                using (SqlCommand studentCommand = new SqlCommand(studentQuery, connection))
                {
                    studentCommand.Parameters.AddWithValue("@UserId", User_ID);
                    studentCommand.Parameters.AddWithValue("@Password", passwordInput);
                    using (var studentReader = studentCommand.ExecuteReader())
                    {
                        if (studentReader.Read())
                        {
                            return "student";
                        }
                    }
                }

                // Check if the user is in the Instructor table.
                string instructorQuery = "SELECT 1 FROM instructor WHERE instructor_id = @UserId AND password = @Password";
                using (SqlCommand instructorCommand = new SqlCommand(instructorQuery, connection))
                {
                    instructorCommand.Parameters.AddWithValue("@UserId", User_ID);
                    instructorCommand.Parameters.AddWithValue("@Password", passwordInput);
                    using (var instructorReader = instructorCommand.ExecuteReader())
                    {
                        if (instructorReader.Read())
                        {
                            return "instructor";
                        }
                    }
                }

                // Close the connection after both queries.
                return "not found";
            }
        }


        public IActionResult OnPost()
        {
            // While the input state is valid, the SQL request command is executed
            while (ModelState.IsValid)
            {
                string userRole = GetUserRoleUsingADO(User_ID, passwordInput);

                // Redirect based on the user's role.
                if (userRole == "student")
                {
                    return RedirectToPage("/StudentPage");
                } 
                if (userRole == "instructor")
                {
                    return RedirectToPage("/InstructorPage");
                }
                ModelState.AddModelError(string.Empty,userRole );
            }
            return Page();
        }
    }
}