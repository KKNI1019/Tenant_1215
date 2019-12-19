using Rg.Plugins.Popup.Services;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
using tenantApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QAPage : ContentPage
	{
        public IList<Columns> Columns { get; set; }
        int itemIndex = -1;

        public QAPage ()
		{
			InitializeComponent ();

            removeNew();

            MessagingCenter.Subscribe<App>((App)Application.Current, "refreshpage", (sender) =>
            {
                Refresh();
            });
        }

        private async void Refresh()
        {
            sflistview.ItemsSource = await App.QA_Data.GetNotiAsync();
        }

        private async void removeNew()
        {
            DateTime oldDate = DateTime.Now.AddDays(-5);

            var qalist = await App.QA_Data.GetNotiAsync();
            if (qalist.Count > 0)
            {
                for (int i = 0; i < qalist.Count; i++)
                {
                    int compare_date = DateTime.Compare(qalist[i].c_date, oldDate);
                    if (compare_date < 0)
                    {
                        qalist[i].img_visibility = false;
                        await App.QA_Data.SaveNotiAsync(qalist[i]);
                    }
                }
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            sflistview.ItemsSource = await App.QA_Data.GetNotiAsync();
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
            var action = await DisplayAlert("", "アイテムを削除します。", "削除する", "キャンセル");
            if (action)
            {
                Button button = (Button)sender;
                string question = button.CommandParameter.ToString();

                var qalist = await App.QA_Data.GetDelQAAsync(question);
                await App.QA_Data.DeleteNotiAsync(qalist);

                MessagingCenter.Send<App>((App)Application.Current, "refreshpage");
            }
        }

        private async void Sflistview_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new QADetailPage
            {
                BindingContext = e.ItemData as QAList
            });

            ((SfListView)sender).SelectedItem = null;
        }
    }
}