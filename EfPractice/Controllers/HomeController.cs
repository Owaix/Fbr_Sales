using EfPractice.Areas.Identity.Data;
using EfPractice.Models;
using EfPractice.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace EfPractice.Controllers
{
    public class HomeController : BaseController
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

        public async Task<IActionResult> Customer(int id = 0)
        {
            CustomerViewModel model = new CustomerViewModel();
            if (id > 0)
                model.Customer = await _master.GetCustomerByIdAsync(id);

            var customers = await _master.GetAllCustomersAsync(CompanyId ?? 0);
            customers.ForEach(c => c.Prn = GetProvinceName(c.City ?? 0));
            model.Customers = customers;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Customer(Customer customer)
        {
            CustomerViewModel model = new CustomerViewModel();
            if (ModelState.IsValid)
            {
                customer.CompanyId = CompanyId ?? 0;
                if (customer.CID > 0)
                    await _master.UpdateCustomerAsync(customer);
                else
                    await _master.AddCustomerAsync(customer);
            }
            model.Customers = await _master.GetAllCustomersAsync(CompanyId ?? 0);
            model.Customers.ForEach(c => c.Prn = GetProvinceName(c.City ?? 0));
            return View(model);
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
                    UserRoleId = GetUserRoleId(company.UserRole)
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

    }
}
