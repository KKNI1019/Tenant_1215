using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Webservices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Second_RegPage : ContentPage
	{
        

		public Second_RegPage ()
		{
			InitializeComponent ();

            lbl_buildingName.Text = App.estate_name;
            //lbl_buildingName.Text = "レジデンシャル六本木";
            if (App.estate_image_url != null)
            {
                img_building.Source = Constants.ESTATE_IMAGE_URL_PREFIX + App.estate_image_url;
            }
            else
            {
                img_building.Source = "img_building.png";
            }
            
        }

        private async void ImgBtn_yes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Third_RegPage());
        }

        private async void ImgBtn_no_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PopAsync();

            await Navigation.PushAsync(new LoginPage());
        }
    }
}