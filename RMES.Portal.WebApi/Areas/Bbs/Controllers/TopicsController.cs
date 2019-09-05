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

        // GET: api/Topics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
        {
            return await _context.Topics.ToListAsync();
        }

        // GET: api/Topics/5
        [HttpGet("{id}")]
        public async Task<Result> GetTopic(int id)
        {
            var result = await _topicService.LoadWithPosts(id);
            return result;
        }

        // PUT: api/Topics/5
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

        // POST: api/Topics
        [HttpPost]
        public async Task<Result> PostTopic(TopicCreateDto topic)
        {
            var result = await _topicService.Create(topic, _user);

            return result;
        }

        // DELETE: api/Topics/5
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