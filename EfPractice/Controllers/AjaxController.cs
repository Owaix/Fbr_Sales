using EfPractice.Models;
using EfPractice.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EfPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AjaxController : ControllerBase
    {
        private readonly IMaster _master;
        public AjaxController(IMaster master)
        {
            _master = master;
        }

        [HttpGet("GetItems")]
        public async Task<IActionResult> GetItems()
        {
            var items = await _master.GetAllItemsAsync();
            var result = items.Select(x => new
            {
                HsCode = x.HSCode,
                ProductDescription = x.ItemName,
                Rate = x.Rate?.ToString() ?? "",
                UoM = x.UOM,
                Quantity = 1,
                ValueSalesExcludingST = 0,
                SalesTaxApplicable = 0,
                FurtherTax = 0,
                Discount = 0
            }).ToList();
            return Ok(result);
        }
    }
}