using System.ComponentModel.DataAnnotations;

namespace WebStock.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email or Username is required.")]
        [Display(Name = "Email or Username")]
        [StringLength(100, ErrorMessage = "Identifier cannot exceed 100 characters.")]
        public string Identifier { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string Password { get; set; }
    }
}
