using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMES.Services.Bbs;

namespace RMES.Portal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public TopicService Service { get; set; }

        public ValuesController()
        {
            Console.WriteLine("注册");
        }

        /// <summary>
        /// 测试的方法
        /// </summary>
        /// <param name="id">测试的参数，没啥用</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string[] GetValues(int id)
        {
            return new[] { "value1", "value2" };
        }
    }
}