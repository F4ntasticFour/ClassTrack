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
        
        //On Post Event Function
        public IActionResult OnPost()
        {
            //While the input state is valid, the sql request command is executed
            while (ModelState.IsValid)
            {
                //Connection String to Azure Sql Database
                const string connectionString = "Server=localhost; Database=ClassTrack; User Id=sa; Password=Saf4002ey_";

                using var connection = new SqlConnection(connectionString);
                
                connection.Open();

                var query =
                    "SELECT * FROM Users WHERE USER_NAME = @UserName AND USER_PASSWORD = @Password";

                var command = new SqlCommand(query, connection);

                //Add Parameters with input values
                command.Parameters.AddWithValue("@UserName", userNameInput);
                command.Parameters.AddWithValue("@Password", passwordInput);

                var reader = command.ExecuteReader();
                
                //If Reader returns True (Data is retrieved from database) proceed
                if (reader.Read())
                {
                    // Authentication successful
                    //Get user ID
                    User_ID = reader.GetInt32(0);
                    return RedirectToPage("SigninSuccess", "SigninSuccess",
                        new { UserName = userNameInput, UserID = User_ID });
                }

                {
                    // Authentication failed
                    //Throw "Invalid username and password" error
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return Page();
                }
            }
            return Page();
        }
    }
}