namespace RMES.Entity
{
    public class LikeLog
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
        /// 帖子ID
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 值，0不赞不踩，1赞，2踩
        /// </summary>
        public int Value { get; set; }
    }
}
