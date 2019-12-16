using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Webservices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
    
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserInfoConfPage : ContentPage
	{
       
        public UserInfoConfPage ()
		{
			InitializeComponent ();

            lbl_familyname.Text = App.tenant_family_name;
            lbl_name.Text = App.tenant_first_name;
            lbl_phonenum.Text = App.tenant_phone1;
            lbl_birthday.Text = App.tenant_birthday;
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
                        new KeyValuePair<string, string>("tenant_phone1", App.tenant_phone1),
                        new KeyValuePair<string, string>("tenant_birthday", App.tenant_birthday)
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

                        await Navigation.PushAsync(new CaptureCertiPage());
                    }
                    else
                    {
                        loadingbar.IsRunning = false;

                        await DisplayAlert("", resultMsg.resp, "はい");
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