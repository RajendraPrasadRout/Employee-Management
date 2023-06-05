using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationCRUD.Models
{
    public class UserCredentials
    {
        [Key]
        public string? UserId { get; set; }

        
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? UserType { get; set; }
    }
}
