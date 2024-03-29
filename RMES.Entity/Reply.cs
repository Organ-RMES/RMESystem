﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        [ForeignKey("CreateBy")]
        public User Creator { get; set; }

        /// <summary>
        /// 回复对象
        /// </summary>
        [ForeignKey("TargetUserId")]
        public User TargetUser { get; set; }

        public Post Post { get; set; }
    }
}
