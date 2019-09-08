using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RMES.Portal.WebApi.Extensions
{
    [Area("Bbs")]
    [Route("Bbs/[controller]")]
    [ApiController]
    //[Authorize]
    public class BbsBaseController : ControllerBase
    {
    }
}
