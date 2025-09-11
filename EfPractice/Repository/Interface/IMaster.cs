using EfPractice.Models;
using EfPractice.Models.CustomerModel;


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
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<int> AddCustomerAsync(Customer customer);
        Task<int> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomerAsync(int id);
        #endregion

        Task<List<Item>> GetAllItemsAsync();
        Task<Item?> GetItemByIdAsync(int id);
        Task<int> AddItemAsync(Item item);
        Task<int> UpdateItemAsync(Item item);
        Task<int> DeleteItemAsync(int id);

        Task<List<Cate>> GetAllCatesAsync();
        Task<Cate?> GetCateByIdAsync(int id);
        Task<int> AddCateAsync(Cate cate);
        Task<int> UpdateCateAsync(Cate cate);
        Task<int> DeleteCateAsync(int id);

        Task<List<Company>> GetAllCompaniesAsync();
        Task<Company?> GetCompanyByIdAsync(int id);
        Task<int> AddCompanyAsync(Company company);
        Task<int> UpdateCompanyAsync(Company company);
        Task<int> DeleteCompanyAsync(int id);

    }
}
