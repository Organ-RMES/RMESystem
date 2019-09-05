namespace RMES.Services.Bbs
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public class HistoryTypes
    {
        /// <summary>
        /// 浏览帖子
        /// </summary>
        public static int Visit => 1;

        /// <summary>
        /// 点赞
        /// </summary>
        public static int Like => 2;

        /// <summary>
        /// 取消点赞
        /// </summary>
        public static int UndoLike => 11;

        /// <summary>
        /// 踩
        /// </summary>
        public static int Dislike => 3;

        /// <summary>
        /// 取消踩
        /// </summary>
        public static int UndoDislike => 12;

        /// <summary>
        /// 回帖
        /// </summary>
        public static int Comment => 4;

        /// <summary>
        /// 回复
        /// </summary>
        public static int Reply => 5;

        /// <summary>
        /// 关注某人
        /// </summary>
        public static int Follow => 6;

        /// <summary>
        /// 取消关注
        /// </summary>
        public static int UnFollow => 7;

        /// <summary>
        /// 签到
        /// </summary>
        public static int SignIn => 8;

        /// <summary>
        /// 收藏帖子
        /// </summary>
        public static int Collect => 9;

        /// <summary>
        /// 取消收藏
        /// </summary>
        public static int UnCollect => 10;
    }
}
