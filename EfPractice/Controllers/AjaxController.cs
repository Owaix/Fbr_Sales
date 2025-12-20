using EfPractice.Models;
using EfPractice.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EfPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
                ListPrice = x.ListPrice?.ToString() ?? "",
                PosRate = x.PosRate?.ToString() ?? "",
                ProductDescription = x.Name,
                Rate = x.Rate?.ToString() ?? "",
                UoM = x.UOM,
                Quantity = 1,
                ValueSalesExcludingST = 0,
                SalesTaxApplicable = 0,
                FurtherTax = 0,
                Discount = 0,
                Id = x.Id
            }).ToList();
            return Ok(result);
        }

        [HttpGet("GetTaxes")]
        public async Task<IActionResult> GetTaxes()
        {
            int companyId = 0;
            var claim = User?.FindFirst("CompanyId")?.Value;
            if (!string.IsNullOrEmpty(claim)) int.TryParse(claim, out companyId);
            var taxes = await _master.GetTaxesAsync(companyId);
            var result = taxes.Select(t => new
            {
                id = t.Id,
                text = t.Name,
                rate = t.DefaultRate,
                type = t.TaxType
            }).ToList();
            return Ok(result);
        }
    }
}