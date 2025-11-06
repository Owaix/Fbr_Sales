using EfPractice.Areas.Identity.Data;
using EfPractice.Models;
using EfPractice.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // added for SelectListItem
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
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
                if (!ModelState.IsValid)
                {
                    return View(customer);
                }

                customer.CompanyId = CompanyId ?? 0;
                if (customer.CustomerID > 0)
                    await _master.UpdateCustomerAsync(customer);
                else
                    await _master.AddCustomerAsync(customer);
                // After successful save reset the form model
                customer = new Customer();
                TempData["Success"] = "Customer saved successfully.";

                var vm = new CustomerViewModel
                {
                    Customers = await _master.GetAllCustomersAsync(CompanyId ?? 0),
                    Customer = customer
                };
                vm.Customers.ForEach(c => c.Prn = GetProvinceName(c.City ?? 0));
                vm.CustomerTypes = new List<SelectListItem> { new("Retail", "Retail"), new("Wholesale", "Wholesale"), new("Corporate", "Corporate") };
                vm.BusinessSectors = new List<SelectListItem> { new("IT", "IT"), new("Finance", "Finance"), new("Manufacturing", "Manufacturing"), new("Agriculture", "Agriculture") };
                vm.Countries = new List<SelectListItem> { new("Pakistan", "Pakistan"), new("USA", "USA"), new("UK", "UK") };
                vm.States = new List<SelectListItem> { new("Punjab", "Punjab"), new("Sindh", "Sindh"), new("KPK", "KPK"), new("Balochistan", "Balochistan") };
                vm.Cities = new List<SelectListItem> { new("Lahore", "Lahore"), new("Karachi", "Karachi"), new("Islamabad", "Islamabad"), new("Peshawar", "Peshawar") };
                vm.ProductPriceLevels = new List<SelectListItem> { new("Standard", "Standard"), new("Premium", "Premium"), new("Discount", "Discount") };
                vm.AccountManagers = new List<SelectListItem> { new("Ali", "Ali"), new("Ahmed", "Ahmed"), new("Sara", "Sara") };
                vm.Areas = new List<SelectListItem> { new("North", "North"), new("South", "South"), new("East", "East"), new("West", "West") };
                vm.CreditTermsOptions = new List<SelectListItem> { new("Cash", "Cash"), new("Net 30", "Net 30"), new("Net 60", "Net 60") };
                return RedirectToAction("Customer");

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

        public async Task<IActionResult> Items(int id = 0)
        {
            ItemsViewModel model = new ItemsViewModel();
            if (id > 0)
                model.Item = await _master.GetItemByIdAsync(id);

            var items = await _master.GetAllItemsAsync();
            model.Items = items;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Items(Item item)
        {
            ItemsViewModel model = new ItemsViewModel();
            model.Items = await _master.GetAllItemsAsync();
            if (ModelState.IsValid)
            {
                item.CompanyId = CompanyId ?? 0;
                if (item.Id > 0)
                    await _master.UpdateItemAsync(item);
                else
                    await _master.AddItemAsync(item);
            }
            return View(model);
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
                "Manager" => 2,
                "Staff" => 3,
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

        public async Task<IActionResult> Users()
        {
            var cid = CompanyId ?? 0;
            var users = _userManager.Users.Where(u => u.CompanyId == cid).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View("UserEdit", new UserEditViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserEditViewModel vm)
        {
            if (!ModelState.IsValid) return View("UserEdit", vm);
            if (string.IsNullOrWhiteSpace(vm.Password))
            {
                ModelState.AddModelError("Password", "Password required");
                return View("UserEdit", vm);
            }
            var user = new ApplicationUser
            {
                UserName = vm.UserName,
                Email = vm.Email,
                CompanyId = CompanyId ?? 0,
                // Force normal User role for users created from user screen
                UserRoleId = GetUserRoleId("User")
            };
            var result = await _userManager.CreateAsync(user, vm.Password!);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return View("UserEdit", vm);
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.CompanyId != (CompanyId ?? 0)) return NotFound();
            var vm = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                UserName = user.UserName ?? string.Empty,
                RoleName = user.UserRoleId switch { 1 => "Admin", 2 => "Manager", 3 => "Staff", _ => "User" }
            };
            return View("UserEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditViewModel vm)
        {
            if (!ModelState.IsValid) return View("UserEdit", vm);
            if (vm.Id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null || user.CompanyId != (CompanyId ?? 0)) return NotFound();
            user.Email = vm.Email;
            user.UserName = vm.UserName;
            user.UserRoleId = GetUserRoleId(vm.RoleName);
            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
            {
                foreach (var e in update.Errors) ModelState.AddModelError(string.Empty, e.Description);
                return View("UserEdit", vm);
            }
            if (!string.IsNullOrWhiteSpace(vm.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passRes = await _userManager.ResetPasswordAsync(user, token, vm.Password);
                if (!passRes.Succeeded)
                {
                    foreach (var e in passRes.Errors) ModelState.AddModelError(string.Empty, e.Description);
                    return View("UserEdit", vm);
                }
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
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

    }
}
