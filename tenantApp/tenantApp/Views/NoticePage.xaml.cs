using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
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
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoticePage : ContentPage
	{
        public IList<Notifications> Notifications { get; set; }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listview.ItemsSource = await App.Noti_Data.GetNotiAsync();
        }

        public NoticePage ()
		{
			InitializeComponent ();

            get_noti_list(App.tenant_ID, Preferences.Get("last_notice_id", ""));
            
        }

        private async void get_noti_list(string tenantId, string noticeId)
        {
            

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("tenant_id", tenantId),
                    new KeyValuePair<string, string>("last_notice_id", noticeId)
                });

                try
                {
                    var request = await cl.PostAsync(App.endpoint + "api/user/get_notice", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                       

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        var notice_num = resultData.notice_list_data.Length;
                        if (notice_num != 0)
                        {
                            int badgeNum = Preferences.Get("badgeNum", 0);

                            for (int i = 0; i < notice_num; i++)
                            {
                                badgeNum++;

                                await App.Noti_Data.SaveNotiAsync(new Notifications
                                {
                                    noti_id = resultData.notice_list_data[i].notice_id,
                                    noti_title = resultData.notice_list_data[i].notice_title,
                                    noti_content = resultData.notice_list_data[i].notice_contents,
                                    date = resultData.notice_list_data[i].u_date,
                                    IsVisible = true
                                });
                            }

                            Preferences.Set("badgeNum", badgeNum);
                            MessagingCenter.Send<App>((App)Application.Current, "BadgeCountRefresh");

                            listview.ItemsSource = await App.Noti_Data.GetNotiAsync();

                            Preferences.Set("last_notice_id", resultData.notice_list_data[notice_num - 1].notice_id);
                        }
                    }
                    else
                    {
                        

                        await DisplayAlert("", resultMsg.resp, "はい");
                    }
                }
                catch
                {
                   

                    await DisplayAlert("", "サーバー接続でエラーが発生しました。", "はい");
                }

            }
        }

        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {

                await Navigation.PushAsync(new NoticeDetailPage
                {
                    BindingContext = e.SelectedItem as Notifications
                });

                ((ListView)sender).SelectedItem = null;
                
            }
        }
    }
}