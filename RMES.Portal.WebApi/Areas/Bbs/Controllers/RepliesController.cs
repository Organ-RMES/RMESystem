using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMES.Services.Bbs;

namespace RMES.Portal.WebApi.Areas.Bbs.Controllers
{
    /// <summary>
    /// 帖子的回复
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RepliesController : ControllerBase
    {
        private readonly PostService _service;

        public RepliesController(PostService service)
        {
            _service = service;
        }

        /// <summary>
        /// 根据主题Id获取帖子的回复（不是回帖，是对帖子的回复）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Topic/{id}")]
        public async Task<IActionResult> GetByTopicId(int id)
        {
            var data = await _service.GetRepliesByTopicId(id);
            return Ok(data);
        }
    }
}