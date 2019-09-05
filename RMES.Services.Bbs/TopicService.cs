using Microsoft.EntityFrameworkCore;
using RMES.EF;
using RMES.Entity;
using RMES.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMES.Services.Bbs
{
    /// <summary>
    /// 主题操作Service
    /// </summary>
    public class TopicService
    {
        private readonly RmesContext _context;

        public TopicService(RmesContext context)
        {
            _context = context;
        }

        #region 新帖
        public async Task<Result> Create(TopicCreateDto topicCreateDto, AppUser user)
        {
            var error = BeforeCreate(topicCreateDto);
            if (!string.IsNullOrWhiteSpace(error))
            {
                return ResultUtil.BadRequest(error);
            }

            var topic = new Topic
            {
                ChannelId = topicCreateDto.ChannelId,
                Title = topicCreateDto.Title,
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

        public async Task<Result> LoadWithPosts(int id, int pageSize = 20)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return ResultUtil.NotFound();
            }

            var posts = await _context.Posts.Include(p => p.Creator)
                                .Where(p => p.TopicId == id)
                                .OrderByDescending(p => p.IsMaster).ThenByDescending(p => p.UpdateAt)
                                .Select(p => new PostView
                                {
                                    Id = p.Id,
                                    IsMaster = p.IsMaster,
                                    Contents = p.Contents,
                                    IsAccept = p.IsAccept,
                                    ReplyCount = p.ReplyCount,
                                    LikeCount = p.LikeCount,
                                    DislikeCount = p.DislikeCount,
                                    UpdateAt = p.UpdateAt,
                                    CreateBy = p.CreateBy,
                                    Author = p.Creator.NickName,
                                    AuthorGrade = p.Creator.Grade,
                                    AuthorAvatar = p.Creator.Avatar
                                })
                                .ToListAsync();

            var view = new TopicDetailsView
            {
                Id = topic.Id,
                Title = topic.Title,
                Posts = posts
            };
            return ResultUtil.Ok(view);
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
