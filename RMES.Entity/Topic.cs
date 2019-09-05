using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMES.Entity
{
    public class Topic
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 板块ID
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 主题类型：qa问答，normal普通帖子
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 主题标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 主贴的图片
        /// </summary>
        public string Pics { get; set; }

        /// <summary>
        /// 跟帖数量
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 被收藏的次数
        /// </summary>
        public int CollectCount { get; set; }

        /// <summary>
        /// 是否已解决
        /// </summary>
        public bool IsResolved { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后回帖人ID
        /// </summary>
        public int? LastCommentBy { get; set; }

        /// <summary>
        /// 最后回帖人
        /// </summary>
        public string LastCommenter { get; set; }

        /// <summary>
        /// 最后回帖时间
        /// </summary>
        public DateTime? LastCommentAt { get; set; }

        /// <summary>
        /// 主题下的所有帖子
        /// </summary>
        public List<Post> Posts { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [ForeignKey("CreateBy")]
        public User Creator { get; set; }
    }
}
