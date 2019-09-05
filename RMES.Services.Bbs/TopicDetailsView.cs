using System;
using System.Collections.Generic;
using System.Text;

namespace RMES.Services.Bbs
{
    public class TopicDetailsView
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<PostView> Posts { get; set; }
    }
}
