using System;
using System.Collections.Generic;
using System.Text;

namespace tenantApp.Webservices
{
    public class ResponseMsg
    {
        public string resp { get; set; }

        public string tenant_id { get; set; }

        public string message { get; set; }

        public string thread_comment_id { get; set; }

        public int comment_likes { get; set; }

        public string tenant_profile { get; set; }

        public string tenant_video { get; set; }
    }
}
