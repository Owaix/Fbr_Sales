using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public partial class Cate : IHasCompany
    {
        [Key]
        public int Cid { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Aid { get; set; }
        public string? Aname { get; set; }
        public int? Mid { get; set; }
        public string? Mn { get; set; }
        public int CompanyId { get; set; }
    }
}
