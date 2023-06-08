using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApplicationCRUD.Models
{

    public class EmployeeRecords
    {
        [Key]
        public int EmployeeId { get; set; }

        [MaxLength(30)]
        public string? Name { get; set; }

        
        public string? Email { get; set; }

        
        public string? Phone { get; set; }

        
        public string? Designation { get; set; }

        
        public string? Address { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        
        [Remote(action: "IsUserIdAvailable", controller: "EmployeeRecords", ErrorMessage = "The User ID is already taken.")]
        public string? UserId { get; set; }

        
        public string? Password { get; set;}

        
        public string? UserType { get; set;}

    }
}
