using Plugin.Badge.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
using tenantApp.Webservices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TabPage : TabbedPage
	{
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public TabPage ()
		{
			InitializeComponent ();

            Preferences.Set(Constants.REGISTERED, true);
            Xamarin.Forms.PlatformConfiguration.AndroidSpecific.TabbedPage.SetIsSwipePagingEnabled(this, false);

            showBadge();

            MessagingCenter.Subscribe<App>((App)Application.Current, "BadgeCountRefresh", (sender) => {
                showBadge();
            });

            //this.CurrentPageChanged += (object sender, EventArgs e) => {
            //    var i = this.Children.IndexOf(this.CurrentPage);
            //    var masterPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1] as TabbedPage;

            //    //NoticeTab.Icon = "notification.png";

            //    switch (i)
            //    {
            //        case 0:
            //        case 1:
            //        case 2:
            //        case 3: break;
            //    }
            //};
        }

        private void showBadge()
        {
            string strBadgeNum;
            int badgeNum = Preferences.Get("badgeNum", 0);

            if (badgeNum > 0)
            {
                //strBadgeNum = badgeNum.ToString();
                strBadgeNum = "1";
            }
            else
            {
                strBadgeNum = string.Empty;
            }

            TabBadge.SetBadgeText(NoticeTab, strBadgeNum);
        }
    }
}