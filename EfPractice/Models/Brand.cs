using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public class Brand : IHasCompany
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public int CompanyId { get; set; }
    }
}