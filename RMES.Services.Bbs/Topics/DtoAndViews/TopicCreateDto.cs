using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "所属板块不能为空")]
        public int ChannelId { get; set; }

        /// <summary>
        /// 主题的标题
        /// </summary>
        [Required(ErrorMessage = "主题名称不能为空", AllowEmptyStrings = false)]
        public string Title { get; set; }

        /// <summary>
        /// 主帖内容
        /// </summary>
        [Required(ErrorMessage = "主帖内容不能为空", AllowEmptyStrings = false)]
        public string Contents { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string Pics { get; set; }

        /// <summary>
        /// 主贴类型，normal 普通，qa 问答
        /// </summary>
        public string Type { get; set; } = "normal";
    }
}
