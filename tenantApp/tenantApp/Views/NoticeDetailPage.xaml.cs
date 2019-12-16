using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoticeDetailPage : ContentPage
	{
		public NoticeDetailPage ()
		{
			InitializeComponent ();
		}

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            removeNew();

            await Navigation.PopAsync();
        }

        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        protected override bool OnBackButtonPressed()
        {
            removeNew();

            return base.OnBackButtonPressed();
        }

        private async void removeNew()
        {
            Notifications notification = this.BindingContext as Notifications;
            if (notification.IsVisible)
            {
                int badgeNum = Preferences.Get("badgeNum", 0);
                badgeNum--;
                Preferences.Set("badgeNum", badgeNum);

                MessagingCenter.Send<App>((App)Application.Current, "BadgeCountRefresh");

                notification.IsVisible = false;
                await App.Noti_Data.SaveNotiAsync(notification);
            }
        }
    }
}