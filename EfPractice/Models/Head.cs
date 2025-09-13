﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public partial class Head
    {
        [Key]
        public int? HeaderId { get; set; }
        public string? HeaderName { get; set; }
        public string? Type { get; set; }
        public decimal? Opening { get; set; }
    }
}
