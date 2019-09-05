using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMES.Entity
{
    /// <summary>
    /// 文章
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 主题ID
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// 是否是主贴
        /// </summary>
        public bool IsMaster { get; set; } = false;

        /// <summary>
        /// 内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 回复数量
        /// </summary>
        public int ReplyCount { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// 踩数量
        /// </summary>
        public int DislikeCount { get; set; }

        /// <summary>
        /// 是否被采纳
        /// </summary>
        public bool IsAccept { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 发帖人
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 所属主题
        /// </summary>
        public Topic Topic { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [ForeignKey("CreateBy")]
        public User Creator { get; set; }
    }
}
