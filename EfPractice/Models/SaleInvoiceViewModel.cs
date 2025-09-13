namespace EfPractice.Models
{
    public class SaleInvoiceViewModel
    {
        public SaleInvoice Invoice { get; set; }
        public List<SaleInvoiceItem> Items { get; set; } = new List<SaleInvoiceItem>();
    }
}