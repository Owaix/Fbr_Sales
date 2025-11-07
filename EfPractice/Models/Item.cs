using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfPractice.Models
{
    public class Item : IHasCompany
    {
        [Key]
        public int Id { get; set; }

        // Existing fields
        public string? HSCode { get; set; }
        public string? ItemCode { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? UOM { get; set; }
        public decimal? Rate { get; set; }
        [Column("CompanyID")]
        public int CompanyId { get; set; }

        // New / extended fields for redesigned screen
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public int? ReorderLevel { get; set; }
        public string? SubCategory { get; set; }
        public decimal? BuyingRate { get; set; }
        public decimal? ListPrice { get; set; }
        public decimal? PosRate { get; set; } // Price 1 / POS Rate
        public bool Active { get; set; } = true;

        // Barcode section
        public string? BarcodeNo { get; set; }
        public decimal? BarcodePrice { get; set; }
        public string? Brand { get; set; }
        [MaxLength(500)] public string? Label { get; set; }

        // Tax section
        public int? TaxId { get; set; }
        public decimal? TaxRate { get; set; }
        public string? TaxType { get; set; }
    }
}
