using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfPractice.Models
{
    public class SaleInvoice : IHasCompany
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public int? CustomerId { get; set; }

        public string EmployeeRefer { get; set; }

        public string InvoiceType { get; set; }

        public string InventoryLocation { get; set; }

        [Required]
        public int? AccountId { get; set; }

        public bool Credit { get; set; }

        public bool Active { get; set; } = true;

        public decimal GrossAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal CarriageFreight { get; set; }
        public decimal TotalAdditionalTax { get; set; }
        public decimal NetTotal { get; set; }
        public decimal AmountReceived { get; set; }

        // Tax summary
        public int? TaxId { get; set; }
        public decimal? TaxRate { get; set; }
        public string TaxType { get; set; }

        // Navigation properties
        public List<SaleInvoiceItem> Items { get; set; } = new List<SaleInvoiceItem>();
        public int CompanyId { get; set; }
        public string invoiceNumber { get; set; }
        public string dated { get; set; }
    }

    public class SaleInvoiceItem
    {
        [Key]
        public int Id { get; set; }

        public string HsCode { get; set; }
        public int SaleInvoiceId { get; set; }

        [Required]
        public string ProductDescription { get; set; }

        public decimal Quantity { get; set; }

        public string UoM { get; set; }

        public decimal ListPrice { get; set; }

        public decimal Rate { get; set; }

        public decimal Discount { get; set; }

        public decimal ValueSalesExcludingST { get; set; }

        public decimal SalesTaxApplicable { get; set; }

        public decimal FurtherTax { get; set; }

        public decimal TotalValues { get; set; }
    }
}