using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tenantApp.FaceBook;
using tenantApp.Line;
using tenantApp.Models;
using tenantApp.Webservices;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        private string device_token;

        public LoginPage ()
		{
			InitializeComponent ();

            device_token = Preferences.Get("device_token", "");

            if (string.IsNullOrEmpty(Preferences.Get(Constants.TENANT_REGIST_FROM, "")))
            {
                ent_email.Text = Preferences.Get(Constants.TENANT_EMAIL, "");
            }

            //label_rule.GestureRecognizers.Add(new TapGestureRecognizer
            //{
            //    Command = new Command(() => OnLabelClicked()),
            //});

            //CountBadgeNum();
        }

        private async void OnLabelClicked()
        {
            await Navigation.PushAsync(new RulePage());
        }

        private async void ImgBtn_Login_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ent_email.Text)){
                await DisplayAlert("", "メールアドレスフィールドは必須です。", "はい");
            }
            else if (string.IsNullOrWhiteSpace(ent_pwd.Text))
            {
                await DisplayAlert("", "パスワードフィールドは必須です。", "はい");
            }
            //else if (imgBtn_checked.IsVisible == false)
            //{
            //    await DisplayAlert("", "利用規約を確認してください。", "はい");
            //}
            else
            {
                loginInfoCheck();
                //await Navigation.PushAsync(new TabPage());
            }
        }

        private async void loginInfoCheck()
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("tenant_email", ent_email.Text),
                        new KeyValuePair<string, string>("tenant_password", ent_pwd.Text),
                        new KeyValuePair<string, string>("device_token", device_token)
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
                        Preferences.Set(Constants.TENANT_EMAIL, ent_email.Text);
                        Preferences.Set(Constants.TENANT_PWD, ent_pwd.Text);

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);
                        
                        //ent_email.Text = string.Empty;
                        ent_pwd.Text = string.Empty;
                        //imgBtn_checked.IsVisible = false;
                        //imgBtn_unchecked.IsVisible = true;

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
                        App.tenant_profile = resultData.tenant_data.tenant_profile;

                        if (resultData.estate_data != null)
                        {
                            App.estate_rent = resultData.estate_data.estate_rent;
                            App.estate_next_renewal_month = resultData.estate_data.estate_next_renewal_month;
                            App.estate_renewal_period = resultData.estate_data.estate_renewal_period;
                            App.estate_deadline = resultData.estate_data.estate_deadline;
                        }
                        await Navigation.PushAsync(new TabPage());
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

        //private async void ImgBtn_facebookLogin_Clicked(object sender, EventArgs e)
        //{
        //    //if (imgBtn_checked.IsVisible == false)
        //    //{
        //    //    await DisplayAlert("", "利用規約を確認してください。", "はい");
        //    //}
        //    //else
        //    //{
        //        OAuth2Base oAuth2 = null;
        //        oAuth2 = FacebookOAuth2.Instance;

        //        var authenticator = new OAuth2Authenticator(
        //                oAuth2.ClientId,
        //                oAuth2.ClientSecret,
        //                oAuth2.Scope,
        //                oAuth2.AuthorizationUri,
        //                oAuth2.RedirectUri,
        //                oAuth2.RequestTokenUri,
        //                null,
        //                oAuth2.IsUsingNativeUI);

        //        authenticator.Completed += async (s, ee) =>
        //        {
        //            if (ee.IsAuthenticated)
        //            {
        //                var user = await oAuth2.GetUserInfoAsync(ee.Account);
        //                string name = user.Name;
        //                string email = user.email;
        //                string Id = user.Id;

        //                App.tenant_name = name;
        //                App.tenant_email = email;

        //                RegisterResultCheck(name, email, "facebook_" + Id, device_token);

        //                //await Navigation.PushAsync(new TabPage());

        //                Debug.WriteLine("Authentication Success");
        //            }
        //        };
        //        authenticator.Error += (s, ee) =>
        //        {
        //            Debug.WriteLine("Authentication error: " + ee.Message);
        //        };

        //        var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
        //        presenter.Login(authenticator);
        //    //}
        //}

        private async void RegisterResultCheck(string name, string email, string method, string token)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("tenant_email", email),
                        new KeyValuePair<string, string>("tenant_name", name),
                        new KeyValuePair<string, string>("tenant_regist_from", method),
                        new KeyValuePair<string, string>("device_token", token)
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
                        Preferences.Set(Constants.TENANT_EMAIL, email);
                        Preferences.Set(Constants.TENANT_NAME, name);
                        Preferences.Set(Constants.TENANT_REGIST_FROM, method);

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

        public class UserDetails
        {
            public string name { get; set; }
            public string email { get; set; }
            public string Id { get; set; }
        }

        //private async void ImgBtn_twitterLogin_Clicked(object sender, EventArgs e)
        //{
        //    //if (imgBtn_checked.IsVisible == false)
        //    //{
        //    //    await DisplayAlert("", "利用規約を確認してください。", "はい");
        //    //}
        //    //else
        //    //{
        //        var Twitterauth = new OAuth1Authenticator(
        //                   consumerKey: "P4a4J57m7UEIGKh4p6LiFsmZa",
        //                   consumerSecret: "pAQRsnP0FFHDT6KVwvKxSPAVi9V2uCUnUGolgupB9QD1EMNbOj",
        //                   requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"),
        //                   authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"),
        //                   accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"),
        //                   callbackUrl: new Uri("https://www.facebook.com/connect/login_success.html")
        //     );

        //        Twitterauth.Completed += TwitterAuth_Completed;

        //        var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
        //        presenter.Login(Twitterauth);
        //    //}
        //}

        private async void TwitterAuth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("include_email", "true");
                var request = new OAuth1Request("GET",
                    new Uri("https://api.twitter.com/1.1/account/verify_credentials.json"),
                    param,
                    e.Account);
                try
                {
                    var response = await request.GetResponseAsync();
                    var json = response.GetResponseText();
                    var twitterUser = JsonConvert.DeserializeObject<UserDetails>(json);

                    string name = twitterUser.name;
                    string email = twitterUser.email;
                    string id = twitterUser.Id;

                    App.tenant_name = name;
                    App.tenant_email = email;

                    RegisterResultCheck(name, email, "twitter_" + id, device_token);

                    //await Navigation.PushAsync(new TabPage());
                }
                catch{ }
            }
        }

        //private async void ImgBtn_lineLogin_Clicked(object sender, EventArgs e)
        //{
        //    //if (imgBtn_checked.IsVisible == false)
        //    //{
        //    //    await DisplayAlert("", "利用規約を確認してください。", "はい");
        //    //}
        //    //else
        //    //{
        //        OAuth2Base oAuth2 = null;
        //        oAuth2 = LineOAuth2.Instance;

        //        var authenticator = new OAuth2Authenticator(
        //                oAuth2.ClientId,
        //                oAuth2.ClientSecret,
        //                oAuth2.Scope,
        //                oAuth2.AuthorizationUri,
        //                oAuth2.RedirectUri,
        //                oAuth2.RequestTokenUri,
        //                null,
        //                oAuth2.IsUsingNativeUI);

        //        authenticator.Completed += async (s, ee) =>
        //        {
        //            if (ee.IsAuthenticated)
        //            {
        //                await Navigation.PushAsync(new TabPage());

        //                var user = await oAuth2.GetUserInfoAsync(ee.Account);
        //                string name = user.Name;
        //                string email = user.email;
        //                string Id = user.Id;

        //                App.tenant_name = name;
        //                App.tenant_email = email;

        //                RegisterResultCheck(name, email, "line_" + Id, device_token);
        //            }
        //        };
        //        authenticator.Error += (s, ee) =>
        //        {
        //            Debug.WriteLine("Authentication error: " + ee.Message);
        //        };

        //        var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
        //        presenter.Login(authenticator);
        //    //}
        //}

        private async void stkBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        //private void uncheckedBtn_Clicked(object sender, EventArgs e)
        //{
        //    imgBtn_unchecked.IsVisible = false;
        //    imgBtn_checked.IsVisible = true;
        //}

        //private void checkedBtn_Clicked(object sender, EventArgs e)
        //{
        //    imgBtn_checked.IsVisible = false;
        //    imgBtn_unchecked.IsVisible = true;
        //}

        private void CountBadgeNum()
        {
            
            var isRegistered = Preferences.Get("isRegistered", false);
            if (!isRegistered)
            {
                Preferences.Set("badgeNum", 1);
                Preferences.Set("isRegistered", true);
            }
        }

        protected  override bool OnBackButtonPressed()
        {
            closeApp_check();

            //return base.OnBackButtonPressed();
            return true;
        }

        private async void closeApp_check()
        {
            var action = await DisplayAlert("", "アプリを終了しますか？", "はい", "キャンセル");
            if (action)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}