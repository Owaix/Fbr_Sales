using EfPractice.Models;
using EfPractice.Models.CustomerModel;
using EfPractice.Repository.Interface;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;

namespace EfPractice.Repository.Class
{
    public class Master : IMaster
    {
        private readonly StudentContext _studentDB;

        #region ItemHeader
        public Master(StudentContext studentDB)
        {
            _studentDB = studentDB;
        }

        public async Task<bool> AddItemHeaderAsync(Imh model)
        {
            model.Mid = 0;
            _studentDB.Imhs.Add(model);
            int rowsAffected = await _studentDB.SaveChangesAsync();

            // Optional: Use the rowsAffected value if needed
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }



        public async Task<List<Imh>> GetItemHeaderAsync()
        {
            List<Imh> List = new List<Imh>();

            List = await _studentDB.Imhs.ToListAsync();
            return List;

        }



        public async Task<bool> UpdateItemHeaderAsync(Imh model)
        {


            var existingItemHeader = await _studentDB.Imhs.FindAsync(model.Mid);

            if (existingItemHeader != null)
            {
                // Update the properties of the existing item header
                existingItemHeader.Mname = model.Mname;
                // Update other properties as needed

                // Save changes to the database
                //_studentDB.Imhs.Update(model);
                int rowsAffected = await _studentDB.SaveChangesAsync();

                // Optional: Use the rowsAffected value if needed
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                // Handle the case when the item header is not found
                // You might throw an exception or return null
                // For example, we will throw an exception here
                return false;
            }

        }


        public async Task<ItemCatergory> GetHeaderListAsync()
        {
            ItemCatergory itemCatergory = new ItemCatergory();

            itemCatergory.ImhList = new List<Imh>();

            itemCatergory.ImhList = await _studentDB.Imhs.ToListAsync();


            return itemCatergory;



        }

        #endregion


        #region ItemCatergory


