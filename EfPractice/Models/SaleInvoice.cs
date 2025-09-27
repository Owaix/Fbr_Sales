using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfPractice.Models
{
    public class SaleInvoice : IHasCompany
    {
        [Key]
        public int Id { get; set; }
        public string InvoiceType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BuyerNTNCNIC { get; set; }
        public string BuyerBusinessName { get; set; }
        public string BuyerProvince { get; set; }
        public string BuyerAddress { get; set; }
        public string BuyerRegistrationType { get; set; }
        public string InvoiceRefNo { get; set; }
        public string ScenarioId { get; set; }
        public int CompanyId { get; set; }
        public string? invoiceNumber { get; set; }
        public DateTime? dated { get; set; }    
        public List<SaleInvoiceItem> Items { get; set; } = new List<SaleInvoiceItem>();
    }

    public class SaleInvoiceItem
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("SaleInvoice")]
        public int SaleInvoiceId { get; set; }
        public string? HsCode { get; set; }
        public string? ProductDescription { get; set; }
        public string? Rate { get; set; }
        public string? UoM { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalValues { get; set; }
        public decimal ValueSalesExcludingST { get; set; }
        public decimal FixedNotifiedValueOrRetailPrice { get; set; }
        public decimal SalesTaxApplicable { get; set; }
        public decimal SalesTaxWithheldAtSource { get; set; }
        public decimal ExtraTax { get; set; }
        public decimal FurtherTax { get; set; }
        public string? SroScheduleNo { get; set; }
        public decimal FedPayable { get; set; }
        public decimal Discount { get; set; }
        public string? SaleType { get; set; }
        public string? SroItemSerialNo { get; set; }
    }
}