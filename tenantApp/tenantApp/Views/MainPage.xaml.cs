using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;
using tenantApp.Webservices;
using Xamarin.Forms;

namespace tenantApp
{
    public partial class MainPage : ContentPage
    {
        YogaService _yogaService;

        public MainPage()
        {
            InitializeComponent();

            _yogaService = new YogaService();

            GetYogaInfos();
        }

        private async void GetYogaInfos()
        {
            string GenerateRequestUri(string endpoint)
            {
                string requestUri = endpoint;
                requestUri += $"search?channelId={Constants.ChannelID}";
                requestUri += "&type=video&part=snippet&order=date&maxResults=10";
                requestUri += $"&key={Constants.OpenAPIKey}";
                return requestUri;
            }

            YogaImageInfo yogaData = await _yogaService.GetYogaDataAsync(GenerateRequestUri(Constants.OpenEndpoint));
            
            lstview.ItemsSource = yogaData.Item;
        }
    }
}
 