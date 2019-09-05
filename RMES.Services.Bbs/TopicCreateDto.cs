namespace RMES.Services.Bbs
{
    /// <summary>
    /// 创建主题实体
    /// </summary>
    public class TopicCreateDto
    {
        /// <summary>
        /// 板块ID
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 主题的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 主帖内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 主贴类型，normal，qa
        /// </summary>
        public string Type { get; set; }
    }
}
