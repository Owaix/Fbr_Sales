using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public class SubCategory : IHasCompany
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; } // maps to Cate.Cid
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        public int CompanyId { get; set; }
    }
}