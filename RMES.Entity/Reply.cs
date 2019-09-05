using System;

namespace RMES.Entity
{
    public class Reply
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 主题ID
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// 主贴ID
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// 回复对象ID
        /// </summary>
        public int TargetUserId { get; set; }

        /// <summary>
        /// 回复的内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
