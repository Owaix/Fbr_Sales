using EfPractice.Models;

namespace EfPractice.Models
{
    public class PrintInvoiceViewModel
    {
        public SaleInvoice Invoice { get; set; }
        public Company? Company { get; set; }
        public decimal SubTotal => Invoice?.Items?.Sum(i => i.TotalValues) ?? 0m;
        public decimal TotalQuantity => Invoice?.Items?.Sum(i => i.Quantity) ?? 0m;
    }
}
