using EfPractice.Models;
using EfPractice.Models.CustomerModel;
using Microsoft.EntityFrameworkCore.Storage;
using EfPractice.Areas.Identity.Data;

namespace EfPractice.Repository.Interface
{
    public interface IMaster
    {
        #region ItemHeader
        Task<List<Imh>> GetItemHeaderAsync();
        Task<bool> UpdateItemHeaderAsync(Imh model);
        Task<bool> AddItemHeaderAsync(Imh model);
        Task<ItemCatergory> GetHeaderListAsync();

        #endregion


        #region ItemCatergoryRegistrarion


        Task<bool> ADDItemCatergoryRegistrarionAsync(Cate model);
        Task<bool> UPDATEItemCatergoryRegistrarionAsync(Cate model);

        Task<List<Cate>> GETItemCatergoryRegistrarionAsync();
        Task<ItemRegistrarion> GetCategoryListAsync();
        #endregion

        // SubCategory CRUD
        Task<SubCategory?> GetSubCategoryByIdAsync(int id);
        Task<List<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryId);
        Task<int> AddSubCategoryAsync(SubCategory subCategory);
        Task<int> UpdateSubCategoryAsync(SubCategory subCategory);
        Task<int> DeleteSubCategoryAsync(int id);

        #region Item

        Task<bool> AddItemRegistration(Item model);
        Task<bool> UpdateItemRegistration(Item model);
        Task<List<ItemRegistrarion>> GetItemRegistration();

        #endregion

        #region Supplier
        Task<List<Supplier>> GetSupplierAsync();
        Task<bool> UpdateSupplierAsync(Supplier model);
        Task<bool> AddSupplierAsync(Supplier model);
        //  Task<Supplier> GetSupplierListAsync();

        #endregion


        #region Party
        Task<List<Party>> GetPartyAsync();
        Task<bool> UpdatePartyAsync(Party model);
        Task<bool> AddPartyAsync(Party model);
        //  Task<Party> GetPartyListAsync();
        #endregion

        #region Head
        Task<bool> AddHeadAsync(Head model);
        Task<bool> UpdateHeadAsync(Head model);

        Task<List<Head>> GetHeadListAsync();

        #endregion


        #region City

        Task<List<City>> GetCityListAsync();
        #endregion
        #region Customer
        Task<List<Customer>> GetAllCustomersAsync(int companyId);
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<int> AddCustomerAsync(Customer customer);
        Task<int> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomerAsync(int id);
        Task<Customer?> GetCustomerByNTNAsync(string ntn, int companyId);
        #endregion

        #region Item (with companyId filter)
        Task<List<Item>> GetAllItemsAsync();
        Task<Item?> GetItemByIdAsync(int id);
        Task<int> AddItemAsync(Item item);
        Task<int> UpdateItemAsync(Item item);
        Task<int> DeleteItemAsync(int id);
        #endregion

        Task<Cate?> GetCateByIdAsync(int id);
        Task<int> AddCateAsync(Cate cate);
        Task<int> UpdateCateAsync(Cate cate);
        Task<int> DeleteCateAsync(int id);

        Task<List<Company>> GetAllCompaniesAsync();
        Task<List<Company>> GetCompanyAsync(Company filter);
        Task<Company?> GetCompanyByIdAsync(int id);
        Task<int> AddCompanyAsync(Company company);
        Task<int> UpdateCompanyAsync(Company company);
        Task<int> DeleteCompanyAsync(int id);

        Task<SaleInvoice> GetSaleInvoiceByIdAsync(int id);
        Task<int> AddSaleInvoiceAsync(SaleInvoice invoice);
        Task<int> UpdateSaleInvoiceAsync(SaleInvoice invoice);
        Task<HttpResponseMessage> SendInvoiceToFbrAsync(SaleInvoice invoice);
        Task<int> AddSaleInvoiceDetailAsync(List<SaleInvoiceItem> invoice);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<SaleInvoice?> GetSaleInvoiceByNumberAsync(string invoiceNo);
        Task<List<SaleInvoice>> GetSaleInvoices();
        Task<List<ApplicationUser>> GetAllUsersAsync();
        Task AddUserAsync(UserEditViewModel model);
        Task UpdateUserAsync(UserEditViewModel model);
        Task DeleteUserAsync(string id);
        #region Tax
        Task<List<Tax>> GetTaxesAsync(int companyId);
        Task<Tax?> GetTaxByIdAsync(int id);
        Task<int> AddTaxAsync(Tax tax);
        Task<int> UpdateTaxAsync(Tax tax);
        Task<int> DeleteTaxAsync(int id);
        #endregion

        // Accounts
        Task<List<Account>> GetAccountsAsync(int companyId);
        Task<Account?> GetAccountByIdAsync(int id);
        Task<int> AddAccountAsync(Account account);
        Task<int> UpdateAccountAsync(Account account);
        Task<int> DeleteAccountAsync(int id);
    }
}
