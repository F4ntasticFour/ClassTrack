using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClassTrack.Pages
{
    public class Register : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Name is required.")]
        public string UserNameInput { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "User ID is required.")]
        [MaxLength(15, ErrorMessage = "User ID cannot exceed 15 characters.")]
        public string UserIDInput { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string UserEmailInput { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Gender is required.")]
        public string UserGender { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must exceed 8 characters.")]
        public string UserPasswordInput { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8)]
        [Compare("UserPasswordInput", ErrorMessage = "Passwords must match.")]
        public string UserPasswordConfirmationInput { get; set; }
        
        [BindProperty]
        public string UserType { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed, return the page with validation errors
                return Page();
            }

            if (UserPasswordInput != UserPasswordConfirmationInput)
            {
                // Passwords don't match, add an error to the ModelState and return the page
                ModelState.AddModelError(string.Empty, "Passwords must match.");
                return Page();
            }

            const string connectionString = "Server=34.155.113.141,1433; Database=classtrack; User Id=sqlserver; Password=YUgMfE.H0^4A'zhS";

            using var connection = new SqlConnection(connectionString);
            if (UserType == "student")
            {
                try
                {
                    connection.Open();

                    var query = "INSERT INTO student(student_id, name, gender, email, advisor_id, password) VALUES (@UserID, @UserName, @UserGender, @UserEmail, 1, @UserPassword)";

                    using var command = new SqlCommand(query, connection);

                    // Add Parameters with input values
                    command.Parameters.AddWithValue("@UserID", UserIDInput);
                    command.Parameters.AddWithValue("@UserName", UserNameInput);
                    command.Parameters.AddWithValue("@UserGender", UserGender);
                    command.Parameters.AddWithValue("@UserEmail", UserEmailInput);
                    command.Parameters.AddWithValue("@UserPassword", UserPasswordInput);

                    // Execute the query
                    command.ExecuteNonQuery();

                    return RedirectToPage("/Index");
                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, etc.
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                    return Page();
                } 
            }
            else
            {
                try
                {
                    connection.Open();

                    var query = "INSERT INTO instructor(instructor_id, name, email, password) VALUES (@UserID, @UserName, @UserEmail, @UserPassword)";

                    using var command = new SqlCommand(query, connection);

                    // Add Parameters with input values
                    command.Parameters.AddWithValue("@UserID", UserIDInput);
                    command.Parameters.AddWithValue("@UserName", UserNameInput);
                    command.Parameters.AddWithValue("@UserEmail", UserEmailInput);
                    command.Parameters.AddWithValue("@UserPassword", UserPasswordInput);

                    // Execute the query
                    command.ExecuteNonQuery();

                    return RedirectToPage("/Index");
                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, etc.
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                    return Page();
                }
            }
        }
    }
}
