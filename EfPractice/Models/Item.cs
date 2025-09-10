using System;
using System.Collections.Generic;

namespace EfPractice.Models
{
    public partial class Item
    {
        public int Itcode { get; set; }
        public string? Itname { get; set; }
        public string? Unit { get; set; }
        public decimal? Srate { get; set; }
        public decimal? Weight { get; set; }
        public string? Pack { get; set; }
        public string? Type { get; set; }
        public int? Acid { get; set; }
        public decimal? Amt { get; set; }
        public decimal? Prate { get; set; }
        public string? Ic { get; set; }
        public string? Glsa { get; set; }
        public string? Glpa { get; set; }
        public string? Glca { get; set; }
        public string? Expac { get; set; }
    }
}
