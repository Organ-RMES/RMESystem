using RMES.Framework;
using System.Collections.Generic;

namespace RMES.Services.Bbs
{
    public interface ITopicService
    {
        /// <summary>
        /// 分页获取主题下的帖子
        /// </summary>
        /// <param name="topicId">主题ID</param>
        /// <param name="pageIndex">页码</param>
        /// <returns></returns>
        PageListResult<PostView> GetPostsByPage(int topicId, int pageIndex);

        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        PageListResult<TopicListView> Get(int pageIndex);

        /// <summary>
        /// 获取最热主题列表
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Result<List<TopicListView>> GetHotTopics(int count);

        /// <summary>
        /// 获取最新主题列表
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Result<List<TopicListView>> GetNewestTopics(int count);

        /// <summary>
        /// 获取最新评论列表
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        Result<List<TopicListView>> GetNewestComments(int count);

        /// <summary>
        /// 创建主题
        /// </summary>
        /// <param name="topicCreateDto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Result Create(TopicCreateDto topicCreateDto, AppUser user);

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Result Delete(int id, AppUser user);
    }
}
