using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClassTrack.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 15 characters.")]
        public string Name { get; set; }

        [BindProperty]
        [MaxLength(15, ErrorMessage = "Nickname cannot exceed 15 characters.")]
        public string Nickname { get; set; }

        [BindProperty]
        [StringLength(11, MinimumLength = 11 , ErrorMessage = "Phone number must be 11 characters.")]
        public string PhoneNumber { get; set; }


        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                return RedirectToPage("/Success");
            }

            return Page();
        }
    }
}