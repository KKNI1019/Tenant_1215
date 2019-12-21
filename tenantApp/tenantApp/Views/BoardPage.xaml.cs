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
	public partial class BoardPage : ContentPage
	{
        
        public IList<Threads>Threads { get; set; }

		public BoardPage ()
		{
			InitializeComponent ();

            get_thread_list(App.tenant_ID, Preferences.Get("last_thread_id", ""));

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnBoardPageRefresh", (sender) => {
                RefreshPage();
            });

        }

        private async void RefreshPage()
        {
            listview.ItemsSource = await App.Th_Data.GetThreadAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listview.ItemsSource = await App.Th_Data.GetThreadAsync();
        }
        
        //private void imgAdd_Clicked(object sender, EventArgs e)
        //{
        //    PopupNavigation.Instance.PushAsync(new BoardPopup());
        //}

        private async void get_thread_list(string tenantId, string threadId)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("tenant_id", tenantId),
                    new KeyValuePair<string, string>("last_thread_id", threadId)
                });

                try
                {
                    var request = await cl.PostAsync(App.endpoint + "api/user/get_thread_list", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        var thread_num = resultData.thread_list_data.Length;
                        if(thread_num != 0)
                        {
                            for (int i=0; i<thread_num; i++)
                            {
                                await App.Th_Data.SaveThreadAsync(new Threads
                                {
                                    img_url = "img_building.png",
                                    Th_id = resultData.thread_list_data[i].thread_id,
                                    Th_category = resultData.thread_list_data[i].thread_category,
                                    Th_note = resultData.thread_list_data[i].thread_note,
                                    Date = resultData.thread_list_data[i].u_date
                                });
                            }
                            
                            listview.ItemsSource = await App.Th_Data.GetThreadAsync();

                            Preferences.Set("last_thread_id", resultData.thread_list_data[thread_num - 1].thread_id);
                        }
                    }
                    else
                    {
                        loadingbar.IsRunning = false;

                        await DisplayAlert("", "サーバー接続でエラーが発生しました。", "はい");
                    }
                }
                catch
                {
                    loadingbar.IsRunning = false;

                    await DisplayAlert("", "サーバー接続でエラーが発生しました。", "はい");
                }

            }
        }
        

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        
        private void btnMore_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            string del_thread_id = button.CommandParameter.ToString();

            PopupNavigation.Instance.PushAsync(new BoardDelPopup(del_thread_id, "del_thread"));
        }

        private void Listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var obj = (Threads)e.Item;
            var _id = Convert.ToString(obj.Th_id);
            var seleted_thread_title = Convert.ToString(obj.Th_category);
            string selected_thread_id = Convert.ToString(_id);

            //await Navigation.PushAsync(new ThreadPage(selected_thread_id, seleted_thread_title));

            var masterPage = this.Parent.Parent as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[1];
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
}