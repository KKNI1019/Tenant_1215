
using Plugin.FirebasePushNotification;
using System;
using System.IO;
using tenantApp.Data;
using tenantApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace tenantApp
{
    public partial class App : Application
    {
        public static string endpoint = "http://133.18.218.237:5000/real_estate_management/";

        public static string tenant_ID { get; set; }
        public static string tenant_name { get; set; }
        public static string tenant_kana { get; set; }
        public static string tenant_nickname { get; set; }
        public static string tenant_email { get; set; }
        public static string tenant_password { get; set; }
        public static string tenant_phone1 { get; set; }
        public static string tenant_phone2 { get; set; }
        public static string tenant_expect_last_day { get; set; }
        public static string tenant_real_last_day { get; set; }
        public static string tenant_memo { get; set; }
        public static string tenant_birthday { get; set; }
        public static string tenant_profile { get; set; }

        public static string tenant_family_name { get; set; }
        public static string tenant_first_name { get; set; }

        public static string last_news_id { get; set; }
        public static string new_tenant_id { get; set; }

        public static string estate_id;
        public static string estate_name;
        public static string estate_address;
        public static string estate_room_number;
        public static string estate_rent;
        public static string estate_owner_id;
        public static string estate_sale_status;
        public static string estate_zero_status;
        public static string estate_memo;
        public static string estate_image_url;

        public static string estate_renewal_period;
        public static DateTime estate_deadline;
        public static DateTime estate_next_renewal_month;

        static ThreadData thread_data;
        static ColumnData column_data;
        static NoticeData notice_data;
        static Th_CommentData th_comment_data;
        public static FavoriteNewsDB Favorite_Data;
        public static QADatabase qa_data;

        public static QADatabase QA_Data
        {
            get
            {
                if (qa_data == null)
                {
                    qa_data = new QADatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tenantdb.db"));
                }
                return qa_data;
            }
        }

        public static ThreadData Th_Data
        {
            get
            {
                if (thread_data == null)
                {
                    thread_data = new ThreadData(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tenantdb.db"));
                }
                return thread_data;
            }
        }

        public static Th_CommentData Th_CommentData
        {
            get
            {
                if (th_comment_data == null)
                {
                    th_comment_data = new Th_CommentData(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tenantdb.db"));
                }
                return th_comment_data;
            }
        }

        public static ColumnData Col_Data
        {
            get
            {
                if (column_data == null)
                {
                    column_data = new ColumnData(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tenantdb.db"));
                }
                return column_data;
            }
        }

        public static NoticeData Noti_Data
        {
            get
            {
                if (notice_data == null)
                {
                    notice_data = new NoticeData(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tenantdb.db"));
                }
                return notice_data;
            }
        }

        public static FavoriteNewsDB Favorite_News_Data
        {
            get
            {
                if (Favorite_Data == null)
                {
                    Favorite_Data = new FavoriteNewsDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "tenantdb.db"));
                }
                return Favorite_Data;
            }
        }

        public App()
        {
            InitializeComponent();
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5OTIzQDMxMzcyZTMyMmUzMGhGNUZBVnh1T3RBVFlBT0xpeHlrT01rRVRNSk1RMFgxbENSZlJWc0FxbjQ9");

            MainPage = new NavigationPage(new BlankPage());

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Received");
            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }
                }
            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Deleted");
            };

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static void increaseBadgeNum()
        {
            int badgeNum = Preferences.Get("badgeNum", 0);
            badgeNum++;
            Preferences.Set("badgeNum", badgeNum);

            MessagingCenter.Send<App>((App)Application.Current, "BadgeCountRefresh");
        }


    }
}
