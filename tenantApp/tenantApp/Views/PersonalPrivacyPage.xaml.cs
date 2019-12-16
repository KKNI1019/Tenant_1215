using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PersonalPrivacyPage : ContentPage
	{
		public PersonalPrivacyPage ()
		{
			InitializeComponent ();
		}

        private async void Agree_btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void stkBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}