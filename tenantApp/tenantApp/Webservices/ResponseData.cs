using System;
using System.Collections.Generic;
using System.Text;

namespace tenantApp.Webservices
{
    public class ResponseData
    {
        public tenant_data tenant_data { get; set; }
        public estate_data estate_data { get; set; }
        public thread_list_data[] thread_list_data { get; set; }
        public comment_list_data[] comment_list_data { get; set; }
        public notice_list_data[] notice_list_data { get; set; }
        public thread_comment_data[] thread_comment_data { get; set; }
        public news_data[] news_data { get; set; }
    }

    public class estate_data
    {
        public string estate_id;
        public string estate_name;
        public string estate_address;
        public string estate_room_number;
        public string estate_rent;
        public string estate_owner_id;
        public string estate_sale_status;
        public string estate_zero_status;
        public string estate_memo;
        public string estate_renewal_period;
        public DateTime estate_deadline;
        public DateTime estate_next_renewal_month;
        public string estate_image_url;
    }

    public class tenant_data
    {
        public string tenant_id;
        public string tenant_name;
        public string tenant_kana;
        public string tenant_nickname;
        public string tenant_email;
        public string tenant_password;
        public string tenant_phone1;
        public string tenant_phone2;
        public string tenant_expect_last_day;
        public string tenant_real_last_day;
        public string tenant_memo;
        public string tenant_birthday;
        public string tenant_profile;
    }

    public class thread_list_data
    {
        public string thread_id;
        public string thread_category;
        public string thread_note;
        public DateTime u_date;
    }

    public class comment_list_data
    {
        public string comment_id;
        public string comment_title;
        public string comment_writer_user_name;
        public string comment_contents;
        public DateTime u_date;
    }

    public class notice_list_data
    {
        public string notice_id;
        public string notice_title;
        public string notice_contents;
        public DateTime u_date;
    }

    public class thread_comment_data
    {
        public string thread_comment_id;
        public string thread_comment_contents;
        public string thread_comment_writer_nickname;
        public string thread_comment_category;
        public DateTime c_date;
    }

    public class news_data
    {
        public string news_id { get; set; }
        public string news_image_url { get; set; }
        public string news_category { get; set; }
        public string news_title { get; set; }
        public DateTime news_date { get; set; }
        public string news_source { get; set; }
        public string news_writer_image { get; set; }
        public string news_writer_name { get; set; }
        public string news_writer_profile { get; set; }
        public int comment_count { get; set; }
        public string news_url { get; set; }
        public string news_contents { get; set; }
        public news_comment_data[] news_comment_data { get; set; }
    }

    public class news_comment_data
    {
        public string news_comment_id;
        public string news_comment_writer_image;
        public string news_comment_writer_name;
        public string news_comment_writer_profile;
        public string news_comment_contents;
        public string news_comment_likes;
        public DateTime c_date;
    }
}
