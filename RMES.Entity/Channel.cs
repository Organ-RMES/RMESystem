namespace RMES.Entity
{
    /// <summary>
    /// 论坛版块
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 版块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 版块描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 版块点击次数
        /// </summary>
        public int ClickCount { get; set; }

        /// <summary>
        /// 版块帖子数量
        /// </summary>
        public int PostCount { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
