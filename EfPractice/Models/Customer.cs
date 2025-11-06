using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public class Customer : IHasCompany
    {
        [Key]
        public int CustomerID { get; set; }
        // Existing fields (kept for backward compatibility)
        public string? CusName { get; set; } // Will serve as Name
        public string? NTN_No { get; set; } // Legacy NTN/CNIC
        public string? Add { get; set; } // Address
        public int? City { get; set; } // Legacy city id
        public bool? InActive { get; set; } // Legacy status (true = inactive)
        public string? Cell { get; set; } // Legacy cell / telephone
        public string? MrNO { get; set; } // Legacy contract no
        public int CompanyId { get; set; }
        public string? RegistrationType { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string? Prn { get; set; }

        // New fields for redesigned screen
        [MaxLength(50)] public string? Code { get; set; }
        [Required, MaxLength(150)] public string? Name { get => CusName; set => CusName = value; }
        [MaxLength(50)] public string? CustomerType { get; set; }
        [MaxLength(50)] public string? Telephone { get; set; }
        [MaxLength(100)] public string? BusinessSector { get; set; }
        [MaxLength(100)] public string? Country { get; set; }
        [MaxLength(100)] public string? State { get; set; }
        [MaxLength(100)] public string? CityName { get; set; }
        [MaxLength(50)] public string? ProductPriceLevel { get; set; }
        public bool Active { get => !(InActive ?? false); set => InActive = !value; }
        public bool ATL { get; set; }
        public bool ShouldBeSupplier { get; set; }
        [MaxLength(500)] public string? Address { get => Add; set => Add = value; }

        // Additional Information
        [MaxLength(100)] public string? AccountManager { get; set; }
        [MaxLength(150)] public string? ContactPerson { get; set; }
        [MaxLength(50)] public string? Mobile { get; set; }
        [EmailAddress, MaxLength(150)] public string? Email { get; set; }
        [MaxLength(100)] public string? Area { get; set; }
        [MaxLength(50)] public string? Fax { get; set; }
        [Range(0, double.MaxValue)] public decimal? CreditLimit { get; set; }
        [Range(0, 365)] public int? CreditPeriod { get; set; }
        [MaxLength(100)] public string? CreditTerms { get; set; }
        [Url, MaxLength(200)] public string? Website { get; set; }
        [MaxLength(100)] public string? STRegionNo { get; set; }
        [MaxLength(50)] public string? NtnCnic { get => NTN_No; set => NTN_No = value; }
        [MaxLength(1000)] public string? Remarks { get; set; }
    }
}
