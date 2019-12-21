using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
using tenantApp.Popups;
using tenantApp.Views;
using tenantApp.Webservices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendCertiPage : ContentPage
	{
        private MediaFile mediafile;
        private string certification_type;

        public SendCertiPage (string certification, MediaFile file)
		{
			InitializeComponent ();

            img_certificationPhoto.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            mediafile = file;
            certification_type = certification;
        }

        private void ImgBtn_sendCerti_Clicked(object sender, EventArgs e)
        {
            sendFile();
            //await Navigation.PushAsync(new LoginPage());
        }

        private async void sendFile()
        {
            loadinbar.IsRunning = true;

            try
            {
                StreamContent scontent = new StreamContent(mediafile.GetStream());
                scontent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = App.tenant_ID,
                    Name = "image"
                };
                scontent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                var client = new HttpClient();
                var multi = new MultipartFormDataContent();
                multi.Add(scontent);
                client.BaseAddress = new Uri(App.endpoint);
                var result = client.PostAsync("api/user/upload_license", multi).Result;
                result.EnsureSuccessStatusCode();
                var response = await result.Content.ReadAsStringAsync();
                ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);
                if (resultMsg.resp == "success")
                {
                    loadinbar.IsRunning = false;
                    //DependencyService.Get<IMessage>().LongAlert(resultMsg.message);
                    await Navigation.PushAsync(new TabPage());
                }
                else
                {
                    loadinbar.IsRunning = false;

                    await DisplayAlert("", resultMsg.message, "はい");
                }

            }
            catch
            {
                loadinbar.IsRunning = false;

                await DisplayAlert("", "サーバー接続でエラーが発生しました。", "はい");
            }
        }

        private async void BtnCaptureAgain_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void stkBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}