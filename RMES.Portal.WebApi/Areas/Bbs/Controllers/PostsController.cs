using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RMES.EF;
using RMES.Framework;
using RMES.Portal.WebApi.Extensions;
using RMES.Services.Bbs;

namespace RMES.Portal.WebApi.Areas.Bbs.Controllers
{
    /// <summary>
    /// 帖子的文章
    /// </summary>
    public class PostsController : BbsBaseController
    {
        private readonly PostService _service;
        private readonly AppUser _user = new AppUser { Id = 1, NickName = "会飞的猪" };

        public PostsController(RmesContext context)
        {
            _service = new PostService(context);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 获取一篇帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<Result> Get(int id)
        {
            return await Task.FromResult(ResultUtil.Ok());
        }

        /// <summary>
        /// 回复主题
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Post(PostCreateDto dto)
        {
            return await _service.Create(dto, _user);
        }

        /// <summary>
        /// 赞
        /// </summary>
        /// <param name="id">帖子ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Like")]
        public async Task<Result> Like(int id)
        {
            return await _service.SetLike(id, _user);
        }

        /// <summary>
        /// 踩
        /// </summary>
        /// <param name="id">帖子ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Dislike")]
        public async Task<Result> Dislike(int id)
        {
            return await _service.SetDisLike(id, _user);
        }

        /// <summary>
        /// 回复帖子
        /// </summary>
        /// <param name="id">帖子ID</param>
        /// <param name="reply">回复</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/Reply")]
        public async Task<Result> Reply(int id, PostReplyDto reply)
        {
            return await _service.Reply(id, reply.TargetUserId, reply.Contents, _user);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            return await _service.Delete(id, _user);
        }
    }
}