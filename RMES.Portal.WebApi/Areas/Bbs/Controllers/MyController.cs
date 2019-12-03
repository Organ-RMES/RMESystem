using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RMES.Entity;
using RMES.Framework;
using RMES.Portal.WebApi.Extensions;
using RMES.Services.Bbs;
using System.Threading.Tasks;

namespace RMES.Portal.WebApi.Areas.Bbs.Controllers
{
    public class MyController : BbsBaseController
    {
        private readonly BbsUserCenterServices _service;

        private readonly IMapper _mapper;

        private readonly AppUser _user = new AppUser { Id = 1, NickName = "会飞的猪" };

        public MyController(BbsUserCenterServices service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// 我收藏的主题
        /// </summary>
        /// <param name="id">页码</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Collects/{id}")]
        public PageListResult<MyCollectListView> MyCollects(int id)
        {
            id = id < 1 ? 1 : id;
            return _service.MyCollects(_user.Id, id, 20);
        }

        /// <summary>
        /// 我的回复帖子
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Posts/{id}")]
        public async Task<PageListResult<Post>> MyPosts(int id)
        {
            id = id < 1 ? 1 : id;
            return await _service.MyPosts(_user.Id, id, 20);
        }

        /// <summary>
        /// 我发布的主题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Topics/{id}")]
        public async Task<PageListResult<TopicListView>> MyTopics(int id)
        {
            id = id < 1 ? 1 : id;
            return await _service.MyTopics(_user.Id, id, 20);
        }

        /// <summary>
        /// 我的回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Replies/{id}")]
        public PageListResult<ReplyView> MyReplies(int id)
        {
            id = id < 1 ? 1 : id;
            return _service.MyReplies(_user.Id, id, 20);
        }

        /// <summary>
        /// 回复我的
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ReplyMe/{id}")]
        public PageListResult<ReplyView> ReplyMe(int id)
        {
            id = id < 1 ? 1 : id;
            return _service.ReplyMe(_user.Id, id, 20);
        }
    }
}