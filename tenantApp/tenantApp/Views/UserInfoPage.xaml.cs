using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
using tenantApp.Webservices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserInfoPage : ContentPage
	{

        public UserInfoPage ()
		{
			InitializeComponent ();
        }
        

        private async void ImgBtn_confirm_Clicked(object sender, EventArgs e)
        {
            App.tenant_first_name = ent_name.Text;
            App.tenant_family_name = ent_familyname.Text;
            App.tenant_name = App.tenant_family_name + App.tenant_first_name;
            App.tenant_birthday = ent_birthyear.Text + "/" + ent_birthmonth.Text + "/" + ent_birthdate.Text;
            App.tenant_phone1 = ent_phoneNum1.Text;

            if (string.IsNullOrWhiteSpace(App.tenant_first_name) || string.IsNullOrWhiteSpace(App.tenant_family_name) || App.tenant_birthday == "//" || string.IsNullOrWhiteSpace(App.tenant_phone1) )
            {
                await DisplayAlert("", "詳細情報を正確に入力してください。", "はい");
            }
            else
            {                
                await Navigation.PushAsync(new UserInfoConfPage());
            }
        }

        private async void stkBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}