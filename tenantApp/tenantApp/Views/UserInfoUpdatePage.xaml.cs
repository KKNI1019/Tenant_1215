using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public partial class UserInfoUpdatePage : ContentPage
    {
        public UserInfoUpdatePage()
        {
            InitializeComponent();

            lbl_name.Text = App.tenant_name;
            lbl_nickname.Text = "";
            lbl_tenant_address.Text = "";
            lbl_email.Text = App.tenant_email;

            if (App.tenant_profile != null)
            {
                imgProfile.Source = Constants.IMAGE_UPLOAD_URL_PREFIX_TENANT + App.tenant_profile;
            }

        }

        private void ImgBtn_confirm_Clicked(object sender, EventArgs e)
        {
             App.tenant_name = lbl_name.Text ;
             App.tenant_nickname = lbl_nickname.Text ;
             App.tenant_email = lbl_email.Text ;
            
             getResult();
        }

        private async void getResult()
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("tenant_id", App.tenant_ID),
                    new KeyValuePair<string, string>("tenant_name", App.tenant_name),
                    new KeyValuePair<string, string>("tenant_nickname", App.tenant_nickname),
                    new KeyValuePair<string, string>("tenant_email", App.tenant_email),
                    new KeyValuePair<string, string>("tenant_phone1", App.tenant_phone1),
                    new KeyValuePair<string, string>("tenant_birthday", App.tenant_birthday),
                    new KeyValuePair<string, string>("tenant_password", Preferences.Get(Constants.TENANT_PWD,"")),
                });
                try
                {
                    var request = await cl.PostAsync(App.endpoint + "api/user/update", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        await Navigation.PopAsync();
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

        private void imgprofile_tap(object sender, EventArgs e)
        {
            select_photo();
        }

        private async void select_photo()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("", "お使いのデバイスはこの機能をサポートしていません。", "はい");
                return;
            }
            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };
            var selectedImgFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
            if (selectedImgFile == null)
            {
                await DisplayAlert("", "画像を取得できませんでした。もう一度お試しください。", "はい");
                return;
            }

            imgProfile.Source = ImageSource.FromStream(() => selectedImgFile.GetStream());

            sendfile(selectedImgFile);

        }

        private async void sendfile(MediaFile file)
        {
            try
            {
                loadingbar.IsRunning = true;

                StreamContent scontent = new StreamContent(file.GetStream());
                scontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = App.tenant_ID,
                    Name = "image"
                };
                scontent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                var client = new HttpClient();
                var multi = new MultipartFormDataContent();
                multi.Add(scontent);
                client.BaseAddress = new Uri(Constants.ENDPOINT_URL);
                var result = client.PostAsync("api/user/upload_profile", multi).Result;
                result.EnsureSuccessStatusCode();
                var response = await result.Content.ReadAsStringAsync();
                ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);
                if (resultMsg.resp == "success")
                {
                    loadingbar.IsRunning = false;

                    imgProfile.Source = ImageSource.FromStream(() => file.GetStream());
                    App.tenant_profile = resultMsg.tenant_profile;
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
}