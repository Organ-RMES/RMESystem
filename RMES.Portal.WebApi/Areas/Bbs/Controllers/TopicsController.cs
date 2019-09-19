using Microsoft.AspNetCore.Mvc;
using RMES.EF;
using RMES.Entity;
using RMES.Framework;
using RMES.Portal.WebApi.Extensions;
using RMES.Services.Bbs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMES.Portal.WebApi.Areas.Bbs.Controllers
{
    public class TopicsController : BbsBaseController
    {
        private readonly RmesContext _context;

        private readonly TopicService _topicService;

        private readonly AppUser _user = new AppUser { Id = 1, NickName = "会飞的猪" };

        public TopicsController(RmesContext context, TopicService service)
        {
            _context = context;
            _topicService = service;
        }

        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result<List<TopicListView>>> GetTopics([FromQuery]TopicSearchInput input, int pageIndex, int pageSize = 20)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 5 ? 5 : pageSize;

            return await _topicService.GetListView(input, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取主题列表，包含分页信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public async Task<PageListResult<TopicListView>> GetPagedTopics([FromQuery]TopicSearchInput input, int pageIndex, int pageSize = 20)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 5 ? 5 : pageSize;

            return await _topicService.GetPageListView(input, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取主题详情视图，包含所有的帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Result> GetTopic(int id)
        {
            var result = await _topicService.LoadWithPosts(id);
            return result;
        }

        /// <summary>
        /// 更新主题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPost("Edit/{id}")]
        public async Task<Result> PutTopic(int id, TopicEditDto topic)
        {
            if (id != topic.Id)
            {
                return ResultUtil.BadRequest();
            }

            if (string.IsNullOrWhiteSpace(topic.Title))
            {
                return ResultUtil.BadRequest("主题标题不能为空");
            }

            var origin = await _context.FindAsync<Topic>(id);
            if (origin == null)
            {
                return ResultUtil.BadRequest("主题不存在或已删除");
            }
            if (origin.CreateBy != _user.Id)
            {
                return ResultUtil.BadRequest("无权编辑此主题");
            }

            origin.Title = topic.Title;
            origin.UpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return ResultUtil.Ok();
        }

        /// <summary>
        /// 发布主题
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> PostTopic(TopicCreateDto topic)
        {
            if (!TryValidateModel(topic))
            {
                var errors = new List<string>();
                if (!TryValidateModel(topic))
                {
                    foreach (var value in ModelState.Values)
                    {
                        foreach (var error in value.Errors)
                        {
                            errors.Add(error.ErrorMessage);
                        }
                    }
                }

                return ResultUtil.BadRequest(string.Join(";", errors));
            }
            var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Path}";
            var result = await _topicService.Create(topic, _user, host);
            return result;
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="id">主题ID</param>
        /// <returns></returns>
        [HttpPost("Delete/{id}")]
        public async Task<Result> DeleteTopic(int id)
        {
            return await _topicService.Delete(id, _user);
        }

        /// <summary>
        /// 收藏主题
        /// </summary>
        /// <param name="id">主题Id</param>
        /// <returns></returns>
        [HttpPost("{id}/Collect")]
        public async Task<Result> Collect(int id)
        {
            return await _topicService.Collect(id, _user);
        }

        /// <summary>
        /// 取消收藏主题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/UnCollect")]
        public async Task<Result> UnCollect(int id)
        {
            return await _topicService.RemoveCollect(id, _user);
        }
    }
}