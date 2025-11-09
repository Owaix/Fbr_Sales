using EfPractice.Areas.Identity.Data;
using EfPractice.Models;
using EfPractice.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // added for SelectListItem
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;

namespace EfPractice.Controllers
{
    public partial class HomeController : BaseController
    {
        private readonly IMaster _master;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IMaster master, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _master = master;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Main()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        // REWRITTEN CUSTOMER ACTIONS FOR REDESIGNED CRUD
        public async Task<IActionResult> Customer(int id = 0)
        {
            var vm = new CustomerViewModel();
            if (id > 0)
                vm.Customer = await _master.GetCustomerByIdAsync(id) ?? new Customer();

            vm.Customers = await _master.GetAllCustomersAsync(CompanyId ?? 0);
            vm.Customers.ForEach(c => c.Prn = GetProvinceName(c.City ?? 0));

            // Static dropdown data (replace with DB sourced lists if available)
            vm.CustomerTypes = new List<SelectListItem> { new("Retail", "Retail"), new("Wholesale", "Wholesale"), new("Corporate", "Corporate") };
            vm.BusinessSectors = new List<SelectListItem> { new("IT", "IT"), new("Finance", "Finance"), new("Manufacturing", "Manufacturing"), new("Agriculture", "Agriculture") };
            vm.Countries = new List<SelectListItem> { new("Pakistan", "Pakistan"), new("USA", "USA"), new("UK", "UK") };
            vm.States = new List<SelectListItem> { new("Punjab", "Punjab"), new("Sindh", "Sindh"), new("KPK", "KPK"), new("Balochistan", "Balochistan") };
            vm.Cities = new List<SelectListItem> { new("Lahore", "Lahore"), new("Karachi", "Karachi"), new("Islamabad", "Islamabad"), new("Peshawar", "Peshawar") };
            vm.ProductPriceLevels = new List<SelectListItem> { new("Standard", "Standard"), new("Premium", "Premium"), new("Discount", "Discount") };
            vm.AccountManagers = new List<SelectListItem> { new("Ali", "Ali"), new("Ahmed", "Ahmed"), new("Sara", "Sara") };
            vm.Areas = new List<SelectListItem> { new("North", "North"), new("South", "South"), new("East", "East"), new("West", "West") };
            vm.CreditTermsOptions = new List<SelectListItem> { new("Cash", "Cash"), new("Net 30", "Net 30"), new("Net 60", "Net 60") };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Customer(Customer customer)
        {
            try
            {
                var model = new CustomerViewModel
                {
                    Customers = await _master.GetAllCustomersAsync(CompanyId ?? 0),
                    Customer = customer,
                };

                model.Customers.ForEach(c => c.Prn = GetProvinceName(c.City ?? 0));
                model.CustomerTypes = new List<SelectListItem> { new("Retail", "Retail"), new("Wholesale", "Wholesale"), new("Corporate", "Corporate") };
                model.BusinessSectors = new List<SelectListItem> { new("IT", "IT"), new("Finance", "Finance"), new("Manufacturing", "Manufacturing"), new("Agriculture", "Agriculture") };
                model.Countries = new List<SelectListItem> { new("Pakistan", "Pakistan"), new("USA", "USA"), new("UK", "UK") };
                model.States = new List<SelectListItem> { new("Punjab", "Punjab"), new("Sindh", "Sindh"), new("KPK", "KPK"), new("Balochistan", "Balochistan") };
                model.Cities = new List<SelectListItem> { new("Lahore", "Lahore"), new("Karachi", "Karachi"), new("Islamabad", "Islamabad"), new("Peshawar", "Peshawar") };
                model.ProductPriceLevels = new List<SelectListItem> { new("Standard", "Standard"), new("Premium", "Premium"), new("Discount", "Discount") };
                model.AccountManagers = new List<SelectListItem> { new("Ali", "Ali"), new("Ahmed", "Ahmed"), new("Sara", "Sara") };
                model.Areas = new List<SelectListItem> { new("North", "North"), new("South", "South"), new("East", "East"), new("West", "West") };
                model.CreditTermsOptions = new List<SelectListItem> { new("Cash", "Cash"), new("Net 30", "Net 30"), new("Net 60", "Net 60") };

                if (ModelState.IsValid)
                {
                    customer.CompanyId = CompanyId ?? 0;
                    if (customer.CustomerID > 0)
                        await _master.UpdateCustomerAsync(customer);
                    else
                        await _master.AddCustomerAsync(customer);
                    return RedirectToAction(nameof(Customer)); // Redirect after save
                }
                return View(model); // Show validation errors
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    Console.WriteLine($"Entity: {entry.Entity.GetType().Name}");
                    Console.WriteLine($"State: {entry.State}");
                }
                throw; // rethrow after inspecting
            }
        }

