using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public class Customer : IHasCompany
    {
        [Key]
        public int CID { get; set; }
        public string? CusName { get; set; }
        public string? NTN_No { get; set; }
        public string? Add { get; set; }
        public int? City { get; set; }
        public bool? InActive { get; set; }
        public string? Cell { get; set; }
        public string? MrNO { get; set; }
        public int CompanyId { get; set; }
        public string? RegistrationType { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string? Prn { get; set; }
    }
}
