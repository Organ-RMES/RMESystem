using System;

namespace RMES.Portal.WebApi.Models
{
    public class SessionUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public string AccessToken { get; set; }

        public DateTime ExpireTime { get; set; }
    }
}
