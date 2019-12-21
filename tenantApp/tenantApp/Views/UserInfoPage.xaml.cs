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
using Xamarin.Essentials;
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

            label_rule.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => OnLabelClicked()),
            });
        }

        private async void OnLabelClicked()
        {
            await Navigation.PushAsync(new RulePage());
        }


        private async void ImgBtn_confirm_Clicked(object sender, EventArgs e)
        {
            App.tenant_first_name = ent_name.Text;
            App.tenant_family_name = ent_familyname.Text;
            App.tenant_name = App.tenant_family_name + App.tenant_first_name;
            App.tenant_email = ent_email.Text;
            App.tenant_password = ent_pwd.Text;

            if (string.IsNullOrWhiteSpace(App.tenant_first_name) || string.IsNullOrWhiteSpace(App.tenant_family_name))
            {
                await DisplayAlert("", "名前フィールドは必須です。", "はい");
            }
            else if (string.IsNullOrWhiteSpace(ent_email.Text))
            {
                await DisplayAlert("", "メールアドレスフィールドは必須です。", "はい");
            }
            else if (string.IsNullOrWhiteSpace(ent_pwd.Text))
            {
                await DisplayAlert("", "パスワードフィールドは必須です。", "はい");
            }
            else if (string.IsNullOrWhiteSpace(ent_pwd_again.Text))
            {
                await DisplayAlert("", "パスワード再入力を入力してください。", "はい");
            }
            else if (!string.Equals(ent_pwd.Text, ent_pwd_again.Text))
            {
                await DisplayAlert("", "パスワードとパスワード再入力が一致しません。", "はい");
            }
            else if (imgBtn_checked.IsVisible == false)
            {
                await DisplayAlert("", "利用規約を確認してください。", "はい");
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

        private void uncheckedBtn_Clicked(object sender, EventArgs e)
        {
            imgBtn_unchecked.IsVisible = false;
            imgBtn_checked.IsVisible = true;
        }

        private void checkedBtn_Clicked(object sender, EventArgs e)
        {
            imgBtn_checked.IsVisible = false;
            imgBtn_unchecked.IsVisible = true;
        }
    }
}