using System;
using System.Collections.Generic;
using System.Text;

namespace RMES.Framework
{
    public class ResultUtil
    {
        public static Result Do(int code, string message)
        {
            return new Result {Code = code, Message = message};
        }

        public static Result<T> Do<T>(int code, string message, T body)
        {
            return new Result<T> {Code = code, Message = message, Body = body};

        }

        public static Result Ok(string message = "操作成功")
        {
            return Do(ResultCodes.Ok, message);
        }

        public static Result<T> Ok<T>(T body, string message = "操作成功")
        {
            return Do(ResultCodes.Ok, message, body);
        }

        public static Result DbFail(string message = "操作失败")
        {
            return Do(ResultCodes.DbFail, message);
        }

        public static Result NotFound(string message = "请求的数据不存在")
        {
            return Do(ResultCodes.NotFound, message);
        }

        public static Result BadRequest(string message = "无效请求")
        {
            return Do(ResultCodes.BadRequest, message);
        }

        public static Result UnAuthentication(string message = "您尚未登录")
        {
            return Do(ResultCodes.UnAuthentication, message);
        }

        public static Result UnAuthorization(string message = "您已登录，但无权使用此功能")
        {
            return Do(ResultCodes.UnAuthorization, message);
        }

        public static Result Exception(Exception ex)
        {
            return Do(ResultCodes.Exception, ex.Message);
        }

        public static Result Exception(string message = "系统异常")
        {
            return Do(ResultCodes.Exception, message);
        }

        public static PageListResult<T> PageList<T>(int total, int pageIndex, int pageSize, IEnumerable<T> body)
        {
            return new PageListResult<T>
            {
                RecordCount = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Body = body
            };
        }
    }
}
