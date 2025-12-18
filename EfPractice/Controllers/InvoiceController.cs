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
    public partial class InvoiceController : BaseController
    {
        private readonly IMaster _master;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvoiceController(IMaster master, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _master = master;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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

            // Bind customers and accounts to model for dropdowns
            var cid = CompanyId ?? 0;
            var customers = await _master.GetAllCustomersAsync(cid);
            ViewBag.Customers = customers
                .Select(c => new SelectListItem { Value = c.CustomerID.ToString(), Text = c.Name ?? c.CusName ?? string.Empty })
                .ToList();
            var accounts = await _master.GetAccountsAsync(cid);
            ViewBag.Accounts = accounts
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = string.Concat(a.AccountId, " - ", a.AccountTitle) })
                .ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SInv(SaleInvoice model)
        {
            //if (!ModelState.IsValid)
            //{
            //    // Ensure Items is non-null for the view's for-loop
            //    model.Items ??= new List<SaleInvoiceItem>();

            //    // Repopulate dropdowns used by the view
            //    var cid = CompanyId ?? 0;
            //    var customers = await _master.GetAllCustomersAsync(cid);
            //    ViewBag.Customers = customers
            //        .Select(c => new SelectListItem { Value = c.CustomerID.ToString(), Text = c.Name ?? c.CusName ?? string.Empty })
            //        .ToList();

            //    var accounts = await _master.GetAccountsAsync(cid);
            //    ViewBag.Accounts = accounts
            //        .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = string.Concat(a.AccountId, " - ", a.AccountTitle) })
            //        .ToList();

            //    return View(model);
            //}

            try
            {
                if (model.Id == 0)
                {
                    // Generate a random invoice number if not provided
                    if (string.IsNullOrWhiteSpace(model.invoiceNumber))
                    {
                        // Pattern: INV-YYYYMMDD-HHMMSS-XXXX (XXXX random alphanumeric)
                        var ts = DateTime.UtcNow.ToString("yyyyMMdd-HHmm");
                        var rnd = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                            .Replace("/", string.Empty)
                            .Replace("+", string.Empty)
                            .Substring(0, 6)
                            .ToUpperInvariant();
                        model.invoiceNumber = $"INV-{ts}-{rnd}";
                    }
                    //model.CompanyId = CompanyId ?? 0;
                    //var resp = await _master.SendInvoiceToFbrAsync(model);
                    //var content = await resp.Content.ReadAsStringAsync();
                    //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    //var fbrResponse = JsonSerializer.Deserialize<FbrResponse>(content, options);

                    //if (fbrResponse?.validationResponse?.statusCode == "01")
                    //{
                    //model.fbrinvoiceNumber = fbrResponse.invoiceNumber;
                    //    model.dated = fbrResponse.dated;
                    var id = await _master.AddSaleInvoiceAsync(model);
                    //    TempData["Message"] = $"Invoice sent successfully. FBR Invoice#: {fbrResponse.invoiceNumber}";
                    //    TempData["Message"] = $"Invoice sent successfully. FBR Invoice#: {fbrResponse.invoiceNumber}";
                    return RedirectToAction("PrintInvoice", new { id });
                    //}
                    //else
                    //{
                    //    FBRErrorCodes fBRErrorCodes = new FBRErrorCodes();
                    //    var Error = fBRErrorCodes.GetErrorByCode(fbrResponse?.validationResponse?.errorCode);
                    //    ModelState.AddModelError(string.Empty, Error ?? "Unknown FBR error");
                    //}
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

            // Repopulate dropdowns again before returning after error
            var cid2 = CompanyId ?? 0;
            ViewBag.Customers = (await _master.GetAllCustomersAsync(cid2))
                .Select(c => new SelectListItem { Value = c.CustomerID.ToString(), Text = c.Name ?? c.CusName ?? string.Empty })
                .ToList();
            ViewBag.Accounts = (await _master.GetAccountsAsync(cid2))
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = string.Concat(a.AccountId, " - ", a.AccountTitle) })
                .ToList();

            model.Items ??= new List<SaleInvoiceItem>();
            return View(model);
        }
    }
}
