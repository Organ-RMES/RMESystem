﻿using Microsoft.AspNetCore.Mvc;
using RMES.EF;
using RMES.Framework;
using RMES.Portal.WebApi.Extensions;
using RMES.Services.Bbs;
using System.Threading.Tasks;

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

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="id">帖子ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> Delete(int id)
        {
            return await _service.Delete(id, _user);
        }
    }
}