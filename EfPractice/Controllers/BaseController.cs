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

            // If companyId is 0, sign out and redirect to login
            if (CompanyId == 0)
            {
                // Sign out
                HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme).Wait();
                context.Result = RedirectToAction("Login", "Account");
            }
        }
    }
}
