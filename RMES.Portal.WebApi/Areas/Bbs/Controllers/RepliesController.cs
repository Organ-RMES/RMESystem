using Microsoft.AspNetCore.Mvc;
using RMES.Framework;
using RMES.Portal.WebApi.Extensions;
using RMES.Services.Bbs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMES.Portal.WebApi.Areas.Bbs.Controllers
{
    /// <summary>
    /// 帖子的回复
    /// </summary>
    public class RepliesController : BbsBaseController
    {
        private readonly PostService _service;
        private readonly ReplyService _replyService;
        private readonly AppUser _user = new AppUser { Id = 1, NickName = "会飞的猪" };

        public RepliesController(PostService service, ReplyService replyService)
        {
            _service = service;
            _replyService = replyService;
        }

        /// <summary>
        /// 根据主题Id获取帖子的回复（不是回帖，是对帖子的回复）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Topic/{id}")]
        public async Task<Result<List<ReplyView>>> GetByTopicId(int id)
        {
            var data = await _service.GetRepliesByTopicId(id);
            return ResultUtil.Ok(data);
        }

        /// <summary>
        /// 根据帖子Id获取帖子的回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Post/{id}")]
        public async Task<Result<List<ReplyView>>> GetByPostId(int id)
        {
            var data = await _service.GetRepliesByPostId(id);
            return ResultUtil.Ok(data);
        }

        /// <summary>
        /// 根据帖子Ids获取帖子的回复
        /// </summary>
        /// <param name="ids">用半角逗号分隔的数字</param>
        /// <returns></returns>
        [HttpGet("Posts")]
        public async Task<Result<List<ReplyView>>> GetByPostIds(string ids)
        {
            var result = new List<ReplyView>();
            if (string.IsNullOrWhiteSpace(ids))
            {
                return ResultUtil.Ok(result);
            }

            var idList = new List<int>();
            foreach (var idStr in ids.Split(","))
            {
                if (int.TryParse(idStr, out var id))
                {
                    idList.Add(id);
                }
            }

            if (idList.Count == 0)
            {
                return ResultUtil.Ok(result);
            }

            var data = await _service.GetReplies(idList.ToArray());
            return ResultUtil.Ok(data);
        }

        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("Delete/{id}")]
        public async Task<Result> Delete(int id)
        {
            var result = await _replyService.Delete(id, _user);
            return result;
        }
    }
}