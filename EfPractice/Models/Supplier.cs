using System;
using System.Collections.Generic;

namespace EfPractice.Models
{
    public partial class Supplier
    {
        public int? Headcode { get; set; }
        public string? Subcode { get; set; }
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
    }
}
