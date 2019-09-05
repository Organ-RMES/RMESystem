using System;

namespace RMES.Entity
{
    public class User
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pw { get; set; }

        /// <summary>
        /// 盐
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateAt { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 主题数
        /// </summary>
        public int TopicCount { get; set; }

        /// <summary>
        /// 回帖数
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 关注我的人的数量
        /// </summary>
        public int FansCount { get; set; }

        /// <summary>
        /// 我关注的人的数量
        /// </summary>
        public int AttentionCount { get; set; }
    }
}
