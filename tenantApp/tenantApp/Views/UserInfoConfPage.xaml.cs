using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Webservices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
    
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserInfoConfPage : ContentPage
	{
        private string device_token;

        public UserInfoConfPage ()
		{
			InitializeComponent ();

            lbl_familyname.Text = App.tenant_family_name;
            lbl_name.Text = App.tenant_first_name;
            lbl_pwd.Text = App.tenant_password;
            lbl_email.Text = App.tenant_email;

            device_token = Preferences.Get("device_token", "");
        }

        private async void Btn_changeInfo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void ImgBtn_Reg_Clicked(object sender, EventArgs e)
        {
            getResult();
            //await Navigation.PushAsync(new CaptureCertiPage());
        }

        private async void getResult()
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("tenant_name", App.tenant_name),
                    new KeyValuePair<string, string>("tenant_email", App.tenant_email),
                    new KeyValuePair<string, string>("tenant_password", App.tenant_password),
                    new KeyValuePair<string, string>("device_token", device_token)
                });
                try
                {
                    var request = await cl.PostAsync(App.endpoint + "api/user/regist", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);
                    
                    if (resultMsg.resp.Equals("success") || resultMsg.resp.Equals("half"))
                    {
                        loadingbar.IsRunning = false;

                        App.tenant_ID = resultMsg.tenant_id;

                        Preferences.Set(Constants.TENANT_EMAIL, App.tenant_email);
                        Preferences.Set(Constants.TENANT_PWD, App.tenant_password);
                        await Navigation.PushAsync(new ConfirmEmailPage(App.tenant_email));
                    }
                    else
                    {
                        loadingbar.IsRunning = false;

                        await DisplayAlert("", resultMsg.message, "はい");
                    }
                }
                catch
                {
                    loadingbar.IsRunning = false;

                    await DisplayAlert("", "サーバー接続でエラーが発生しました。", "はい");
                }

            }
        }

        private async void stkBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}