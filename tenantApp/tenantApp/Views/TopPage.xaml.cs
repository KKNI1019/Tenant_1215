using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tenantApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TopPage : ContentPage
	{
        private string pay_year;
        private string pay_month;
        private string pay_date;
        private string pay_total;
        private string finalDay;
        private int period_year;
        private int period_month;
        private int cost;

        public TopPage ()
		{
			InitializeComponent ();

            DateTime date = DateTime.Now;
            pay_year = $"{date.Year} {"年"}";
            pay_month = $"{date.Month} {"月"}";
            pay_date = $"{date.Month} {"日"}";
            pay_total = $"{pay_year}{pay_month}{pay_date}";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (!string.IsNullOrEmpty(App.estate_renewal_period))
            {
                var paid_date = App.estate_next_renewal_month.AddMonths(-Convert.ToInt32(App.estate_renewal_period));
                DateTime current_date = DateTime.Now;
                TimeSpan diff = App.estate_deadline.Subtract(current_date);

                var days = diff.Days;
                var yy = days / 365;
                var mm = days % 365 / 30;
                string period;
                if (yy == 0)
                {
                    period = mm.ToString() + "ヶ月";
                }
                else
                {
                    period = yy.ToString() + "年" + mm.ToString() + "ヶ月";
                }

                lbl_finalDay.Text = App.estate_next_renewal_month.ToString("yyyy年M月dd日");
                lbl_payDay.Text = paid_date.ToString("yyyy年M月dd日");
                lbl_period.Text = period;
                lbl_rentCost.Text = App.estate_rent + "円";
            }
        }

            private void menuBtn_clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new SideMenuPopup());
        }

        private async void ImgBtn_board_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BoardPage());
        }

        private async void ImgBtn_column_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewsPage());
        }

        private async void ImgBtn_RecentQA_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QAPage());
        }

        private void ImgBtn_chatbg_Clicked(object sender, EventArgs e)
        {
            
            var masterPage = this.Parent.Parent as TabbedPage;
            if (masterPage != null)
            {
                masterPage.CurrentPage = masterPage.Children[1];
            }
           
        }

        private async void btnWaterQA_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TroubleWaterPage());
        }

        private async void btnElectricityTrouble_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TroubleElectricityPage());
        }

        private async void btnGasTrouble_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TroubleGasPage());
        }
    }
}