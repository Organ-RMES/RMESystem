using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMES.EF;
using RMES.Entity;
using RMES.Framework;
using RMES.Portal.WebApi.Extensions;
using RMES.Services.Bbs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMES.Portal.WebApi.Areas.Bbs.Controllers
{
    public class TopicsController : BbsBaseController
    {
        private readonly RmesContext _context;

        private readonly TopicService _topicService;

        private readonly AppUser _user = new AppUser { Id = 1, NickName = "会飞的猪" };

        public TopicsController(RmesContext context)
        {
            _context = context;
            _topicService = new TopicService(context);
        }

        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics([FromQuery]TopicSearchInput input, int pageIndex, int pageSize = 20)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 5 ? 5 : pageSize;

            var where = input.ToExpression();

            return await _context.Topics.Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTopic(int id, Topic topic)
        {
            if (id != topic.Id)
            {
                return BadRequest();
            }

            _context.Entry(topic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
            var result = await _topicService.Create(topic, _user);
            return result;
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="id">主题ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<Result> DeleteTopic(int id)
        {
            return await _topicService.Delete(id, _user);
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.Id == id);
        }
    }
}