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
	public partial class ThreadPage : ContentPage
	{
        public IList<Threads> Threads { get; set; }

        private string selected_thread_id;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listview.ItemsSource = await App.Th_CommentData.GetTh_commentAsync(selected_thread_id);

            var thread = await App.Th_Data.GetSelectedThreadAsync(selected_thread_id);
            get_th_comment_list(selected_thread_id, thread.Last_comment_Id, App.tenant_ID);

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnThreadPageRefresh", (sender) => {
                RefreshPage();
            });
        }

        public ThreadPage (string selected_th_id , string selected_th_title)
		{
			InitializeComponent ();

            lbl_thread_title.Text = selected_th_title;
            selected_thread_id = selected_th_id;

            listview.ItemSelected += DeselectItem;
        }

        public void DeselectItem(object sender, EventArgs e)
        {
            ((Xamarin.Forms.ListView)sender).SelectedItem = null;
        }

        private async void RefreshPage()
        {
            listview.ItemsSource = await App.Th_CommentData.GetTh_commentAsync(selected_thread_id);
        }

        private async void get_th_comment_list(string threadId, string th_comment_id, string tenantId)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("thread_id", threadId),
                    new KeyValuePair<string, string>("last_thread_comment_id",th_comment_id),
                    new KeyValuePair<string, string>("tenant_id", tenantId)
                });
                try
                {
                    var request = await cl.PostAsync(App.endpoint + "api/user/get_thread_detail", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        var th_comment_num = resultData.thread_comment_data.Length;
                        if (th_comment_num != 0)
                        {
                            
                            for (int i = 0; i < th_comment_num; i++)
                            {
                                await App.Th_CommentData.SaveTh_CommentAsync(new Th_comments
                                {
                                    img_url = "imgUser.png",
                                    Th_comment_id = resultData.thread_comment_data[i].thread_comment_id,
                                    Th_id = resultData.thread_comment_data[i].thread_comment_category,
                                    Th_comment_content = System.Net.WebUtility.UrlDecode(resultData.thread_comment_data[i].thread_comment_contents) ,
                                    c_date = resultData.thread_comment_data[i].c_date,
                                    Th_comment_writer_nickname = resultData.thread_comment_data[i].thread_comment_writer_nickname
                                });
                            }

                            var source = await App.Th_CommentData.GetTh_commentAsync(selected_thread_id);
                            listview.ItemsSource = source;
                            
                            var thread = await App.Th_Data.GetSelectedThreadAsync(resultData.thread_comment_data[th_comment_num-1].thread_comment_category);
                            thread.Last_comment_Id = resultData.thread_comment_data[th_comment_num - 1].thread_comment_id;

                            await App.Th_Data.SaveThreadAsync(thread);
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

        private async void Btn_post_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ent_myComment.Text))
            {
                loadingbar.IsRunning = true;

                using (var cl = new HttpClient())
                {
                    var formcontent = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("thread_comment_contents", System.Net.WebUtility.UrlEncode(ent_myComment.Text)),
                    new KeyValuePair<string, string>("thread_comment_writer_id", "t_"+App.tenant_ID),
                    new KeyValuePair<string, string>("thread_comment_writer_nickname", App.tenant_nickname),
                    new KeyValuePair<string, string>("thread_comment_category", selected_thread_id)
                });
                    try
                    {
                        var request = await cl.PostAsync(App.endpoint + "api/user/send_thread_comment", formcontent);
                        request.EnsureSuccessStatusCode();
                        var response = await request.Content.ReadAsStringAsync();
                        ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                        if (resultMsg.resp.Equals("success"))
                        {
                            loadingbar.IsRunning = false;

                            ent_myComment.Text = string.Empty;

                            var thread = await App.Th_Data.GetSelectedThreadAsync(selected_thread_id);
                            get_th_comment_list(selected_thread_id, thread.Last_comment_Id, App.tenant_ID);

                            List<Th_comments> msgList = ((IEnumerable<Th_comments>)this.listview.ItemsSource).ToList();
                            listview.ScrollTo(msgList[msgList.Count - 1], ScrollToPosition.End, true);
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
            
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void ImgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void btnMore_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            string del_comment_id = button.CommandParameter.ToString();

            PopupNavigation.Instance.PushAsync(new BoardDelPopup(del_comment_id, "del_comment"));
        }
        
    }
}