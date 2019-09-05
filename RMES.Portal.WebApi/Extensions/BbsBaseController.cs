using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMES.Portal.WebApi.Extensions
{
    [Area("Bbs")]
    [Route("Bbs/[controller]")]
    [ApiController]
    public class BbsBaseController : ControllerBase
    {
    }
}
