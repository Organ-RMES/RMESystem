﻿namespace RMES.Services.Bbs
{
    /// <summary>
    /// 主题列表视图
    /// </summary>
    public class TopicListView
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 主题标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 回帖数量
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Pics { get; set; }

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public string UpdateAt { get; set; }

        /// <summary>
        /// 作者的昵称
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 最后一次回帖人
        /// </summary>
        public string LastCommenter { get; set; }

        /// <summary>
        /// 最后一次回帖时间
        /// </summary>
        public string LastCommentAt { get; set; }
    }
}
