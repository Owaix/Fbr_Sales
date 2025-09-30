using EfPractice.Context;
using EfPractice.Models;
using EfPractice.Models.CustomerModel;
using EfPractice.Repository.Interface;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using EfPractice.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EfPractice.Repository.Class
{
    public class Master : IMaster
    {
        private readonly StudentContext _studentDB;
        private readonly string _fbrBaseUrl;
        private readonly string _bearerToken;
        private readonly FbrApiClient _fbrApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int? _companyId;
        public Master(StudentContext studentDB, FbrApiClient fbrApiClient, IHttpContextAccessor httpContextAccessor)
        {
            _studentDB = studentDB;
            _fbrApiClient = fbrApiClient;
            _httpContextAccessor = httpContextAccessor;
            _companyId = Convert.ToInt16(_httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId")?.Value);
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
            return await _studentDB.Imhs
                .AsNoTracking()
                .Where(x => x.CompanyId == _companyId)
                .ToListAsync();
        }



        public async Task<bool> UpdateItemHeaderAsync(Imh model)
        {


            var existingItemHeader = await _studentDB.Imhs.FirstOrDefaultAsync(x => x.Mid == model.Mid && x.CompanyId == _companyId);

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

            itemCatergory.ImhList = await _studentDB.Imhs
                .AsNoTracking()
                .Where(x => x.CompanyId == _companyId)
                .ToListAsync();


            return itemCatergory;



        }



        #region ItemCatergory


        public async Task<bool> ADDItemCatergoryRegistrarionAsync(Cate model)
        {
            int maxCid = await _studentDB.Cates
                                .Where(c => c.CompanyId == _companyId)
                                .Select(c => (int?)c.Cid) // Cast to nullable int to handle empty sequence
                                .MaxAsync() ?? 0;

            model.Cid = maxCid + 1;
            model.CompanyId = _companyId ?? 0;
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

            var existingItemHeader = await _studentDB.Cates.FirstOrDefaultAsync(x => x.Cid == model.Cid && x.CompanyId == _companyId);

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
            return await (from category in _studentDB.Cates.AsNoTracking().Where(c => c.CompanyId == _companyId)
                          join header in _studentDB.Imhs.AsNoTracking().Where(h => h.CompanyId == _companyId)
                          on category.Mid equals header.Mid
                          select new Cate
                          {
                              Cid = category.Cid,
                              Name = category.Name,
                              Mid = header.Mid,
                              Mn = header.Mname,
                              CompanyId = category.CompanyId
                          }).ToListAsync();
        }

        public async Task<ItemRegistrarion> GetCategoryListAsync()
        {
            var itemCategory = new ItemRegistrarion
            {
                CatergoryList = await _studentDB.Cates
                    .AsNoTracking()
                    .Where(c => c.CompanyId == _companyId)
                    .ToListAsync()
            };
            return itemCategory;
        }


        #endregion


        #region Item Registration


        public async Task<bool> AddItemRegistration(Item model)
        {
            model.CompanyId = _companyId ?? model.CompanyId;
            _studentDB.Items.Add(model);
            return await _studentDB.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateItemRegistration(Item model)
        {

            var existingItem = await _studentDB.Items.FirstOrDefaultAsync(x => x.Id == model.Id && x.CompanyId == _companyId);

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
            return await (from item in _studentDB.Items.AsNoTracking().Where(i => i.CompanyId == _companyId)
                          join cate in _studentDB.Cates.AsNoTracking().Where(c => c.CompanyId == _companyId)
                          on item.Category equals cate.Cid.ToString()
                          select new ItemRegistrarion
                          {
                              Ic = cate.Cid.ToString(),
                              IcName = cate.Name
                          }).ToListAsync();
        }

        #endregion


        #region supplier
        public async Task<List<Supplier>> GetSupplierAsync()
        {
            return await _studentDB.Suppliers.AsNoTracking().Where(s => s.CompanyId == _companyId).ToListAsync();
        }

        public async Task<bool> UpdateSupplierAsync(Supplier model)
        {
            var existingItemHeader = await _studentDB.Suppliers.FirstOrDefaultAsync(x => x.Headcode == model.Headcode && x.CompanyId == _companyId);

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
                                .Where(s => s.CompanyId == _companyId)
                                .Select(c => (int?)c.Headcode) // Cast to nullable int to handle empty sequence
                                .MaxAsync() ?? 0;

            model.Headcode = Itcode + 1;
            model.CompanyId = _companyId ?? 0;
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
            return await _studentDB.Parties.AsNoTracking().Where(p => p.CompanyId == _companyId).ToListAsync();
        }

        public async Task<bool> UpdatePartyAsync(Party model)
        {
            var existingItemHeader = await _studentDB.Parties.FirstOrDefaultAsync(x => x.Headcode == model.Headcode && x.CompanyId == _companyId);

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
                                .Where(p => p.CompanyId == _companyId)
                                .Select(c => (int?)c.Headcode) // Cast to nullable int to handle empty sequence
                                .MaxAsync() ?? 0;

            model.Headcode = Itcode + 1;
            model.CompanyId = _companyId ?? 0;
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
                               .Where(h => h.CompanyId == _companyId)
                               .Select(c => (int?)c.HeaderId) // Cast to nullable int to handle empty sequence
                               .MaxAsync() ?? 0;

            if (Itcode == 0 || Itcode == null)
            {

                Itcode = 1000;


            }

            model.HeaderId = Itcode + 1;
            model.CompanyId = _companyId ?? 0;
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
            var existingItemHeader = await _studentDB.Heads.FirstOrDefaultAsync(x => x.HeaderId == model.HeaderId && x.CompanyId == _companyId);

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
            return await _studentDB.Heads.AsNoTracking().Where(h => h.CompanyId == _companyId).ToListAsync();
        }
        #endregion

        #region City

        public async Task<List<City>> GetCityListAsync()
        {

            return await _studentDB.Cities.AsNoTracking().ToListAsync();

        }

        #endregion


        #region Customer

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _studentDB.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.CID == id && c.CompanyId == _companyId);
        }

        public async Task<Customer?> GetCustomerByNTNAsync(string ntn, int companyId)
        {
            return await _studentDB.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.NTN_No == ntn && c.CompanyId == companyId);
        }

        public async Task<int> AddCustomerAsync(Customer customer)
        {
            customer.CompanyId = _companyId ?? customer.CompanyId;
            _studentDB.Customers.Add(customer);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            var existing = await _studentDB.Customers.FirstOrDefaultAsync(c => c.CID == customer.CID && c.CompanyId == _companyId);
            if (existing == null) return 0;
            existing.CusName = customer.CusName;
            existing.NTN_No = customer.NTN_No;
            existing.Add = customer.Add;
            existing.City = customer.City;
            existing.InActive = customer.InActive;
            existing.Cell = customer.Cell;
            existing.MrNO = customer.MrNO;
            existing.RegistrationType = customer.RegistrationType;
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            var customer = await _studentDB.Customers.FirstOrDefaultAsync(c => c.CID == id && c.CompanyId == _companyId);
            if (customer == null) return 0;
            _studentDB.Customers.Remove(customer);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAllCustomersAsync(int companyId)
        {
            return await _studentDB.Customers.AsNoTracking().Where(c => c.CompanyId == companyId).ToListAsync();
        }

        #endregion

        #region Item


        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _studentDB.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id && i.CompanyId == _companyId);
        }

        public async Task<int> AddItemAsync(Item item)
        {
            item.CompanyId = _companyId ?? item.CompanyId;
            _studentDB.Items.Add(item);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> UpdateItemAsync(Item item)
        {
            var existing = await _studentDB.Items.FirstOrDefaultAsync(i => i.Id == item.Id && i.CompanyId == _companyId);
            if (existing == null) return 0;
            existing.ItemName = item.ItemName;
            existing.Description = item.Description;
            existing.HSCode = item.HSCode;
            existing.Rate = item.Rate;
            existing.UOM = item.UOM;
            existing.Category = item.Category;
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> DeleteItemAsync(int id)
        {
            var existing = await _studentDB.Items.FirstOrDefaultAsync(i => i.Id == id && i.CompanyId == _companyId);
            if (existing == null) return 0;
            _studentDB.Items.Remove(existing);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<List<Item>> GetAllItemsAsync()
        {
            return await _studentDB.Items.AsNoTracking().Where(i => i.CompanyId == _companyId).ToListAsync();
        }

        #endregion

        #region Cate (Category)

        public async Task<Cate?> GetCateByIdAsync(int id)
        {
            return await _studentDB.Cates.AsNoTracking().FirstOrDefaultAsync(c => c.Cid == id && c.CompanyId == _companyId);
        }

        public async Task<int> AddCateAsync(Cate cate)
        {
            cate.CompanyId = _companyId ?? cate.CompanyId;
            _studentDB.Cates.Add(cate);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> UpdateCateAsync(Cate cate)
        {
            var existing = await _studentDB.Cates.FirstOrDefaultAsync(c => c.Cid == cate.Cid && c.CompanyId == _companyId);
            if (existing == null) return 0;
            existing.Name = cate.Name;
            existing.Mid = cate.Mid;
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> DeleteCateAsync(int id)
        {
            var existing = await _studentDB.Cates.FirstOrDefaultAsync(c => c.Cid == id && c.CompanyId == _companyId);
            if (existing == null) return 0;
            _studentDB.Cates.Remove(existing);
            return await _studentDB.SaveChangesAsync();
        }

        #endregion

        #region Company
        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _studentDB.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
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
            if (company == null) return 0;
            _studentDB.Companies.Remove(company);
            return await _studentDB.SaveChangesAsync();
        }
        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            if (_httpContextAccessor.HttpContext.User.HasClaim("CompanyId", "0"))
                return await _studentDB.Companies.AsNoTracking().ToListAsync();
            else
                return await _studentDB.Companies.AsNoTracking().Where(x => x.Id == _companyId).ToListAsync();
        }
        public async Task<List<Company>> GetCompanyAsync(Company filter)
        {
            IQueryable<Company> query = _studentDB.Companies.AsNoTracking();
            if (!string.IsNullOrEmpty(filter.Email)) query = query.Where(c => c.Email == filter.Email);
            if (!string.IsNullOrEmpty(filter.UserName)) query = query.Where(c => c.UserName == filter.UserName);
            if (!string.IsNullOrEmpty(filter.BusinessName)) query = query.Where(c => c.BusinessName.Contains(filter.BusinessName));
            return await query.ToListAsync();
        }
        #endregion

        #region SaleInvoice
        public async Task<SaleInvoice> GetSaleInvoiceByIdAsync(int id)
        {
            return await _studentDB.SaleInvoices
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == _companyId);
        }

        public async Task<int> AddSaleInvoiceAsync(SaleInvoice invoice)
        {
            invoice.CompanyId = _companyId ?? invoice.CompanyId;
            _studentDB.SaleInvoices.Add(invoice);
            return await _studentDB.SaveChangesAsync();
        }

        public async Task<int> UpdateSaleInvoiceAsync(SaleInvoice invoice)
        {
            var existing = await _studentDB.SaleInvoices.FirstOrDefaultAsync(s => s.Id == invoice.Id && s.CompanyId == _companyId);
            if (existing == null) return 0;
            _studentDB.Entry(existing).CurrentValues.SetValues(invoice);
            return await _studentDB.SaveChangesAsync();
        }
        public async Task<HttpResponseMessage> SendInvoiceToFbrAsync(SaleInvoice invoice)
        {
            var json = JsonConvert.SerializeObject(invoice);
            return await _fbrApiClient.PostAsync("postinvoicedata_sb", json);
        }

        public async Task<int> AddSaleInvoiceDetailAsync(List<SaleInvoiceItem> invoice)
        {
            _studentDB.SaleInvoiceItems.AddRange(invoice);
            return await _studentDB.SaveChangesAsync();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _studentDB.Database.BeginTransactionAsync();
        }

        public async Task<SaleInvoice?> GetSaleInvoiceByNumberAsync(string invoiceNo)
        {
            return await _studentDB.SaleInvoices
                        .AsNoTracking()
                        .Include(s => s.Items)
                        .FirstOrDefaultAsync(x => x.invoiceNumber == invoiceNo && x.CompanyId == _companyId);
        }
        public async Task<List<SaleInvoice>> GetSaleInvoices()
        {
            return await _studentDB.SaleInvoices
                .AsNoTracking()
                .Include(s => s.Items)
                .Where(s => s.CompanyId == _companyId)
                .OrderByDescending(s => s.InvoiceDate)
                .ThenByDescending(s => s.Id)
                .ToListAsync();
        }
        #endregion

        #region User Management

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _studentDB.Users.AsNoTracking().Where(u => u.CompanyId == _companyId).ToListAsync();
        }

        public async Task AddUserAsync(UserEditViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                CompanyId = _companyId ?? 0,
                UserRoleId = model.RoleName == "Admin" ? 1 : model.RoleName == "Manager" ? 2 : model.RoleName == "Staff" ? 3 : 0
            };
            var userManager = (UserManager<ApplicationUser>)_httpContextAccessor.HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>));
            await userManager.CreateAsync(user, model.Password ?? "123456");
        }

        public async Task UpdateUserAsync(UserEditViewModel model)
        {
            var userManager = (UserManager<ApplicationUser>)_httpContextAccessor.HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>));
            var user = await _studentDB.Users.FirstOrDefaultAsync(u => u.Id == model.Id && u.CompanyId == _companyId);
            if (user == null) return;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.UserRoleId = model.RoleName == "Admin" ? 1 : model.RoleName == "Manager" ? 2 : model.RoleName == "Staff" ? 3 : 0;
            await userManager.UpdateAsync(user);
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                await userManager.ResetPasswordAsync(user, token, model.Password);
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var userManager = (UserManager<ApplicationUser>)_httpContextAccessor.HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>));
            var user = await _studentDB.Users.FirstOrDefaultAsync(u => u.Id == id && u.CompanyId == _companyId);
            if (user != null)
                await userManager.DeleteAsync(user);
        }
        #endregion
    }
}