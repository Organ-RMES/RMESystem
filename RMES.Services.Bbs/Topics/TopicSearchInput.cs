using RMES.Entity;
using RMES.Services.Common;
using System;
using System.Linq.Expressions;

namespace RMES.Services.Bbs
{
    /// <summary>
    /// 主题的查询参数
    /// </summary>
    public class TopicSearchInput
    {
        /// <summary>
        /// 作者ID
        /// </summary>
        public int? Author { get; set; }

        /// <summary>
        /// 创建日期开始于
        /// </summary>
        public DateTime? CreateAtStart { get; set; }

        /// <summary>
        /// 创建日期结束于
        /// </summary>
        public DateTime? CreateAtEnd { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Key { get; set; }

        public Expression<Func<Topic, bool>> ToExpression()
        {
            var where = LinqExtensions.True<Topic>();

            if (Author.HasValue && Author.Value > 0)
            {
                where = where.And(t => t.CreateBy == Author.Value);
            }

            var date = DateTime.Parse("2019-1-1");

            if (CreateAtStart.HasValue && CreateAtStart.Value > date)
            {
                where = where.And(t => t.CreateAt >= CreateAtStart.Value);
            }

            if (CreateAtEnd.HasValue && CreateAtEnd.Value > date)
            {
                where = where.And(t => t.CreateAt <= CreateAtEnd);
            }

            if (!string.IsNullOrWhiteSpace(Key))
            {
                where = where.And(t => t.Title.Contains(Key.Trim()));
            }

            return where;
        }
    }
}
