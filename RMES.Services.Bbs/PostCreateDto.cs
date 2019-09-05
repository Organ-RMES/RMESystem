namespace RMES.Services.Bbs
{
    /// <summary>
    /// 回帖实体
    /// </summary>
    public class PostCreateDto
    {
        /// <summary>
        /// 主题ID
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// 回帖内容
        /// </summary>
        public string Contents { get; set; }
    }
}
