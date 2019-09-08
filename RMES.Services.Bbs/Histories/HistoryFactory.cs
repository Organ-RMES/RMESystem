using RMES.Entity;

namespace RMES.Services.Bbs
{
    /// <summary>
    /// 操作记录帮助类，返回操作记录实体
    /// </summary>
    public class HistoryFactory
    {
        public static History Visit(int topicId, string title, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                Type = HistoryTypes.Visit,
                Contents = title
            };
        }

        public static History Like(int topicId, int postId, string title, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                PostId = postId,
                Contents = title,
                Type = HistoryTypes.Like
            };
        }

        public static History UndoLike(int topicId, int postId, string title, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                PostId = postId,
                Contents = title,
                Type = HistoryTypes.UndoLike
            };
        }

        public static History Dislike(int topicId, int postId, string title, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                PostId = postId,
                Contents = title,
                Type = HistoryTypes.Dislike
            };
        }

        public static History UndoDislike(int topicId, int postId, string title, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                PostId = postId,
                Contents = title,
                Type = HistoryTypes.UndoDislike
            };
        }

        public static History Comment(int topicId, string title, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                Contents = title,
                Type = HistoryTypes.Comment
            };
        }

        public static History DeleteComment(int topicId, string title, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                Contents = title,
                Type = HistoryTypes.DeleteComment
            };
        }

        public static History Reply(int topicId, int postId, int targetUserId, string targetUserName, string content, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                TargetUserId = targetUserId,
                PostId = postId,
                Contents = $"回复了 {targetUserName}，内容：{content}",
                Type = HistoryTypes.Reply
            };
        }

        public static History DeleteReply(int topicId, int postId, int targetUserId, string targetUserName, string content, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                TargetUserId = targetUserId,
                PostId = postId,
                Contents = $"回复了 {targetUserName}，内容：{content}",
                Type = HistoryTypes.DeleteReply
            };
        }

        public static History Follow(int targetUserId, string targetUserName, int userId)
        {
            return new History
            {
                UserId = userId,
                TargetUserId = targetUserId,
                Contents = $"关注了用户 {targetUserId}",
                Type = HistoryTypes.Follow
            };
        }

        public static History UnFollow(int targetUserId, string targetUserName, int userId)
        {
            return new History
            {
                UserId = userId,
                TargetUserId = targetUserId,
                Contents = $"取消关注用户 {targetUserId}",
                Type = HistoryTypes.UnFollow
            };
        }

        public static History Collect(int topicId, string title, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                Contents = $"关注了主题 {title}",
                Type = HistoryTypes.Collect
            };
        }

        public static History UnCollect(int topicId, string title, int userId)
        {
            return new History
            {
                UserId = userId,
                TopicId = topicId,
                Contents = $"取消关注主题 {title}",
                Type = HistoryTypes.UnCollect
            };
        }

        public static History SignIn(int userId)
        {
            return new History
            {
                UserId = userId,
                Contents = "签到",
                Type = HistoryTypes.SignIn
            };
        }
    }
}
