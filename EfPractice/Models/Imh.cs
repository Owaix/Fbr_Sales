using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public partial class Imh
    {
        [Key]
        public int Mid { get; set; }
        public string? Mname { get; set; }
    }
}
