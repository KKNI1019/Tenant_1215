using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
	public partial class BoardDelPopup : PopupPage
    {
        private string del_thread_id;
        private string del_type;

		public BoardDelPopup (string clicked_thread_id, string type)
		{
			InitializeComponent ();

            del_thread_id = clicked_thread_id;
            del_type = type;
		}

        private async void btnDel_Clicked(object sender, EventArgs e)
        {
            if(del_type == "del_thread")
            {
                var thread = await App.Th_Data.GetSelectedThreadAsync(del_thread_id);
                await App.Th_Data.DeleteThreadAsync(thread);

                MessagingCenter.Send<App>((App)Application.Current, "OnBoardPageRefresh");
            }
            else if(del_type == "del_comment")
            {
                var th_comment = await App.Th_CommentData.GetSelectedTh_commentAsync(del_thread_id);
                await App.Th_CommentData.DeleteTh_commentAsync(th_comment);

                MessagingCenter.Send<App>((App)Application.Current, "OnThreadPageRefresh");
            }
            
            await PopupNavigation.Instance.PopAsync();
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}