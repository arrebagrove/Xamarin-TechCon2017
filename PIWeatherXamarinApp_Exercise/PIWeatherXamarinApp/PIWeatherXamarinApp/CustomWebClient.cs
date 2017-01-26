using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PIWeatherXamarinApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace PIWeatherXamarinApp
{
    public class CustomWebClient
    {
        private string baseUrl = "https://cross-platform-lab-uc2017.osisoft.com/piwebapi/";
        private async Task<string> DownloadWebData(string url)
        {
            var handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential("pilabuser", "PIWebAPI2015");
            HttpClient httpClient = new HttpClient(handler);
            HttpResponseMessage httpMessage = await httpClient.GetAsync(url);
            if (httpMessage.IsSuccessStatusCode == true)
            {
                using (var stream = await httpMessage.Content.ReadAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        return await streamReader.ReadToEndAsync();
                    }
                }
            }
            return string.Empty;
        }

        private async Task<string> PostWebData(string url, HttpContent httpContent)
        {
            var handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential("pilabuser", "PIWebAPI2015");
            HttpClient httpClient = new HttpClient(handler);
            HttpResponseMessage httpMessage = await httpClient.PostAsync(url, httpContent);
            if (httpMessage.IsSuccessStatusCode == true)
            {
                using (var stream = await httpMessage.Content.ReadAsStreamAsync())
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        return await streamReader.ReadToEndAsync();
                    }
                }
            }
            return string.Empty;
        }



        public async Task<City> GetCityData(string elementWebId)
        {
            string url = baseUrl + "streamsets/" + elementWebId + "/value";
            string response = await DownloadWebData(url);
            PIListObject elementData = JsonConvert.DeserializeObject<PIListObject>(response);
            return new City(elementData);
        }

        private async Task<Cities> GetCitiesDataWithBatch()
        {
            //Exercise 5
            return null;
        }


        private async Task<Cities> GetCitiesDataNoBatch()
        {
            //Exercise 1
            return null;
        }

        public async Task<Cities> GetCitiesData(bool useBatch = true)
        {
            if (useBatch == false)
            {
                return await GetCitiesDataNoBatch();
            }
            else
            {
                return await GetCitiesDataWithBatch();
            }
        }
    }
}
