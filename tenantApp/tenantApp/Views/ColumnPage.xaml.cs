using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Syncfusion.ListView.XForms;
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
	public partial class ColumnPage : ContentPage
	{
        public IList<Columns> Columns { get; set; }
        int itemIndex = -1;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            sflist.ItemsSource = await App.Col_Data.GetColumnAsync();
        }

        public ColumnPage ()
		{
			InitializeComponent ();

            get_column_list(App.tenant_ID, Preferences.Get("last_comment_id", ""));
            
            sflist.ItemTapped += ListView_ItemTapped;

            MessagingCenter.Subscribe<App>((App)Application.Current, "OnCategoryCreated", (sender) => {
                GetMerchantCategory();
            });
        }

        private async void GetMerchantCategory()
        {
            sflist.ItemsSource = await App.Col_Data.GetColumnAsync();
        }

        private async void get_column_list(string tenantId, string columnId)
        {
            loadingbar.IsRunning = true;

            using (var cl = new HttpClient())
            {
                var formcontent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("tenant_id", tenantId),
                    new KeyValuePair<string, string>("last_comment_id", columnId)
                });

                try
                {
                    var request = await cl.PostAsync(App.endpoint + "api/user/get_comment_list", formcontent);
                    request.EnsureSuccessStatusCode();
                    var response = await request.Content.ReadAsStringAsync();
                    ResponseMsg resultMsg = JsonConvert.DeserializeObject<ResponseMsg>(response);

                    if (resultMsg.resp.Equals("success"))
                    {
                        loadingbar.IsRunning = false;

                        ResponseData resultData = JsonConvert.DeserializeObject<ResponseData>(response);

                        var column_num = resultData.comment_list_data.Length;
                        if (column_num != 0)
                        {
                            for (int i = 0; i < column_num; i++)
                            {
                                await App.Col_Data.SaveColumnAsync(new Columns
                                {
                                    img_url = "imgUser.png",
                                    column_id = resultData.comment_list_data[i].comment_id,
                                    col_title = resultData.comment_list_data[i].comment_title,
                                    col_content = resultData.comment_list_data[i].comment_contents,
                                    user_name = resultData.comment_list_data[i].comment_writer_user_name,
                                    date = resultData.comment_list_data[i].u_date,
                                    IsVisible = true
                                });
                            }

                            sflist.ItemsSource = await App.Col_Data.GetColumnAsync();

                            Preferences.Set("last_comment_id", resultData.comment_list_data[column_num - 1].comment_id);
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

                    await DisplayAlert("", "サーバー接続でエラーが発生しました。", "はい");
                }

            }
        }

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new ColumnDetailPage
            {
                BindingContext = e.ItemData as Columns
            });

            ((SfListView)sender).SelectedItem = null;
        }
       
        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void imgMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }
        
        private void ListView_SwipeStarted(object sender, SwipeStartedEventArgs e)
        {
            itemIndex = -1;
        }

        private void ListView_Swiping(object sender, SwipingEventArgs e)
        {
            if (e.ItemIndex == 1 && e.SwipeOffSet > 70)
                e.Handled = true;
        }

        private void ListView_SwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            itemIndex = e.ItemIndex;
        }

        private async void BtnDel_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert("", "コラムを削除します。", "削除する", "キャンセル");
            if (action)
            {
                Button button = (Button)sender;
                string del_column_id = button.CommandParameter.ToString();

                var column = await App.Col_Data.GetDelColumnAsync(del_column_id);
                await App.Col_Data.DeletecolumnAsync(column);

                MessagingCenter.Send<App>((App)Application.Current, "OnCategoryCreated");
            }
        }
    }
}