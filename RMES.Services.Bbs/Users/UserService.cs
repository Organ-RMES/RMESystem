using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMES.EF;
using RMES.Framework;
using RMES.Util;
using RMES.Entity;

namespace RMES.Services.Bbs
{
    public class UserService
    {
        private readonly RmesContext _context;

        public UserService(RmesContext context)
        {
            _context = context;
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
    }
}
