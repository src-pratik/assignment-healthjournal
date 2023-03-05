using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace portal.WebAPI.Controllers
{
    public abstract class PortalBaseController : ControllerBase
    {
        protected string UserId
        {
            get
            {
                return HttpContext?.User?.Claims?.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            }
        }
    }
}