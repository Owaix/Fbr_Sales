using EfPractice.Areas.Identity.Data;
using EfPractice.Models;
using EfPractice.Repository.Class;
using EfPractice.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EfPractice.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IMaster _master;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IMaster master, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
        {
            _master = master;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Main()
        {

            //  var stdRecord = studentDB.Students.ToList();

            return View(/*stdRecord*/);

        }
        public IActionResult Index()
        {

            //  var stdRecord = studentDB.Students.ToList();

            return View(/*stdRecord*/);

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
                customer.CompanyID = CompanyId ?? 0;
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

            var items = await _master.GetAllItemsAsync(CompanyId ?? 0);
            model.Items = items;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Items(Item item)
        {
            ItemsViewModel model = new ItemsViewModel();
            model.Items = await _master.GetAllItemsAsync(CompanyId ?? 0);
            if (ModelState.IsValid)
            {
                item.CompanyID = CompanyId ?? 0;
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

            if (ModelState.IsValid)
            {
                company.Id = CompanyId ?? 0; // If you want to restrict update to current company only
                if (company.Id > 0)
                    await _master.UpdateCompanyAsync(company);
                else
                    await _master.AddCompanyAsync(company);

                // Register user in Identity
                var user = new ApplicationUser
                {
                    UserName = company.UserName,
                    Email = company.UserName,
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
            switch (id)
            {
                case 1: return "Punjab";
                case 2: return "Sindh";
                case 3: return "KPK";
                case 4: return "Balochistan";
                default: return "Unknown";
            }
        }

        // Helper to map role string to int
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
        public async Task<IActionResult> SInv(int? id)
        {
            // Load invoice for edit or create new
            SaleInvoiceViewModel model = new SaleInvoiceViewModel();
            if (id.HasValue)
            {
                model.Invoice = await _master.GetSaleInvoiceByIdAsync(id.Value);
            }
            else
            {
                model.Invoice = new SaleInvoice { InvoiceDate = DateTime.Now, Items = new List<SaleInvoiceItem>() };
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SInv(SaleInvoiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Invoice.Id > 0)
                    await _master.UpdateSaleInvoiceAsync(model.Invoice);
                else
                    await _master.AddSaleInvoiceAsync(model.Invoice);

                // Call FBR API
                await _master.SendInvoiceToFbrAsync(model.Invoice);

                return RedirectToAction("SInv");
            }
            return View(model);
        }
    }
}
