using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RMES.EF;
using RMES.Entity;
using RMES.Framework;
using RMES.Services.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RMES.Util;

namespace RMES.Services.Bbs
{
    /// <summary>
    /// 主题操作Service
    /// </summary>
    public class TopicService
    {
        private readonly RmesContext _context;
        private readonly IMapper _mapper;

        public TopicService(RmesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region 新帖
        /// <summary>
        /// 发布主题
        /// </summary>
        /// <param name="topicCreateDto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> Create(TopicCreateDto topicCreateDto, AppUser user, string host)
        {
            var error = BeforeCreate(topicCreateDto);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return ResultUtil.BadRequest(error);
            }

            if (string.IsNullOrWhiteSpace(topicCreateDto.Summary))
            {
                //var summary = HtmlUtil.StripHtml(topicCreateDto.Contents).Trim()
                //    .Replace(" ","")
                //    .Replace("\r\n", "")
                //    .Replace("\n", "");

                var summary = HtmlUtil.GetContentSummary(topicCreateDto.Contents, 100, true);

                if (summary.Length > 100)
                {
                    summary = summary.Substring(0, 100) + "...";
                }

                topicCreateDto.Summary = summary;
            }

            if (string.IsNullOrWhiteSpace(topicCreateDto.Pics))
            {
                var urls = HtmlUtil.GetImageUrl(topicCreateDto.Contents);
                if (urls.Count > 0)
                {
                    var urlList = new List<string>();
                    foreach (var url in urls.Take(4))
                    {
                        if (!url.Contains("images/face"))
                        {
                            if (!url.StartsWith("http"))
                            {
                                urlList.Add(Path.Combine($"{host}", url));
                            }

                            urlList.Add(url);
                        }
                    }
                    topicCreateDto.Pics = string.Join(",", urlList);
                }
                else
                {
                    topicCreateDto.Pics = "";
                }
            }

            var topic = new Topic
            {
                ChannelId = topicCreateDto.ChannelId,
                Title = topicCreateDto.Title,
                Summary = topicCreateDto.Summary,
                Pics = topicCreateDto.Pics,
                Type = topicCreateDto.Type,
                CreateBy = user.Id,
                Posts = new List<Post>
                {
                    new Post
                    {
                        IsMaster = true,
                        Contents = topicCreateDto.Contents,
                        CreateBy = user.Id,
                        CreateAt = DateTime.Now
                    }
                }
            };

            var creator = await _context.FindAsync<User>(user.Id);

            creator.TopicCount++;
            _context.Topics.Add(topic);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? ResultUtil.Ok() : ResultUtil.DbFail();
        }
        #endregion

        #region 删帖
        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> Delete(int id, AppUser user)
        {
            var topic = await _context.Topics.Include(t => t.Creator).SingleOrDefaultAsync(t => t.Id == id);
            if (topic == null || topic.IsDel)
            {
                return ResultUtil.NotFound();
            }

            if (topic.CreateBy != user.Id)
            {
                return ResultUtil.UnAuthorization("您无权删除此记录");
            }

            topic.IsDel = true;
            topic.Creator.TopicCount -= 1;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? ResultUtil.Ok() : ResultUtil.DbFail();
        }
        #endregion

        #region 查询
        /// <summary>
        /// 加载主题及其帖子
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<Result> LoadWithPosts(int id, int pageSize = 20)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return ResultUtil.NotFound();
            }

            var posts = await _context.Posts.Include(p => p.Creator)
                                .Where(p => p.TopicId == id)
                                .OrderByDescending(p => p.IsMaster)
                                .ThenByDescending(p => p.UpdateAt)
                                .Take(pageSize)
                                .ToListAsync();

            var view = new TopicDetailsView
            {
                Id = topic.Id,
                Title = topic.Title,
                CommentCount = topic.CommentCount,
                Posts = _mapper.Map<List<PostView>>(posts)
            };
            return ResultUtil.Ok(view);
        }

        public async Task<Result<List<TopicListView>>> GetListView(TopicSearchInput input, int pageIndex = 1, int pageSize = 20)
        {
            var query = _context.Topics.Include(t => t.Creator);
            var where = input?.ToExpression() ?? LinqExtensions.True<Topic>();
            var source = await query.Where(where).OrderByDescending(t => t.UpdateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync();
            var data = _mapper.Map<List<TopicListView>>(source);
            return ResultUtil.Ok(data);
        }

        public async Task<PageListResult<TopicListView>> GetPageListView(TopicSearchInput input, int pageIndex = 1, int pageSize = 20)
        {
            var query = _context.Topics.AsQueryable();
            var where = input?.ToExpression() ?? LinqExtensions.True<Topic>();
            query = query.Where(where);
            var count = await query.CountAsync();
            var source = await query.Include(t => t.Creator)
                .OrderByDescending(t => t.UpdateAt).Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var data = _mapper.Map<List<TopicListView>>(source);
            return ResultUtil.PageList(count, pageIndex, pageSize, data);
        }

        #endregion

        #region 收藏

        public async Task<Result> Collect(int id, AppUser user)
        {
            var topic = await _context.Topics.SingleOrDefaultAsync(t => t.Id == id);
            if (topic == null)
            {
                return ResultUtil.NotFound("请求的主题不存在");
            }

            var collect = await _context.Collects.SingleOrDefaultAsync(c => c.UserId == user.Id && c.TopicId == id);
            if (collect == null)
            {
                _context.Collects.Add(new Collect
                {
                    TopicId = id,
                    UserId = user.Id,
                    CollectAt = DateTime.Now
                });

                topic.CollectCount++;

                var result = await _context.SaveChangesAsync();
                return result > 0 ? ResultUtil.Ok() : ResultUtil.DbFail();
            }

            return ResultUtil.BadRequest("该主题已收藏");
        }

        public async Task<Result> RemoveCollect(int id, AppUser user)
        {
            var collect = await _context.Collects.Include(c => c.Topic).SingleOrDefaultAsync(c => c.UserId == user.Id && c.TopicId == id);
            if (collect != null)
            {
                collect.Topic.CollectCount--;
                _context.Collects.Remove(collect);

                var result = await _context.SaveChangesAsync();
                return result > 0 ? ResultUtil.Ok() : ResultUtil.DbFail();
            }

            return ResultUtil.BadRequest("您尚未收藏该主题");
        }
        #endregion

        #region 私有方法
        private string BeforeCreate(TopicCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                return "标题不能为空";
            }

            return string.Empty;
        }
        #endregion
    }
}
