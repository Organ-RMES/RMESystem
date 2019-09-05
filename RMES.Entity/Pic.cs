using System;

namespace RMES.Entity
{
    public class Pic
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 文章ID
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// 原始文件名
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// 上传后的文件名
        /// </summary>
        public string NewName { get; set; }

        /// <summary>
        /// 文章描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 图片标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 图片保存路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 上传人ID
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 上传人
        /// </summary>
        public string Creator { get; set; }
    }
}
