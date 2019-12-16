using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace tenantApp.Models
{
    public class Th_comments
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Th_id { get; set; }
        public string Th_comment_id { get; set; }
        public string Th_comment_content { get; set; }
        public string Th_comment_writer_nickname { get; set; }
        public DateTime c_date { get; set; }
        public string img_url { get; set; }
    }
}
