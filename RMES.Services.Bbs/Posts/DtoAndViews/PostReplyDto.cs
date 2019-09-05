namespace RMES.Services.Bbs
{
    /// <summary>
    /// 回复帖子实体
    /// </summary>
    public class PostReplyDto
    {
        public int TargetUserId { get; set; }

        public string Contents { get; set; }
    }
}
