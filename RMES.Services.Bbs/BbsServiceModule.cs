using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace RMES.Services.Bbs
{
    public static class BbsServiceModule
    {
        public static void AddBbsServiceModule(this IServiceCollection services)
        {
            services.AddTransient<ReplyService>();
            services.AddTransient<BbsUserCenterServices>();
            services.AddTransient<PostService>();
            services.AddTransient<TopicService>();
            services.AddTransient<UserService>();
        }
    }
}
