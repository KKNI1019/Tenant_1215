using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Views;
using tenantApp.Webservices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideMenuPopup : PopupPage
    {
        public SideMenuPopup()
        {
            InitializeComponent();

            ToProfilePage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ToProfilePage_clicked()),
            });
            ToQAPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ToQAPage_clicked()),
            });
            ToRulePage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ToRulePage_clicked()),
            });
            ToPersonalPolicy.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ToPersonalPolicy_clicked()),
            });
            ToQuestionPage.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => ToQuestionPage_clicked()),
            });
            lbl_logout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => lblLogout_clicked()),
            }); 
        }

        private async void ToProfilePage_clicked()
        {
            //await Navigation.PushAsync(new UserProfilepage());
            var masterPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1] as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[3];
            }

            await PopupNavigation.Instance.RemovePageAsync(this);
        }

        private async void ToQAPage_clicked()
        {
            var masterPage = Navigation.NavigationStack[Navigation.NavigationStack.Count - 1] as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[1];
            }

            await PopupNavigation.Instance.RemovePageAsync(this);
        }

        private async void ToRulePage_clicked()
        {
                    
            await Navigation.PushAsync(new RulePage());

            await PopupNavigation.Instance.RemovePageAsync(this);
        }

        private async void ToPersonalPolicy_clicked()
        {
            await Navigation.PushAsync(new PersonalPrivacyPage());

            await PopupNavigation.Instance.RemovePageAsync(this);
        }

        private async void ToQuestionPage_clicked()
        {
            await Navigation.PushAsync(new QuizPage());

            await PopupNavigation.Instance.RemovePageAsync(this);
        }

        private async void lblLogout_clicked()
        {
            Preferences.Set(Constants.REGISTERED, false);
            await Navigation.PushAsync(new LoginPage());

            await PopupNavigation.Instance.RemovePageAsync(this);
        }
        

        private async void imgDel_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}