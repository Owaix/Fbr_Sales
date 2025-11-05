using System;
using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public class Tax : IHasCompany
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Code { get; set; } = string.Empty;
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        public decimal DefaultRate { get; set; }
        [MaxLength(30)]
        public string? DefaultRateType { get; set; } // Percentage / Fixed
        public int? SalesAccountHeadId { get; set; }
        public int? PurchaseAccountHeadId { get; set; }
        [MaxLength(30)]
        public string? TaxType { get; set; } // e.g. GST, VAT
        public bool Active { get; set; } = true;
        public int CompanyId { get; set; }
        // Audit fields
        [MaxLength(100)] public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [MaxLength(100)] public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
