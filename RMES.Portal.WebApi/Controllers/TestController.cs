using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RMES.Portal.WebApi.Controllers
{
    [Route("Test")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            throw new Exception("MVC异常");
            return View();
        }
    }
}