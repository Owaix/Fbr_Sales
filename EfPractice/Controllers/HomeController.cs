using EfPractice.Models;
using EfPractice.Repository.Class;
using EfPractice.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EfPractice.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMaster _master;
        public HomeController(IMaster Master)
        {
            _master = Master;
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

            var Customers = await _master.GetAllCustomersAsync();
            Customers.ForEach(c =>
            {
                c.Prn = GetProvinceName(c.City ?? 0);
            });
            model.Customers = Customers;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Customer(Customer customer)
        {
            CustomerViewModel model = new CustomerViewModel();
            model.Customers = await _master.GetAllCustomersAsync();
            model.Customers.ForEach(c =>
            {
                c.Prn = GetProvinceName(c.City ?? 0);
            });
            if (ModelState.IsValid)
            {
                if (customer.CID > 0)
                    await _master.UpdateCustomerAsync(customer);
                else
                    await _master.AddCustomerAsync(customer);
            }
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

            var Items = await _master.GetAllItemsAsync();
            //Items.ForEach(c =>
            //{
            //    c.Prn = GetCategory(c.City ?? 0);
            //});
            model.Items = Items;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Items(Item Items)
        {
            ItemsViewModel model = new ItemsViewModel();
            model.Items = await _master.GetAllItemsAsync();
            //model.Items.ForEach(c =>
            //{
            //    c.Prn = GetCategory(c.City ?? 0);
            //});
            if (ModelState.IsValid)
            {
                if (Items.Id > 0)
                    await _master.UpdateItemAsync(Items);
                else
                    await _master.AddItemAsync(Items);
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

            var Company = await _master.GetAllCompaniesAsync();
            //Company.ForEach(c =>
            //{
            //    c.Prn = GetCategory(c.City ?? 0);
            //});
            model.Companies = Company;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Company(Company Company)
        {
            CompanyViewModel model = new CompanyViewModel();
            model.Companies = await _master.GetAllCompaniesAsync();
            //model.Company.ForEach(c =>
            //{
            //    c.Prn = GetCategory(c.City ?? 0);
            //});
            if (ModelState.IsValid)
            {
                if (Company.Id > 0)
                    await _master.UpdateCompanyAsync(Company);
                else
                    await _master.AddCompanyAsync(Company);
            }
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

    }
}

