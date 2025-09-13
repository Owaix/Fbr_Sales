using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public partial class Party
    {
        [Key]
        public int? Headcode { get; set; }
        public string Subcode { get; set; } = null!;
        public string? Subname { get; set; }
        public string? Addr { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Telno { get; set; }
        public string? Mobile { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Type { get; set; }
        public int? Term { get; set; }
        public int? Limit { get; set; }
        public string? Owner { get; set; }
        public decimal? Ob { get; set; }
        public string? St { get; set; }
        public string? Ntn { get; set; }
    }
}
