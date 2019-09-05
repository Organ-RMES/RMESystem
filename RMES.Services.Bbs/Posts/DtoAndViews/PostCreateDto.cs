using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "所属主题不能为空")]
        public int TopicId { get; set; }

        /// <summary>
        /// 回帖内容
        /// </summary>
        [Required(ErrorMessage = "回帖内容不能为空", AllowEmptyStrings = false)]
        public string Contents { get; set; }
    }
}
