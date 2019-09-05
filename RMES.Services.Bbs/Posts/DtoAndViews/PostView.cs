using System;

namespace RMES.Services.Bbs
{
    /// <summary>
    /// 帖子视图
    /// </summary>
    public class PostView
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 是不是主贴
        /// </summary>
        public bool IsMaster { get; set; }

        /// <summary>
        /// 帖子内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 是否被采纳
        /// </summary>
        public bool IsAccept { get; set; }

        /// <summary>
        /// 回复数量
        /// </summary>
        public int ReplyCount { get; set; }

        /// <summary>
        /// 赞的数量
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// 踩的数量
        /// </summary>
        public int DislikeCount { get; set; }

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime UpdateAt { get; set; }

        /// <summary>
        /// 作者ID
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 作者昵称
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 作者级别
        /// </summary>
        public int AuthorGrade { get; set; }

        /// <summary>
        /// 作者头像
        /// </summary>
        public string AuthorAvatar { get; set; }
    }
}
