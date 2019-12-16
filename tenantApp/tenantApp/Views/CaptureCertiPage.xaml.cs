using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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
	public partial class CaptureCertiPage : ContentPage
	{
        private string certification_type;

        public CaptureCertiPage ()
		{
			InitializeComponent ();
        }

        private async void ImgBtn_capture_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(certification_type))
            {
                await DisplayAlert("", "身分証明書を選択します。", "はい");
            }
            else
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "Test",
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear
                });

                if (file == null) return;

                await Navigation.PushAsync(new SendCertiPage(certification_type, file));
            }
        }

        private async void stkBack_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Drivercard_capture_Clicked(object sender, EventArgs e)
        {
            drivercard_capture.BackgroundColor = Color.FromHex("#FFDF01");
            certification_type = "driver_card";
            healthcard_capture.BackgroundColor = Color.Transparent;
        }

        private void Healthcard_capture_Clicked(object sender, EventArgs e)
        {
            healthcard_capture.BackgroundColor = Color.FromHex("#FFDF01");
            certification_type = "health_card";
            drivercard_capture.BackgroundColor = Color.Transparent;
        }
    }
}