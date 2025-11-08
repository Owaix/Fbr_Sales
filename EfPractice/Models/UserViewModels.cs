using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public class UserEditViewModel
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string RoleName { get; set; } = "User"; // Admin, Manager, Staff, User
        [Display(Name = "Password")]
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }
        [DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }

    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
        [DataType(DataType.Password), Compare("NewPassword")] public string ConfirmPassword { get; set; } = string.Empty;
    }
}
