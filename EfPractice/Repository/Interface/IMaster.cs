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

        Task <List<Head>> GetHeadListAsync();

        #endregion


        #region City

        Task<List<City>> GetCityListAsync();
        #endregion
        #region Customer

        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<int> AddAsync(Customer customer);
        Task<int> UpdateAsync(Customer customer);
        Task<int> DeleteAsync(int id);
        #endregion

    }
}
