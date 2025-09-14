using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EfPractice.Controllers
{
    public class BaseController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? CompanyId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId")?.Value;
                if (int.TryParse(claim, out int companyId))
                    return companyId;
                return null;
            }
        }
    }
}
