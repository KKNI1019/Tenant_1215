using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QADetailPage : ContentPage
	{
		public QADetailPage ()
		{
			InitializeComponent ();
		}

        private void ImgMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void ImgBack_Clicked(object sender, EventArgs e)
        {
            removeNew();

            await Navigation.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            removeNew();

            return base.OnBackButtonPressed();
        }

        private async void removeNew()
        {
            QAList qalist = this.BindingContext as QAList;
            if (qalist.img_visibility)
            {
                qalist.img_visibility = false;
                await App.QA_Data.SaveNotiAsync(qalist);
            }
        }
    }
}