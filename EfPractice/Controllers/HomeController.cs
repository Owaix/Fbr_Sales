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
                model.Customer = await _master.GetByIdAsync(id);
            model.Customers = await _master.GetAllAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Customer(Customer customer)
        {
            if (customer.CID > 0)
                await _master.UpdateAsync(customer);
            else
                await _master.AddAsync(customer);
            return RedirectToAction("Customer", "Home");
        }

        public async Task<IActionResult> CustomerDelete(int id)
        {
            await _master.DeleteAsync(id);
            return RedirectToAction("Customer", "Home");
        }

        public IActionResult Items()
        {

            //  var stdRecord = studentDB.Students.ToList();

            return View(/*stdRecord*/);

        }

        public IActionResult Company()
        {

            //  var stdRecord = studentDB.Students.ToList();

            return View(/*stdRecord*/);

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
    }
}
