using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RMES.EF;
using RMES.Framework;
using System.Threading.Tasks;

namespace RMES.Services.Bbs
{
    public class ReplyService
    {
        private readonly RmesContext _context;
        private readonly IMapper _mapper;

        public ReplyService(RmesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Delete(int id, AppUser user)
        {
            var entity = await _context.Replies.Include(r => r.Post).FirstOrDefaultAsync(r => r.Id == id);

            if (entity == null)
            {
                return ResultUtil.BadRequest("请求的数据不存在或已删除");
            }

            if (entity.CreateBy != user.Id)
            {
                return ResultUtil.BadRequest("您无权删除此数据");
            }

            entity.IsDel = true;
            entity.Post.ReplyCount--;

            var history = HistoryFactory.DeleteReply(entity.TopicId, entity.PostId, entity.TargetUserId, entity.TargetUser?.NickName ?? "",
                "删除了回复", entity.CreateBy);
            _context.Add(history);

            await _context.SaveChangesAsync();
            return ResultUtil.Ok();
        }
    }
}
