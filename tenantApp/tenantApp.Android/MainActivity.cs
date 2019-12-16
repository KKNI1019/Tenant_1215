using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Rg.Plugins.Popup.Services;
using Plugin.Media;
using Plugin.Permissions;
using Android.Content;
using Octane.Xamarin.Forms.VideoPlayer.Android;
using Firebase;
using System.Threading.Tasks;
using Firebase.Iid;
using Xamarin.Essentials;

namespace tenantApp.Droid
{
    [Activity(Label = "budi concierge[入居者用]", Icon = "@drawable/appicon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public async override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                await PopupNavigation.Instance.PopAsync();
            }
        }
        
        protected  override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CrossMedia.Current.Initialize();

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);

            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);

            FormsVideoPlayer.Init();

            FirebaseApp.InitializeApp(this);

            Task.Run(() =>
            {
                var preferenceToken = Preferences.Get("device_token", "");
                if (preferenceToken == "")
                {
                    FirebaseInstanceId.Instance.DeleteInstanceId();
                }
                
            });

            LoadApplication(new App());

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5OTIzQDMxMzcyZTMyMmUzMGhGNUZBVnh1T3RBVFlBT0xpeHlrT01rRVRNSk1RMFgxbENSZlJWc0FxbjQ9");

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}