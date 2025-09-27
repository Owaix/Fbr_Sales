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

        public string? HSCode { get; set; }

        public string? ItemCode { get; set; }

        [Required]
        public string ItemName { get; set; }

        public string? Category { get; set; }

        public string? Description { get; set; }

        public string? UOM { get; set; }

        public decimal? Rate { get; set; }

        [Column("CompanyID")]
        public int CompanyId { get; set; }
    }
}
