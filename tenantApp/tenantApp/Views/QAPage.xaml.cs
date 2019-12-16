using Rg.Plugins.Popup.Services;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
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

            Columns = new List<Columns>();
            Columns.Add(new Columns
            {
                date = DateTime.UtcNow,
                img_url = "imgUser.png",
                col_content = "新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新新",
                col_title = "Q&Aタイトル"
            });

            Columns.Add(new Columns
            {
                date = DateTime.UtcNow,
                img_url = "imgUser.png",
                col_content = "最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最",
                col_title = "Q&Aタイトル"
            });
            Columns.Add(new Columns
            {
                date = DateTime.UtcNow,
                img_url = "imgUser.png",
                col_content = "ののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののののの",
                col_title = "Q&Aタイトル"
            });
            Columns.Add(new Columns
            {
                date = DateTime.UtcNow,
                img_url = "imgUser.png",
                col_content = "ささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささささ",
                col_title = "Q&Aタイトル"
            });
            Columns.Add(new Columns
            {
                date = DateTime.UtcNow,
                img_url = "imgUser.png",
                col_content = "稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿稿",
                col_title = "Q&Aタイトル"
            });
            Columns.Add(new Columns
            {
                date = DateTime.UtcNow,
                img_url = "imgUser.png",
                col_content = "ラララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララララv",
                col_title = "Q&Aタイトル"
            });

            Columns.Add(new Columns
            {
                date = DateTime.UtcNow,
                img_url = "imgUser.png",
                col_content = "示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示示",
                col_title = "Q&Aタイトル"
            });
            Columns.Add(new Columns
            {
                date = DateTime.UtcNow,
                img_url = "imgUser.png",
                col_content = "まままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままままま",
                col_title = "Q&Aタイトル"
            });
            Columns.Add(new Columns
            {
                date = DateTime.UtcNow,
                img_url = "imgUser.png",
                col_content = "ひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひひ",
                col_title = "Q&Aタイトル"
            });

            BindingContext = this;
            
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
                
            }
        }
    }
}