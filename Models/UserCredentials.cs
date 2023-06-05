using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApplicationCRUD.Models
{
    public class UserCredentials
    {
        [Key]
        public string? UserId { get; set; }

        
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        /* [Required(ErrorMessage = "The confirm password field is required.")]
         [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]  */
        [NotMapped]
        [DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }


        [Required]
        public string? UserType { get; set; }
    }
}
