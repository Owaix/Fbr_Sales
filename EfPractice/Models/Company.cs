using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfPractice.Models
{
    [Index(nameof(CompanyName), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cell number is required")]
        [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "Only numbers and '-' are allowed")]
        public string CellNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "User role is required")]
        public string UserRole { get; set; } = string.Empty;

        [Required(ErrorMessage = "User name is required")]
        [MaxLength(50)]
        [RegularExpression(@"^\S+$", ErrorMessage = "Username cannot contain spaces")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;

    }
}
