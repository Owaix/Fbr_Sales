using System.ComponentModel.DataAnnotations;

namespace EfPractice.Models
{
    public class CustomerViewModel
    {
        public Customer Customer { get; set; } = new Customer();
        public List<Customer> Customers { get; set; } = new List<Customer>();
    }

    public class ItemsViewModel
    {
        public Item Item { get; set; } = new Item();
        public List<Item> Items { get; set; } = new List<Item>();
    }

    public class CompanyViewModel
    {
        public Company Company { get; set; } = new Company();
        public List<Company> Companies { get; set; } = new List<Company>();
    }


    public class Items
    {
        public int IID { get; set; }
        public string IName { get; set; }
        public string Desc { get; set; }
        public int? SCatID { get; set; }
        public double? SalesPrice { get; set; }
        public int? UOM { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CompanyID { get; set; }
        public int? SaleTax { get; set; }
    }

        [Key]   // ✅ Explicit primary key
        public int CID { get; set; }

        [Required(ErrorMessage = "Customer name is required")]
        [MaxLength(100, ErrorMessage = "Customer name cannot exceed 100 characters")]
        public string? CusName { get; set; }
        public int? PType_ID { get; set; }
        public string? Add { get; set; }
        public string? NTN_No { get; set; }
        public string? ContactPerson { get; set; }
        public string? Owner_Name { get; set; }
        [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "Only numbers and '-' are allowed")]
        public string? Cell { get; set; }
        public string? Eml { get; set; }
        public string? Tel { get; set; }
        public int? SID { get; set; }
        public int? ZID { get; set; }
        public string? AddPer { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public bool? Black_List { get; set; }
        public string? War_Cls { get; set; }
        public string? War_DT { get; set; }
        public string? Prn { get; set; }
        public bool? InActive { get; set; }
        public string? Land { get; set; }
        public int? City { get; set; }
        public int? Village { get; set; }
        public int CompanyId { get; set; }//{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public decimal? CollectPerMonth { get; set; }

        [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "Only numbers and '-' are allowed")]
        public string? MrNO { get; set; }
    }
    public class SalesMaster
    {
        public int RID { get; set; }
        public double? CompID { get; set; }
        public string RID2 { get; set; }
        public DateTime? EDate { get; set; }
        public string CAC_Code { get; set; }
        public double? AC_Code { get; set; }
        public string Ship_To { get; set; }
        public string Ship_ID { get; set; }
        public string Trans_ID { get; set; }
        public string BiltyNo { get; set; }
        public double? SID { get; set; }
        public double? WID { get; set; }
        public string Rem { get; set; }
        public double? NetAmt2 { get; set; }
        public double? DisAmt { get; set; }
        public double? PreBal { get; set; }
        public double? NetAmt { get; set; }
        public string AC_Code3 { get; set; }
        public string CashAmt { get; set; }
        public double? APost { get; set; }
        public double? Posted { get; set; }
        public double? RCancel { get; set; }
        public double? War_Cls { get; set; }
        public double? Create_User_ID { get; set; }
        public DateTime? Create_Date { get; set; }
        public double? Edit_User_ID { get; set; }
        public DateTime? Edit_Date { get; set; }
        public string Del_User_ID { get; set; }
        public string Del_Date { get; set; }
        public string InvNo { get; set; }
        public DateTime? InvDT { get; set; }
        public string InvType { get; set; }
        public int? CstId { get; set; }
        public decimal? TotalAmount { get; set; }
        public string CstInvNo { get; set; }
        public string VenInvDate { get; set; }
        public double? TransportExpense { get; set; }
        public string ReferenceNo { get; set; }
        public string CashCustomer { get; set; }
        public int? DiffCompany { get; set; }
        public bool? DiffCompanyStatus { get; set; }
    }
    public class SalesDetail
    {
        public int SD_ID { get; set; }
        public double? RID { get; set; }
        public double? IID { get; set; }
        public double? BNID { get; set; }
        public DateTime? ExpDT { get; set; }
        public string CTN { get; set; }
        public double? PCS { get; set; }
        public string SCH { get; set; }
        public double? Qty { get; set; }
        public double? PurPrice { get; set; }
        public double? SalesPriceP { get; set; }
        public string AddPer { get; set; }
        public string AddAmt { get; set; }
        public double? SalesPrice { get; set; }
        public double? PAmt { get; set; }
        public string DisP { get; set; }
        public string DisAmt { get; set; }
        public string DisRs { get; set; }
        public double? Amt2 { get; set; }
        public double? Amt { get; set; }
        public double? PCK_Det { get; set; }
        public double? SRT { get; set; }
        public double? RCancel { get; set; }
        public string ExpireDate { get; set; }
        public string BatchNo { get; set; }
        public int? Pur_D_UnitID { get; set; }
    }

    public class FbrResponse
    {
        public string invoiceNumber { get; set; }
        public string? dated { get; set; }
        public ValidationResponse validationResponse { get; set; }
    }

    public class ValidationResponse
    {
        public string statusCode { get; set; }
        public string status { get; set; }
        public string errorCode { get; set; }
        public string error { get; set; }
        public List<InvoiceStatus> invoiceStatuses { get; set; }
    }

    public class InvoiceStatus
    {
        public string itemSNo { get; set; }
        public string statusCode { get; set; }
        public string status { get; set; }
        public string invoiceNo { get; set; }
        public string errorCode { get; set; }
    }
}