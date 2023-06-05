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

        
        [NotMapped]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }


        [Required]
        public string? UserType { get; set; }
    }
}
