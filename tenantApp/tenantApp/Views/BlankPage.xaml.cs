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

namespace tenantApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlankPage : ContentPage
	{
		public BlankPage ()
		{
			InitializeComponent ();

            loadingbar.IsRunning = true;
            loadingbar.IsEnabled = true;

            bool video_checked = Preferences.Get("video_checked", false);
            if (!video_checked)
            {
                ToVideoPage();
            }
            else
            {
                if (!Preferences.Get(Constants.REGISTERED, false))
                {
                    if (string.IsNullOrEmpty(Preferences.Get(Constants.TENANT_EMAIL, "")))
                    {
                        ToQRChecking();
                    }
                    else
                    {
                        ToLogin();
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(Preferences.Get(Constants.TENANT_REGIST_FROM, "")))
                    {
                        login();
                    }
                    else
                    {
                        social_login();
                    }
                }
            }
        }

        private async void ToVideoPage()
        {
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("", "")
                });

                try
                {
                    var request = await cl.PostAsync(Constants.SERVER_VIDEO_URL, formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;
                        Preferences.Set("tenant_video", resultMsg.tenant_video);

                        if (string.IsNullOrEmpty(resultMsg.tenant_video))
                        {
                            if (Preferences.Get(Constants.REGISTERED, false))
                            {
                                if (string.IsNullOrEmpty(Preferences.Get(Constants.TENANT_REGIST_FROM, "")))
                                {
                                    login();
                                }
                                else
                                {
                                    social_login();
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(Preferences.Get(Constants.TENANT_EMAIL, "")))
                                {
                                    ToQRChecking();
                                }
                                else
                                {
                                    ToLogin();
                                }
                            }
                        }
                        else
                        {
                            await Navigation.PushAsync(new VideoPage());
                        }
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
                    await DisplayAlert("", Constants.NETWORK_ERROR, "はい");
                }
            }
        }

        private async void ToQRChecking()
        {
            await Navigation.PushAsync(new QR_checking());
            Navigation.RemovePage(this);
        }

        private async void ToLogin()
        {
            await Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(this);
        }

        private async void login()
        {
            loadingbar.IsRunning = true;
            var dd = Preferences.Get(Constants.TENANT_EMAIL, "");
            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("tenant_email", Preferences.Get(Constants.TENANT_EMAIL,"")),
                        new KeyValuePair<string, string>("tenant_password", Preferences.Get(Constants.TENANT_PWD,"")),
                        new KeyValuePair<string, string>("device_token", Preferences.Get(Constants.DEVICE_TOKEN, ""))
                });

                try
                {
                    var request = await cl.PostAsync(App.endpoint + "api/user/login", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);
                        

                        App.tenant_ID = resultData.tenant_data.tenant_id;
                        App.tenant_name = resultData.tenant_data.tenant_name;
                        App.tenant_kana = resultData.tenant_data.tenant_kana;
                        App.tenant_nickname = resultData.tenant_data.tenant_nickname;
                        App.tenant_phone1 = resultData.tenant_data.tenant_phone1;
                        App.tenant_phone2 = resultData.tenant_data.tenant_phone2;
                        App.tenant_email = resultData.tenant_data.tenant_email;
                        App.tenant_password = resultData.tenant_data.tenant_password;
                        App.tenant_expect_last_day = resultData.tenant_data.tenant_expect_last_day;
                        App.tenant_real_last_day = resultData.tenant_data.tenant_real_last_day;
                        App.tenant_memo = resultData.tenant_data.tenant_memo;
                        App.tenant_birthday = resultData.tenant_data.tenant_birthday;

                        if (resultData.estate_data != null)
                        {
                            App.estate_rent = resultData.estate_data.estate_rent;
                            App.estate_next_renewal_month = resultData.estate_data.estate_next_renewal_month;
                            App.estate_renewal_period = resultData.estate_data.estate_renewal_period;
                            App.estate_deadline = resultData.estate_data.estate_deadline;
                        }
                        App.tenant_profile = resultData.tenant_data.tenant_profile;

                        await Navigation.PushAsync(new TabPage());

                        Navigation.RemovePage(this);
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

        private async void social_login()
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("tenant_email", Preferences.Get(Constants.TENANT_EMAIL,"")),
                        new KeyValuePair<string, string>("tenant_name", Preferences.Get(Constants.TENANT_NAME,"")),
                        new KeyValuePair<string, string>("tenant_regist_from", Preferences.Get(Constants.TENANT_REGIST_FROM,"")),
                        new KeyValuePair<string, string>("device_token", Preferences.Get(Constants.DEVICE_TOKEN,""))
                });

                try
                {
                    var request = await cl.PostAsync(App.endpoint + "api/user/social", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        await Navigation.PushAsync(new TabPage());

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        App.tenant_ID = resultData.tenant_data.tenant_id;
                        App.tenant_name = resultData.tenant_data.tenant_name;
                        App.tenant_kana = resultData.tenant_data.tenant_kana;
                        App.tenant_nickname = resultData.tenant_data.tenant_nickname;
                        App.tenant_phone1 = resultData.tenant_data.tenant_phone1;
                        App.tenant_phone2 = resultData.tenant_data.tenant_phone2;
                        App.tenant_email = resultData.tenant_data.tenant_email;
                        App.tenant_password = resultData.tenant_data.tenant_password;
                        App.tenant_expect_last_day = resultData.tenant_data.tenant_expect_last_day;
                        App.tenant_real_last_day = resultData.tenant_data.tenant_real_last_day;
                        App.tenant_memo = resultData.tenant_data.tenant_memo;
                        App.tenant_birthday = resultData.tenant_data.tenant_birthday;

                        if (resultData.estate_data != null)
                        {
                            App.estate_rent = resultData.estate_data.estate_rent;
                            App.estate_next_renewal_month = resultData.estate_data.estate_next_renewal_month;
                            App.estate_renewal_period = resultData.estate_data.estate_renewal_period;
                            App.estate_deadline = resultData.estate_data.estate_deadline;
                        }
                        App.tenant_profile = resultData.tenant_data.tenant_profile;

                        Navigation.RemovePage(this);
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
    }
}