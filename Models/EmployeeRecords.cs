using System.ComponentModel.DataAnnotations;

namespace MyWebApplicationCRUD.Models
{

    public class EmployeeRecords
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Designation { get; set; }
        [Required]
        public string? Address {get; set;}
        
        public bool IsDeleted { get; set; }

        public bool IsActive { get; set;}
    }
}
