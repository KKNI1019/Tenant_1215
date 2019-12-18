using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.CloudMessaging;
using Foundation;
using Octane.Xamarin.Forms.VideoPlayer.iOS;
using Plugin.FirebasePushNotification;
using Plugin.Media;
using Syncfusion.ListView.XForms.iOS;
using UIKit;
using UserNotifications;
using Xamarin;
using Xamarin.Essentials;

namespace tenantApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate , IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        //public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        //{
        //    System.Diagnostics.Debug.WriteLine($"FCM Token: {fcmToken}");
        //}

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();

            SfListViewRenderer.Init();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTM5OTIzQDMxMzcyZTMyMmUzMGhGNUZBVnh1T3RBVFlBT0xpeHlrT01rRVRNSk1RMFgxbENSZlJWc0FxbjQ9");

            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            CrossMedia.Current.Initialize();

            //KeyboardOverlap.Forms.Plugin.iOSUnified.KeyboardOverlapRenderer.Init();

            IQKeyboardManager.SharedManager.Enable = true;


            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            global::Xamarin.Forms.Forms.Init();

            FormsVideoPlayer.Init();

            LoadApplication(new App());
            
            UITabBar.Appearance.SelectedImageTintColor = UIColor.FromRGB(255, 215, 0);

            //Firebase.Core.App.Configure();

            //var token = Messaging.SharedInstance.FcmToken ?? "";
            //Preferences.Set("device_token", token);


            //if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            //{
            //    // iOS 10 or later
            //    var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
            //    UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
            //    {
            //        Console.WriteLine(granted);
            //    });

            //    // For iOS 10 display notification (sent via APNS)
            //    UNUserNotificationCenter.Current.Delegate = this;

            //    // For iOS 10 data message (sent via FCM)
            //    Messaging.SharedInstance.Delegate = this;
            //}
            //else
            //{
            //    // iOS 9 or before
            //    var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
            //    var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
            //    UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            //}

            //UIApplication.SharedApplication.RegisterForRemoteNotifications();

            return base.FinishedLaunching(app, options);
        }

        //public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        //{
        //    FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        //}

        //public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        //{
        //    FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

        //}
        //public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        //{

        //    FirebasePushNotificationManager.DidReceiveMessage(userInfo);
        //    System.Console.WriteLine(userInfo);

        //    completionHandler(UIBackgroundFetchResult.NewData);
        //}

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired till the user taps on the notification launching the application.
            // TODO: Handle data of notification

            // With swizzling disabled you must let Messaging know about the message, for Analytics
            //Messaging.SharedInstance.AppDidReceiveMessage (userInfo);

            // Print full message.
            Console.WriteLine(userInfo);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired till the user taps on the notification launching the application.
            // TODO: Handle data of notification

            // With swizzling disabled you must let Messaging know about the message, for Analytics
            //Messaging.SharedInstance.AppDidReceiveMessage (userInfo);

            // Print full message.
            Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }

    }
}
