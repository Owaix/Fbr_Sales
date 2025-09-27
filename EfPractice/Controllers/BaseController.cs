using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace EfPractice.Controllers
{
    public class BaseController : Controller
    {
        protected int? CompanyId
        {
            get
            {
                var claim = User.Claims.FirstOrDefault(c => c.Type == "CompanyId");
                if (claim != null && int.TryParse(claim.Value, out int id))
                    return id;
                return 0;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // If companyId is 0 (not authenticated properly) redirect to Identity login page
            if (CompanyId == 0 && !User.Identity?.IsAuthenticated == true)
            {
                // Sign out existing cookie (if any)
                HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme).Wait();
                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToPageResult("/Account/Login", new { area = "Identity", ReturnUrl = returnUrl });
            }
        }
    }
}
