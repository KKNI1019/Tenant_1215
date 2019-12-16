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
	public partial class Third_RegPage : ContentPage
	{
       
        public Third_RegPage ()
		{
			InitializeComponent ();
            
            lbl_buildingName.Text = App.estate_name;

            if (App.estate_image_url != null)
            {
                img_building.Source = Constants.ESTATE_IMAGE_URL_PREFIX + App.estate_image_url;
            }
            else
            {
                img_building.Source = "img_building.png";
            }
        }

        private async void imgBtn_finish_clicked(object sender, EventArgs e)
        {
            if (!string.Equals(ent_roomNum.Text, App.estate_room_number))
            {
                await DisplayAlert("", "部屋番号が正しくありません。", "はい");
            }
            else
            {
                await Navigation.PushAsync(new UserInfoPage());
            }
                
        }

        private async void stkBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        //private async void imgBack_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PopAsync();
        //}
    }

    
}