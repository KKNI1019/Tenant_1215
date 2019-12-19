using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace tenantApp.Models
{
    public class QAList
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string question_id { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public bool img_visibility { get; set; }
        public DateTime c_date { get; set; }
    }
}
