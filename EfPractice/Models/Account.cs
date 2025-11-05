using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public class Account : IHasCompany
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string AccountId { get; set; } = string.Empty;
        [Required]
        [MaxLength(150)]
        public string AccountTitle { get; set; } = string.Empty;
        public int CompanyId { get; set; }
    }
}