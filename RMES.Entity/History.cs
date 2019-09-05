using System;

namespace RMES.Entity
{
    /// <summary>
    /// 操作记录
    /// </summary>
    public class History
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 主题ID
        /// </summary>
        public int? TopicId { get; set; }

        /// <summary>
        /// 文章ID
        /// </summary>
        public int? PostId { get; set; }

        /// <summary>
        /// 目标用用户ID
        /// </summary>
        public int? TargetUserId { get; set; }

        /// <summary>
        /// 操作类型，1浏览帖子2点赞3踩4回帖5回复6关注用户7取消关注用户8签到9收藏帖子10取消收藏帖子
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 操作详情
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
