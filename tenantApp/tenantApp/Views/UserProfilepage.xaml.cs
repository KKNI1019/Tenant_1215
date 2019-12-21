using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Views;
using tenantApp.Webservices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserProfilepage : ContentPage
	{
        

		public UserProfilepage ()
		{
			InitializeComponent ();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnProfilePageRefresh", (sender) => {
                OnAppearing();
            });

            lbl_editProfile.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => EditProfile_Clicked()),
            });

            lbl_QA.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ToQAPage()),
            });

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnNoticeCreated", (sender) => {
                lbl_name.Text = App.tenant_name;
                lbl_nickname.Text = App.tenant_nickname;
                lbl_email.Text = App.tenant_email;
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            lbl_name.Text = App.tenant_name;
            lbl_nickname.Text = App.tenant_nickname;
            lbl_email.Text = App.tenant_email;

            if (App.tenant_profile != null)
            {
                imgProfile.Source = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + App.tenant_profile;
            }
        }

        private async void EditProfile_Clicked()
        {
            await Navigation.PushAsync(new UserInfoUpdatePage());
        }

        private async void ToQAPage()
        {
            await Navigation.PushAsync(new QuizPage());
        }
    }
}