        public async Task<IActionResult> CustomerDelete(int id)
        {
            await _master.DeleteCustomerAsync(id);
            return RedirectToAction("Customer", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetTaxRate(int id)
        {
            var tax = await _master.GetTaxByIdAsync(id);
            if (tax == null) return NotFound();
            return Json(new
            {
                rate = tax.DefaultRate,
                type = tax.TaxType
            });
        }

        [HttpGet]
        public async Task<IActionResult> SubCategory(int id)
        {
            var tax = await _master.GetSubCategoriesByCategoryAsync(id);
            if (tax == null) return NotFound();
            return Json(
                tax.Select(sc => new
                {
                    id = sc.Id,
                    name = sc.Name
                }));
        }

        public async Task<IActionResult> Items(int id = 0)
        {
            ItemsViewModel model = new ItemsViewModel();
            if (id > 0)
                model.Item = await _master.GetItemByIdAsync(id);

            var items = await _master.GetAllItemsAsync();
            model.Items = items;
            model.Taxes = (await _master.GetTaxesAsync(CompanyId ?? 0))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList();
            model.Brands = (await _master.GetBrandsAsync(CompanyId ?? 0)).Select(b => new SelectListItem { Value = b.Name, Text = b.Name }).ToList();
            model.Categories = (await _master.GETItemCatergoryRegistrarionAsync())
                .Select(c => new SelectListItem { Value = c.Cid.ToString(), Text = c.Name }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Items(Item item)
        {
            ItemsViewModel model = new ItemsViewModel();
            model.Items = await _master.GetAllItemsAsync();
            model.Item = item; // This ensures validation errors and user input are shown
            model.Taxes = (await _master.GetTaxesAsync(CompanyId ?? 0))
              .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList();
            model.Brands = (await _master.GetBrandsAsync(CompanyId ?? 0)).Select(b => new SelectListItem { Value = b.Name, Text = b.Name }).ToList();
            model.Categories = (await _master.GETItemCatergoryRegistrarionAsync())
                .Select(c => new SelectListItem { Value = c.Cid.ToString(), Text = c.Name }).ToList();

            if (ModelState.IsValid)
            {
                item.CompanyId = CompanyId ?? 0;
                if (item.Id > 0)
                    await _master.UpdateItemAsync(item);
                else
                    await _master.AddItemAsync(item);
                return RedirectToAction(nameof(Items)); // Redirect after save
            }
            return View(model); // Show validation errors
        }

        public async Task<IActionResult> ItemsDelete(int id)
        {
            await _master.DeleteItemAsync(id);
            return RedirectToAction("Items", "Home");
        }

        public async Task<IActionResult> Company(int id = 0)
        {
            CompanyViewModel model = new CompanyViewModel();
            if (id > 0)
                model.Company = await _master.GetCompanyByIdAsync(id);

            var companies = await _master.GetAllCompaniesAsync();
            model.Companies = companies;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Company(Company company)
        {
            CompanyViewModel model = new CompanyViewModel();

            var companies = await _master.GetCompanyAsync(new Company
            {
                BusinessName = company.BusinessName,
            });

            if (company.Password != string.Empty)
                ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                if (company.Id > 0)
                    await _master.UpdateCompanyAsync(company);
                else
                    await _master.AddCompanyAsync(company);

                var user = new ApplicationUser
                {
                    UserName = company.UserName,
                    Email = company.Email,
                    CompanyId = company.Id,
                    UserRoleId = Convert.ToInt16(company.UserRole)
                };
                var result = await _userManager.CreateAsync(user, company.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            model.Companies = await _master.GetAllCompaniesAsync();
            return View(model);
        }

        public async Task<IActionResult> CompanyDelete(int id)
        {
            await _master.DeleteCompanyAsync(id);
            return RedirectToAction("Company", "Home");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public string GetProvinceName(int id)
        {
            return id switch
            {
                1 => "Punjab",
                2 => "Sindh",
                3 => "KPK",
                4 => "Balochistan",
                _ => "Unknown"
            };
        }

        private int GetUserRoleId(string userRole)
        {
            return userRole switch
            {
                "Admin" => 1,
                "User" => 2,
                _ => 0
            };
        }

        [HttpGet]
        public async Task<IActionResult> SInv(int? id, string? invoiceNo)
        {
            SaleInvoice model = new SaleInvoice();

            if (!string.IsNullOrEmpty(invoiceNo))
            {
                model = await _master.GetSaleInvoiceByNumberAsync(invoiceNo);
                if (model == null)
                    model = new SaleInvoice { InvoiceDate = DateTime.Now, Items = new List<SaleInvoiceItem>() };
            }
            else if (id.HasValue)
            {
                model = await _master.GetSaleInvoiceByIdAsync(id.Value);
            }
            else
            {
                model = new SaleInvoice { InvoiceDate = DateTime.Now, Items = new List<SaleInvoiceItem>() };
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SInv(SaleInvoice model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                if (model.Id == 0)
                {
                    var resp = await _master.SendInvoiceToFbrAsync(model);
                    var content = await resp.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var fbrResponse = JsonSerializer.Deserialize<FbrResponse>(content, options);

                    if (fbrResponse?.validationResponse?.statusCode == "01")
                    {
                        model.invoiceNumber = fbrResponse.invoiceNumber;
                        model.dated = Convert.ToDateTime(fbrResponse.dated);
                        var id = await _master.AddSaleInvoiceAsync(model);
                        TempData["Message"] = $"Invoice sent successfully. FBR Invoice#: {fbrResponse.invoiceNumber}";
                        return RedirectToAction("PrintInvoice", new { id });
                    }
                    else
                    {
                        FBRErrorCodes fBRErrorCodes = new FBRErrorCodes();
                        var Error = fBRErrorCodes.GetErrorByCode(fbrResponse?.validationResponse?.errorCode);
                        ModelState.AddModelError(string.Empty, Error ?? "Unknown FBR error");
                    }
                }
                else
                {
                    await _master.UpdateSaleInvoiceAsync(model);
                    return RedirectToAction("PrintInvoice", new { id = model.Id });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error: " + ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SInvList(string? invoiceNo, string? buyer, DateTime? from, DateTime? to)
        {
            var invoices = await _master.GetSaleInvoices();

            if (!string.IsNullOrWhiteSpace(invoiceNo))
                invoices = invoices.Where(i => (i.invoiceNumber ?? "").Contains(invoiceNo, StringComparison.OrdinalIgnoreCase)
                                             || (i.InvoiceRefNo ?? "").Contains(invoiceNo, StringComparison.OrdinalIgnoreCase))
                               .ToList();
            if (!string.IsNullOrWhiteSpace(buyer))
                invoices = invoices.Where(i => (i.BuyerBusinessName ?? "").Contains(buyer, StringComparison.OrdinalIgnoreCase)).ToList();
            if (from.HasValue)
                invoices = invoices.Where(i => i.InvoiceDate.Date >= from.Value.Date).ToList();
            if (to.HasValue)
                invoices = invoices.Where(i => i.InvoiceDate.Date <= to.Value.Date).ToList();

            return View(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            return RedirectToAction(nameof(SInvList));
        }

        [HttpGet]
        public async Task<IActionResult> PrintInvoice(int id)
        {
            var inv = await _master.GetSaleInvoiceByIdAsync(id);
            if (inv == null) return NotFound();
            Company? comp = null;
            try
            {
                comp = await _master.GetCompanyByIdAsync(inv.CompanyId);
            }
            catch { }
            var vm = new PrintInvoiceViewModel
            {
                Invoice = inv,
                Company = comp
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> CustomerSearch(string term)
        {
            var customers = await _master.GetAllCustomersAsync(CompanyId ?? 0);
            if (!string.IsNullOrWhiteSpace(term))
            {
                term = term.ToLower();
                customers = customers.Where(c => c.CompanyId == CompanyId &&
                (c.CusName ?? string.Empty).ToLower().Contains(term) || (c.NTN_No ?? string.Empty).ToLower().Contains(term))
                                     .Take(50)
                                     .ToList();
            }
            var result = customers.Select(c => new
            {
                id = c.CusName, // will bind directly to BuyerBusinessName
                text = c.CusName,
                ntn = c.NTN_No,
                address = c.Add,
                province = c.City switch
                {
                    1 => "Punjab",
                    2 => "Sindh",
                    3 => "KPK",
                    4 => "Balochistan",
                    _ => string.Empty
                },
                regType = c.RegistrationType
            });
            return Json(result);
        }

        public async Task<IActionResult> Users(string id = "0")
        {
            UserListEditViewModel vm = new UserListEditViewModel();
            if (id != "0")
            {
                var user = await _userManager.FindByIdAsync(id);
                vm.User.Email = user.Email;
                vm.User.Id = user.Id;
                vm.User.UserName = user.UserName;
                //vm.User.i = user.UserRoleId;
            }
            var cid = CompanyId ?? 0;
            vm.Users = _userManager.Users.Where(u => u.CompanyId == cid).ToList();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Users(UserListEditViewModel vm)
        {
            var cid = CompanyId ?? 0;
            if (!ModelState.IsValid)
            {
                vm.Users = _userManager.Users.Where(u => u.CompanyId == cid).ToList();
                ViewData["ShowValidationSummary"] = true;
                return View("Users", vm);
            }

            int companyId = CompanyId ?? 0;
            var input = vm.User; // shorthand

            ApplicationUser user;

            // ---- Update existing user ----
            if (!string.IsNullOrEmpty(input.Id))
            {
                user = await _userManager.FindByIdAsync(input.Id);

                // --- Duplicate checks ---
                var existingUserName = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == input.Email && u.CompanyId == cid && u.Id != input.Id);
                if (existingUserName != null)
                {
                    ModelState.AddModelError("User.Email", "Email is already taken.");
                    vm.Users = await _userManager.Users.ToListAsync();
                    ViewData["ShowValidationSummary"] = true;
                    return View("Users", vm);
                }

                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    vm.Users = _userManager.Users.Where(u => u.CompanyId == cid).ToList();
                    return View("Users", vm);
                }

                user.Email = input.Email;
                user.UserName = input.UserName;
                user.UserRoleId = GetUserRoleId(input.RoleName);

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var e in updateResult.Errors)
                        ModelState.AddModelError(string.Empty, e.Description);

                    vm.Users = _userManager.Users.Where(u => u.CompanyId == cid).ToList();
                    ViewData["ShowValidationSummary"] = true;
                    return View("Users", vm);
                }

                // If password entered, reset it
                if (!string.IsNullOrWhiteSpace(input.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passResult = await _userManager.ResetPasswordAsync(user, token, input.Password);
                    if (!passResult.Succeeded)
                    {
                        foreach (var e in passResult.Errors)
                            ModelState.AddModelError(string.Empty, e.Description);

                        vm.Users = _userManager.Users.Where(u => u.CompanyId == cid).ToList();
                        ViewData["ShowValidationSummary"] = true;
                        return View("Users", vm);
                    }
                }
            }
            // ---- Add new user ----
            else
            {
                user = new ApplicationUser
                {
                    UserName = input.UserName,
                    Email = input.Email,
                    CompanyId = companyId,
                    UserRoleId = GetUserRoleId(input.RoleName ?? "User")
                };

                var createResult = await _userManager.CreateAsync(user, input.Password);
                if (!createResult.Succeeded)
                {
                    foreach (var e in createResult.Errors)
                        ModelState.AddModelError(string.Empty, e.Description);

                    vm.Users = _userManager.Users.Where(u => u.CompanyId == cid).ToList();
                    ViewData["ShowValidationSummary"] = true;
                    return View("Users", vm);
                }
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.CompanyId != (CompanyId ?? 0)) return NotFound();
            var res = await _userManager.DeleteAsync(user);
            if (!res.Succeeded)
            {
                TempData["UserError"] = string.Join("; ", res.Errors.Select(e => e.Description));
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.CompanyId != (CompanyId ?? 0)) return NotFound();
            return View("ResetPassword", new ResetPasswordViewModel { UserId = id });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid) return View("ResetPassword", vm);
            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null || user.CompanyId != (CompanyId ?? 0)) return NotFound();
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var res = await _userManager.ResetPasswordAsync(user, token, vm.NewPassword);
            if (!res.Succeeded)
            {
                foreach (var e in res.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return View("ResetPassword", vm);
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> Taxes(int id = 0)
        {
            var vm = new TaxViewModel
            {
                Taxes = await _master.GetTaxesAsync(CompanyId ?? 0),
                Tax = id > 0 ? await _master.GetTaxByIdAsync(id) ?? new Tax() : new Tax(),
                Accounts = await _master.GetAccountsAsync(CompanyId ?? 0)
            };
            vm.Accounts = vm.Accounts.Select(Accounts =>
            {
                Accounts.AccountTitle = Accounts.AccountId + " - " + Accounts.AccountTitle;
                return Accounts;
            }).ToList();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Taxes(Tax tax)
        {
            if (ModelState.IsValid)
            {
                if (tax.Id == 0)
                    await _master.AddTaxAsync(tax);
                else
                    await _master.UpdateTaxAsync(tax);
                return RedirectToAction(nameof(Taxes));
            }
            var vm = new TaxViewModel
            {
                Tax = tax,
                Taxes = await _master.GetTaxesAsync(CompanyId ?? 0),
                Accounts = await _master.GetAccountsAsync(CompanyId ?? 0)
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTax(int id)
        {
            await _master.DeleteTaxAsync(id);
            return RedirectToAction(nameof(Taxes));
        }

        [HttpGet]
        public async Task<IActionResult> Accounts(int id = 0)
        {
            var vm = new AccountViewModel
            {
                Accounts = await _master.GetAccountsAsync(CompanyId ?? 0),
                Account = id > 0 ? await _master.GetAccountByIdAsync(id) ?? new Account() : new Account()
            }; return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Accounts(Account account)
        {
            if (ModelState.IsValid)
            {
                if (account.Id == 0)
                    await _master.AddAccountAsync(account);
                else
                    await _master.UpdateAccountAsync(account);
                return RedirectToAction(nameof(Accounts));
            }
            var vm = new AccountViewModel
            {
                Account = account,
                Accounts = await _master.GetAccountsAsync(CompanyId ?? 0)
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            await _master.DeleteAccountAsync(id);
            return RedirectToAction(nameof(Accounts));
        }

        [HttpGet]
        public async Task<IActionResult> Categories(int id = 0)
        {
            var vm = new CategoryViewModel
            {
                Category = id > 0 ? await _master.GetCateByIdAsync(id) ?? new Cate() : new Cate(),
                Categories = await _master.GETItemCatergoryRegistrarionAsync()
            }; return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Categories(Cate category)
        {
            if (ModelState.IsValid)
            {
                if (category.Cid > 0)
                    await _master.UpdateCateAsync(category);
                else
                    await _master.AddCateAsync(category);
                return RedirectToAction(nameof(Categories));
            }
            var vm = new CategoryViewModel
            {
                Category = category,
                Categories = await _master.GETItemCatergoryRegistrarionAsync()
            }; return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _master.DeleteCateAsync(id); return RedirectToAction(nameof(Categories));
        }
        [HttpGet]
        public async Task<IActionResult> SubCategories(int categoryId = 0, int id = 0)
        {
            var vm = new SubCategoryViewModel();
            vm.Categories = (await _master.GETItemCatergoryRegistrarionAsync()).Select(c => new SelectListItem(c.Name, c.Cid.ToString())).ToList();
            vm.SelectedCategoryId = categoryId;
            vm.SubCategories = await _master.GetSubCategoriesByCategoryAsync(vm.SelectedCategoryId);

            if (id > 0)
            {
                vm.SubCategory = await _master.GetSubCategoryByIdAsync(id);
            }

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubCategories(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                if (subCategory.Id > 0)
                    await _master.UpdateSubCategoryAsync(subCategory);
                else await _master.AddSubCategoryAsync(subCategory);
                return RedirectToAction(nameof(SubCategories), new
                {
                    categoryId = subCategory.CategoryId
                });
            }
            var vm = new SubCategoryViewModel
            {
                SubCategory = subCategory,
                Categories = (await _master.GETItemCatergoryRegistrarionAsync()).Select(c => new SelectListItem(c.Name, c.Cid.ToString())).ToList(),
                SelectedCategoryId = subCategory.CategoryId,
                SubCategories = subCategory.CategoryId > 0 ? await _master.GetSubCategoriesByCategoryAsync(subCategory.CategoryId) : new List<SubCategory>()
            }; return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubCategory(int id, int categoryId)
        {
            await _master.DeleteSubCategoryAsync(id);
            return RedirectToAction(nameof(SubCategories), new
            {
                categoryId
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            var subs = await _master.GetSubCategoriesByCategoryAsync(categoryId);
            return Json(subs.Select(s => new { id = s.Id, name = s.Name }));
        }

        [HttpGet] public async Task<IActionResult> Brands(int id = 0) { var vm = new BrandViewModel { Brand = id > 0 ? await _master.GetBrandByIdAsync(id) ?? new Brand() : new Brand(), Brands = await _master.GetBrandsAsync(CompanyId ?? 0) }; return View(vm); }
        [HttpPost][ValidateAntiForgeryToken] public async Task<IActionResult> Brands(Brand brand) { if (ModelState.IsValid) { if (brand.Id > 0) await _master.UpdateBrandAsync(brand); else await _master.AddBrandAsync(brand); return RedirectToAction(nameof(Brands)); } var vm = new BrandViewModel { Brand = brand, Brands = await _master.GetBrandsAsync(CompanyId ?? 0) }; return View(vm); }
        [HttpPost][ValidateAntiForgeryToken] public async Task<IActionResult> DeleteBrand(int id) { await _master.DeleteBrandAsync(id); return RedirectToAction(nameof(Brands)); }

    }
}
