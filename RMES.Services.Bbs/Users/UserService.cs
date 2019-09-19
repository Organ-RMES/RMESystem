using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RMES.EF;
using RMES.Framework;
using RMES.Util;
using RMES.Entity;
using RMES.Services.Common;

namespace RMES.Services.Bbs
{
    public class UserService
    {
        private readonly RmesContext _context;

        private readonly IMapper _mapper;

        public UserService(RmesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Result<User> Login(string account, string pw)
        {
            var user = _context.Users.SingleOrDefault(u => u.Account == account.Trim());
            if (user == null)
            {
                return ResultUtil.Do<User>(404, "用户不存在", null);
            }

            var encryptPw = Md5EncryptUtil.Encrypt(pw, user.Salt);
            if (encryptPw != user.Pw)
            {
                return ResultUtil.Do<User>(404, "密码错误", null);
            }

            return ResultUtil.Ok<User>(user, "");
        }

        public async Task<PageListResult<TopicListView>> MyTopics(TopicSearchInput input, int userId, int pageIndex, int pageSize)
        {
            var query = _context.Topics.AsQueryable();
            input.Author = userId;
            var where = input?.ToExpression() ?? LinqExtensions.True<Topic>();
            query = query.Where(where);
            var count = await query.CountAsync();
            var source = await query.Include(t => t.Creator)
                .OrderByDescending(t => t.UpdateAt).Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            var data = _mapper.Map<List<TopicListView>>(source);
            return ResultUtil.PageList(count, pageIndex, pageSize, data);
        }

        public async Task<PageListResult<Post>> MyComments(int userId, int pageIndex, int pageSize)
        {
            var query = _context.Posts.AsQueryable();
            var where = LinqExtensions.True<Post>();
            where = where.And(p => p.CreateBy == userId);
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

        public async Task<PageListResult<Collect>> MyCollects(int userId, int pageIndex, int pageSize)
        {
            var query = _context.Collects.AsQueryable();
            var where = LinqExtensions.True<Collect>();
            where = where.And(p => p.UserId == userId);
            query = query.Where(where);
            var count = await query.CountAsync();
            var source = await query.Include(t => t.Topic)
                .OrderByDescending(t => t.Id).Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            var data = _mapper.Map<List<Collect>>(source);
            return ResultUtil.PageList(count, pageIndex, pageSize, data);
        }
    }
}
