using System;

namespace RMES.Entity
{
    /// <summary>
    /// 收藏
    /// </summary>
    public class Collect
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 收藏人ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 收藏的文章ID
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime CollectAt { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public Topic Topic { get; set; }
    }
}