        public async Task<bool> ADDItemCatergoryRegistrarionAsync(Cate model)
        {
            int maxCid = await _studentDB.Cates
                                .Select(c => (int?)c.Cid) // Cast to nullable int to handle empty sequence
                                .MaxAsync() ?? 0;

            model.Cid = maxCid + 1;
            _studentDB.Cates.Add(model);
            int rowsAffected = await _studentDB.SaveChangesAsync();

            // Optional: Use the rowsAffected value if needed
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UPDATEItemCatergoryRegistrarionAsync(Cate model)
        {

            var existingItemHeader = await _studentDB.Cates.FindAsync(model.Cid);

            if (existingItemHeader != null)
            {
                // Update the properties of the existing item header
                existingItemHeader.Mid = model.Mid;
                existingItemHeader.Name = model.Name;


                int rowsAffected = await _studentDB.SaveChangesAsync();

                // Optional: Use the rowsAffected value if needed
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                // Handle the case when the item header is not found
                // You might throw an exception or return null
                // For example, we will throw an exception here
                return false;
            }

        }

        public async Task<List<Cate>> GETItemCatergoryRegistrarionAsync()
        {
            List<Cate> cates = new List<Cate>();


            cates = await (from category in _studentDB.Cates
                           join header in _studentDB.Imhs
                           on category.Mid equals header.Mid
                           select new Cate
                           {
                               Cid = category.Cid,
                               Name = category.Name,
                               Mid = header.Mid,
                               Mn = header.Mname
                           }).ToListAsync();

            return cates;
        }

        public async Task<ItemRegistrarion> GetCategoryListAsync()
        {
            // Initialize the ItemRegistrarion object
            ItemRegistrarion itemCategory = new ItemRegistrarion();

            // Fetch the list of categories from the database
            itemCategory.CatergoryList = await _studentDB.Cates.ToListAsync();

            // Ensure the list is initialized, even if no data was fetched
            if (itemCategory.CatergoryList == null)
            {
                itemCategory.CatergoryList = new List<Cate>();
            }

            // Return the ItemRegistrarion object
            return itemCategory;
        }


        #endregion


        #region Item Registration


        public async Task<bool> AddItemRegistration(Item model)
        {
            Item item = new Item();
            //int Itcode = await _studentDB.Items
            //                    .Select(c => (int?)c.Itcode) 
            //                    .MaxAsync() ?? 0;

            //model.Itcode = Itcode + 1;
            //_studentDB.Items.Add(model);
            int rowsAffected = await _studentDB.SaveChangesAsync();

            // Optional: Use the rowsAffected value if needed
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> UpdateItemRegistration(Item model)
        {

            var existingItem = await _studentDB.Items.FindAsync(model.ItemCode);

            if (existingItem != null)
            {
                //existingItem.Itcode = model.ItemCode;
                //existingItem.Weight = model.Weight;
                //existingItem.Ic = model.Ic;
                //existingItem.Amt = model.Amt;
                //existingItem.Itname = model.ItemName;
                //existingItem.Prate = model.Rate;
                //existingItem.Srate = model.sa;
                //existingItem.Unit = model.Unit;


                int rowsAffected = await _studentDB.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                // Handle the case when the item header is not found
                // You might throw an exception or return null
                // For example, we will throw an exception here
                return false;
            }

        }
        public async Task<List<ItemRegistrarion>> GetItemRegistration()
        {
            List<ItemRegistrarion> Item = new List<ItemRegistrarion>();


            Item = await (from item in _studentDB.Items
                          join Catergory in _studentDB.Cates
                          on item.Id equals Catergory.Cid
                          select new ItemRegistrarion
                          {
                              //Itcode = item.Itcode,
                              //Itname = item.Itname,
                              Ic = Catergory.Cid.ToString(),
                              IcName = Catergory.Name,
                              //Unit = item.Unit,
                              //Rate = item.Srate,
                              //Weight = item.Weight,
                              //OpenAmt = item.Amt,
                              //Prate = item.Prate,
                          }).ToListAsync();

            return Item;
        }

        #endregion


        #region supplier
        public async Task<List<Supplier>> GetSupplierAsync()
        {
            List<Supplier> Supplier = new List<Supplier>();



            Supplier = await _studentDB.Suppliers.ToListAsync();


            return Supplier;
        }

        public async Task<bool> UpdateSupplierAsync(Supplier model)
        {
            var existingItemHeader = await _studentDB.Suppliers.FindAsync(model.Headcode);

            if (existingItemHeader != null)
            {
                // Update the properties of the existing item header
                existingItemHeader.Subname = model.Subcode;
                existingItemHeader.Term = model.Term;
                existingItemHeader.Telno = model.Telno;
                existingItemHeader.City = model.City;


                // Update other properties as needed

                // Save changes to the database
                //_studentDB.Imhs.Update(model);
                int rowsAffected = await _studentDB.SaveChangesAsync();

                // Optional: Use the rowsAffected value if needed
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                // Handle the case when the item header is not found
                // You might throw an exception or return null
                // For example, we will throw an exception here
                return false;
            }
        }

        public async Task<bool> AddSupplierAsync(Supplier model)
        {
            int Itcode = await _studentDB.Suppliers
                                .Select(c => (int?)c.Headcode) // Cast to nullable int to handle empty sequence
                                .MaxAsync() ?? 0;

            model.Headcode = Itcode + 1;
            _studentDB.Suppliers.Add(model);
            int rowsAffected = await _studentDB.SaveChangesAsync();

            // Optional: Use the rowsAffected value if needed
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Party

        public async Task<List<Party>> GetPartyAsync()
        {
            List<Party> Parties = new List<Party>();



            Parties = await _studentDB.Parties.ToListAsync();


            return Parties;
        }

        public async Task<bool> UpdatePartyAsync(Party model)
        {
            var existingItemHeader = await _studentDB.Parties.FindAsync(model.Headcode);

            if (existingItemHeader != null)
            {
                // Update the properties of the existing item header
                existingItemHeader.Subname = model.Subcode;
                existingItemHeader.Term = model.Term;
                existingItemHeader.Telno = model.Telno;
                existingItemHeader.City = model.City;


                // Update other properties as needed

                // Save changes to the database
                //_studentDB.Imhs.Update(model);
                int rowsAffected = await _studentDB.SaveChangesAsync();

                // Optional: Use the rowsAffected value if needed
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                // Handle the case when the item header is not found
                // You might throw an exception or return null
                // For example, we will throw an exception here
                return false;
            }
        }

        public async Task<bool> AddPartyAsync(Party model)
        {
            int Itcode = await _studentDB.Parties
                                .Select(c => (int?)c.Headcode) // Cast to nullable int to handle empty sequence
                                .MaxAsync() ?? 0;

            model.Headcode = Itcode + 1;
            _studentDB.Parties.Add(model);
            int rowsAffected = await _studentDB.SaveChangesAsync();

            // Optional: Use the rowsAffected value if needed
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }






        #endregion

        #region Head

        public async Task<bool> AddHeadAsync(Head model)
        {
            int Itcode = await _studentDB.Heads
                               .Select(c => (int?)c.HeaderId) // Cast to nullable int to handle empty sequence
                               .MaxAsync() ?? 0;

            if (Itcode == 0 || Itcode == null)
            {

                Itcode = 1000;


            }

            model.HeaderId = Itcode + 1;
            _studentDB.Heads.Add(model);
            int rowsAffected = await _studentDB.SaveChangesAsync();

            // Optional: Use the rowsAffected value if needed
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateHeadAsync(Head model)
        {
            var existingItemHeader = await _studentDB.Heads.FindAsync(model.HeaderId);

            if (existingItemHeader != null)
            {
                // Update the properties of the existing item header
                existingItemHeader.HeaderName = model.HeaderName;
                existingItemHeader.Opening = model.Opening;
                existingItemHeader.Type = model.Type;



                // Update other properties as needed

                // Save changes to the database
                //_studentDB.Imhs.Update(model);
                int rowsAffected = await _studentDB.SaveChangesAsync();

                // Optional: Use the rowsAffected value if needed
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                // Handle the case when the item header is not found
                // You might throw an exception or return null
                // For example, we will throw an exception here
                return false;
            }
        }

        public async Task<List<Head>> GetHeadListAsync()
        {
            List<Head> Head = new List<Head>();

            Head = await _studentDB.Heads.ToListAsync();

            return Head;
        }
        #endregion

        #region City

        public async Task<List<City>> GetCityListAsync()
        {

            List<City> City = new List<City>();

            City = await _studentDB.Cities.ToListAsync();

            return City;

        }

        #endregion


        #region Customer

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _studentDB.customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _studentDB.customers.FindAsync(id);
        }

        public async Task<int> AddCustomerAsync(Customer customer)
        {
            _studentDB.customers.Add(customer);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            _studentDB.customers.Update(customer);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            var customer = await _studentDB.customers.FindAsync(id);
            if (customer != null)
            {
                _studentDB.customers.Remove(customer);
                return await _studentDB.SaveChangesAsync();
            }
            return 0;
        }

        #endregion

        #region Item

        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _studentDB.Items.ToListAsync();
        }

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _studentDB.Items.FindAsync(id);
        }

        public async Task<int> AddItemAsync(Item item)
        {
            _studentDB.Items.Add(item);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            _studentDB.Items.Update(item);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> DeleteItemAsync(int id)
        {
            var item = await _studentDB.Items.FindAsync(id);
            if (item != null)
            {
                _studentDB.Items.Remove(item);
                return await _studentDB.SaveChangesAsync();
            }
            return 0;
        }

        #endregion

        #region Cate (Category)

        public async Task<List<Cate>> GetAllCatesAsync()
        {
            return await _studentDB.Cates.ToListAsync();
        }

        public async Task<Cate?> GetCateByIdAsync(int id)
        {
            return await _studentDB.Cates.FindAsync(id);
        }

        public async Task<int> AddCateAsync(Cate cate)
        {
            _studentDB.Cates.Add(cate);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> UpdateCateAsync(Cate cate)
        {
            _studentDB.Cates.Update(cate);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> DeleteCateAsync(int id)
        {
            var cate = await _studentDB.Cates.FindAsync(id);
            if (cate != null)
            {
                _studentDB.Cates.Remove(cate);
                return await _studentDB.SaveChangesAsync();
            }
            return 0;
        }

        #endregion

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _studentDB.Companies.ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _studentDB.Companies.FindAsync(id);
        }

        public async Task<int> AddCompanyAsync(Company company)
        {
            _studentDB.Companies.Add(company);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> UpdateCompanyAsync(Company company)
        {
            _studentDB.Companies.Update(company);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> DeleteCompanyAsync(int id)
        {
            var company = await _studentDB.Companies.FindAsync(id);
            if (company != null)
            {
                _studentDB.Companies.Remove(company);
                return await _studentDB.SaveChangesAsync();
            }
            return 0;
        }

    }
}