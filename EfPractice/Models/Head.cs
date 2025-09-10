using System;
using System.Collections.Generic;

namespace EfPractice.Models
{
    public partial class Head
    {
        public int? HeaderId { get; set; }
        public string? HeaderName { get; set; }
        public string? Type { get; set; }
        public decimal? Opening { get; set; }
    }
}
