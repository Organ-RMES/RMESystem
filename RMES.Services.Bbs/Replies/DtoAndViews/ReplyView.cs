using System;

namespace RMES.Services.Bbs
{
    /// <summary>
    /// 回复视图
    /// </summary>
    public class ReplyView
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建人昵称
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 回复目标的ID
        /// </summary>
        public int TargetUserId { get; set; }

        /// <summary>
        /// 回复目标的昵称
        /// </summary>
        public string TargetUserName { get; set; }

        /// <summary>
        /// 回复的内容
        /// </summary>
        public string Contents { get; set; }

        /// <summary>
        /// 回复的时间
        /// </summary>
        public string CreateAt { get; set; }
    }
}
