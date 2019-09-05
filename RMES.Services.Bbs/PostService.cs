﻿using Microsoft.EntityFrameworkCore;
using RMES.EF;
using RMES.Entity;
using RMES.Framework;
using System;
using System.Threading.Tasks;

namespace RMES.Services.Bbs
{
    /// <summary>
    /// 帖子操作Service
    /// </summary>
    public class PostService
    {
        private readonly RmesContext _context;

        public PostService(RmesContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 回帖
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> Create(PostCreateDto dto, AppUser user)
        {
            if (string.IsNullOrWhiteSpace(dto.Contents))
            {
                return ResultUtil.BadRequest("跟帖内容不能为空");
            }

            var topic = await _context.Topics.FindAsync(dto.TopicId);
            if (topic == null || topic.IsDel)
            {
                return ResultUtil.NotFound("该主题不存在或已删除");
            }

            var post = new Post
            {
                TopicId = topic.Id,
                Contents = dto.Contents,
                CreateBy = user.Id
            };
            _context.Posts.Add(post);

            // 主题回帖数量加1
            topic.CommentCount += 1;
            topic.UpdateAt = DateTime.Now;

            // 用户回帖数量加1
            var creator = await _context.FindAsync<User>(user.Id);
            creator.CommentCount++;

            var result = await _context.SaveChangesAsync();
            return result > 0 ? ResultUtil.Ok(post.Id) : ResultUtil.DbFail();
        }

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> Delete(int id, AppUser user)
        {
            var post = await _context.Posts.Include(p => p.Topic).Include(p => p.Creator).FirstOrDefaultAsync(p => p.Id == id);
            if (post == null || post.IsDel)
            {
                return ResultUtil.NotFound();
            }

            // 只有帖子的作者和主题的创建人才能删除
            if (post.CreateBy != user.Id || post.Topic.CreateBy != user.Id)
            {
                return ResultUtil.UnAuthorization("您无权删除此记录");
            }

            // 主贴不能删除
            if (post.IsMaster)
            {
                return ResultUtil.BadRequest("主贴禁止该操作，请直接删除主题");
            }
            
            post.IsDel = true;

            // 主题回帖数量减1
            post.Topic.CommentCount -= 1;

            // 用户回帖数量减1
            post.Creator.CommentCount -= 1;
            var result = await _context.SaveChangesAsync();
            return result > 0 ? ResultUtil.Ok() : ResultUtil.DbFail();
        }

        /// <summary>
        /// 点赞，如果已经点赞，则取消；如果未点赞，则设置为点赞；如果已踩，返回错误提醒
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> SetLike(int id, AppUser user)
        {
            var post = await _context.FindAsync<Post>(id);
            if (post == null || post.IsDel)
            {
                return ResultUtil.NotFound("请求的帖子不存在或已删除");
            }

            var log = await 
                _context.LikeLogs.SingleOrDefaultAsync(h => h.UserId == user.Id && h.PostId == id);

            History history;

            if (log != null)
            {
                // 如果已踩，返回错误提醒
                if (log.Value == 2)
                {
                    return ResultUtil.BadRequest("您已将该帖子设为不喜欢，请先取消该状态后再试！");
                }

                // 如果已赞，修改状态为0（不赞不踩），帖子点赞数减1，并添加取消赞的历史记录
                // 如果未赞，修改状态为1（赞），帖子点赞数加1，并添加点赞的历史记录
                if (log.Value == 1)
                {
                    log.Value = 0;
                    post.LikeCount -= 1;
                    history = HistoryUtil.UndoLike(post.TopicId, post.Id, "", user.Id);
                    _context.Add(history);
                }
                else
                {
                    log.Value = 1;
                    post.LikeCount += 1;
                    history = HistoryUtil.Like(post.TopicId, post.Id, "", user.Id);
                    _context.Add(history);
                }
            }
            else
            {
                // 添加记录，帖子点赞数加1，并添加点赞的历史记录
                log = new LikeLog
                {
                    TopicId = post.TopicId,
                    PostId = id,
                    UserId = user.Id,
                    Value = 1
                };
                _context.LikeLogs.Add(log);
                post.LikeCount += 1;
                history = HistoryUtil.Like(post.TopicId, post.Id, "", user.Id);
                _context.Add(history);
            }

            var result = await _context.SaveChangesAsync();
            return result > 0 ? ResultUtil.Ok() : ResultUtil.DbFail();
        }

        /// <summary>
        /// 踩,如果已经踩，则取消；如果未踩，则设置为踩；如果已点赞，返回错误提醒
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> SetDisLike(int id, AppUser user)
        {
            var post = await _context.FindAsync<Post>(id);
            if (post == null || post.IsDel)
            {
                return ResultUtil.NotFound("请求的帖子不存在或已删除");
            }

            var log = await
                _context.LikeLogs.SingleOrDefaultAsync(h => h.UserId == user.Id && h.PostId == id);

            History history;
            if (log != null)
            {
                // 如果已赞，返回错误提醒
                if (log.Value == 1)
                {
                    return ResultUtil.BadRequest("您已将该帖子设为喜欢，请先取消该状态后再试！");
                }
                
                // 如果已踩，修改状态为0（不赞不踩），帖子不喜欢数量减1，添加取消踩的历史记录
                // 如果未踩，修改状态为2（踩），帖子不喜欢数量加1，添加踩的历史记录
                if (log.Value == 2)
                {
                    log.Value = 0;
                    post.DislikeCount -= 1;
                    history = HistoryUtil.UndoDislike(post.TopicId, post.Id, "", user.Id);
                    _context.Add(history);
                }
                else
                {
                    log.Value = 2;
                    post.DislikeCount += 1;
                    history = HistoryUtil.Dislike(post.TopicId, post.Id, "", user.Id);
                    _context.Add(history);
                }
            }
            else
            {
                // 添加记录，帖子不喜欢数加1，并添加踩的历史记录
                log = new LikeLog
                {
                    TopicId = post.TopicId,
                    PostId = id,
                    UserId = user.Id,
                    Value = 2
                };
                post.DislikeCount += 1;
                _context.LikeLogs.Add(log);
                history = HistoryUtil.Dislike(post.TopicId, post.Id, "", user.Id);
                _context.Add(history);
            }

            var result = await _context.SaveChangesAsync();
            return result > 0 ? ResultUtil.Ok() : ResultUtil.DbFail();
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetUserId"></param>
        /// <param name="contents"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Result> Reply(int id, int targetUserId, string contents, AppUser user)
        {
            var post = await _context.FindAsync<Post>(id);
            if (post == null || post.IsDel)
            {
                return ResultUtil.NotFound();
            }

            var reply = new Reply
            {
                TopicId = post.TopicId,
                PostId = post.Id,
                TargetUserId = targetUserId <= 0 ? post.CreateBy : targetUserId,    // 如果目标用户为空，则默认为帖子的作者
                Contents = contents,
                CreateBy = user.Id
            };

            var history = HistoryUtil.Reply(post.TopicId, post.Id, targetUserId, "", contents, user.Id);
            post.ReplyCount += 1;
            _context.Replies.Add(reply);
            _context.Histories.Add(history);

            // TODO:确认下主题的排序方式，如果按更新时间排序，那需确认回复帖子是否需要修改主题的时间

            var result = await _context.SaveChangesAsync();
            return result > 0 ? ResultUtil.Ok() : ResultUtil.DbFail();
        }
    }
}
