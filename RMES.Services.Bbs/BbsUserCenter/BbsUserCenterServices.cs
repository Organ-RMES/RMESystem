using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RMES.EF;
using RMES.Entity;
using RMES.Framework;
using RMES.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMES.Services.Bbs
{
    public class BbsUserCenterServices
    {
        private readonly RmesContext _dbContext;

        private readonly IMapper _mapper;

        public BbsUserCenterServices(RmesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #region 我收藏的主题

        public PageListResult<MyCollectListView> MyCollects(int userId, int pageIndex, int pageSize)
        {
            var query = _dbContext.Collects.AsQueryable()
                    .Include(c => c.Topic).ThenInclude(c => c.Creator)
                    .Where(c => c.UserId == userId).AsNoTracking();

            var count = query.Count();

            var topics = query.Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new MyCollectListView
                    {
                        Id = c.TopicId,
                        Title = c.Topic.Title,
                        Creator = c.Topic.Creator.NickName,
                        UpdateAt = c.Topic.UpdateAt.ToString("yyyy-MM-dd HH:mm:ss")
                    }).ToList();

            return ResultUtil.PageList(count, pageIndex, pageSize, topics);
        }

        #endregion

        #region 回复
        // 我回复的
        public PageListResult<ReplyView> MyReplies(int userId, int pageIndex, int pageSize)
        {
            var query = _dbContext.Replies.AsQueryable()
                .Where(r => r.CreateBy == userId);
            var count = query.Count();
            var data = query.Include(r => r.Creator).Include(r => r.TargetUser)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(r => r.Id)
                .AsNoTracking()
                .ToList();
            var views = _mapper.Map<List<ReplyView>>(data);
            return ResultUtil.PageList(count, pageIndex, pageSize, views);
        }

        // 回复我的
        public PageListResult<ReplyView> ReplyMe(int userId, int pageIndex, int pageSize)
        {
            var query = _dbContext.Replies.AsQueryable()
                .Where(r => r.TargetUserId == userId);
            var count = query.Count();
            var data = query.Include(r => r.Creator).Include(r => r.TargetUser)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(r => r.Id)
                .AsNoTracking()
                .ToList();
            var views = _mapper.Map<List<ReplyView>>(data);
            return ResultUtil.PageList(count, pageIndex, pageSize, views);
        }

        #endregion

        #region 我发布的主题

        public async Task<PageListResult<TopicListView>> MyTopics(int userId, int pageIndex = 1, int pageSize = 20)
        {
            var query = _dbContext.Topics.AsQueryable();
            query = query.Where(t => t.CreateBy == userId);
            var count = await query.CountAsync();
            var source = await query.Include(t => t.Creator)
                .OrderByDescending(t => t.UpdateAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            var data = _mapper.Map<List<TopicListView>>(source);
            return ResultUtil.PageList(count, pageIndex, pageSize, data);
        }

        #endregion

        #region 我回复的帖子

        public async Task<PageListResult<Post>> MyPosts(int userId, int pageIndex, int pageSize)
        {
            var query = _dbContext.Posts.AsQueryable();
            var where = LinqExtensions.True<Post>();
            where = where.And(p => p.CreateBy == userId && p.IsMaster == false);
            query = query.Where(where);
            var count = await query.CountAsync();
            var source = await query.Include(t => t.Topic)
                .OrderByDescending(t => t.UpdateAt).Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            var data = _mapper.Map<List<Post>>(source);
            return ResultUtil.PageList(count, pageIndex, pageSize, data);
        }

        #endregion
    }
}
