using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TroubleWaterPage : ContentPage
	{
		public TroubleWaterPage ()
		{
			InitializeComponent ();
		}

        private void btnMenu_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void imgBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Btn_post_Clicked(object sender, EventArgs e)
        {

        }

        private void trouble1_Clicked(object sender, EventArgs e)
        {

        }

        private void trouble2_Clicked(object sender, EventArgs e)
        {

        }

        private void trouble3_Clicked(object sender, EventArgs e)
        {

        }

        private void troubleother_Clicked(object sender, EventArgs e)
        {

        }
    }
}