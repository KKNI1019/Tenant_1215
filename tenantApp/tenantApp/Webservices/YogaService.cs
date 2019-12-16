using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using tenantApp.Models;

namespace tenantApp.Webservices
{
    public class YogaService
    {
        HttpClient _client;

        public YogaService()
        {
            _client = new HttpClient();
        }

        public async Task<YogaImageInfo> GetYogaDataAsync(string uri)
        {
            YogaImageInfo YogaData = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    YogaData = JsonConvert.DeserializeObject<YogaImageInfo>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tError {0}", ex.Message);
            }

            return YogaData;
        }
    }
}
