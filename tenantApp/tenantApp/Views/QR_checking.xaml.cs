using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
using tenantApp.Webservices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QR_checking : ContentPage
	{
        
        public QR_checking ()
		{
			InitializeComponent();         
        }

        private async void onQRReadBtn_clicked(object sender, EventArgs e)
        {
            
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    ent_qrcode.Text = result.Text;
                });
            };

        }
        private void Scanner_OnScanResult(ZXing.Result result)
        {
            var tt = result.Text;
            ent_qrcode.Text = tt;
        }

        private async void onFinishBtn_clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ent_qrcode.Text))
            {
                await DisplayAlert("", "QRコードを確認してください。", "はい");
            }
            else
            {
                GetBuildingInfo(ent_qrcode.Text);
                //GetBuildingInfo("10");
            }
            
        }

        private async void GetBuildingInfo(string qrcode)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                   new KeyValuePair<string, string>("estate_id", qrcode)
                });

                try
                {
                    var request = await cl.PostAsync(App.endpoint + "api/user/get_building_info", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();

                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);
                   
                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        App.estate_id = resultData.estate_data.estate_id;
                        App.estate_name = resultData.estate_data.estate_name;
                        App.estate_address = resultData.estate_data.estate_address;
                        App.estate_room_number = resultData.estate_data.estate_room_number;
                        App.estate_rent = resultData.estate_data.estate_rent;
                        App.estate_owner_id = resultData.estate_data.estate_owner_id;
                        App.estate_sale_status = resultData.estate_data.estate_sale_status;
                        App.estate_zero_status = resultData.estate_data.estate_zero_status;
                        App.estate_memo = resultData.estate_data.estate_memo;
                        App.estate_image_url = resultData.estate_data.estate_image_url;

                        await Navigation.PushAsync(new Second_RegPage());
                    }
                    else
                    {
                        loadingbar.IsRunning = false;

                        await DisplayAlert("", "登録されていない建物です。", "はい");
                    }
                }
                catch
                {
                    loadingbar.IsRunning = false;

                    await DisplayAlert("", "サーバー接続でエラーが発生しました。", "はい");
                }
                
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //if (Device.OS == TargetPlatform.Android)
            DependencyService.Get<IAndroidMethods>().CloseApp();

            return base.OnBackButtonPressed();
        }
    }
}