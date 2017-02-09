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
        private string baseUrl = "https://pisrv01.pischool.int/piwebapi/";
        private async Task<string> DownloadWebData(string url)
        {
            var handler = new HttpClientHandler();
            handler.Credentials = new NetworkCredential("student01", "student", "pischool");
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
            handler.Credentials = new NetworkCredential("student01", "student", "pischool");
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
            //Please refer to the following page for more information about Batch: https://pisrv01.pischool.int/piwebapi/help/controllers/batch/actions/execute
            Dictionary<string, PIBatchRequest> globalBatch = new Dictionary<string, PIBatchRequest>();

            //Complete the globalBatch

            string json = JsonConvert.SerializeObject(globalBatch);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            string url = baseUrl + "/batch";
            string response = await PostWebData(url, httpContent);
            Dictionary<string, PIBatchResponse> batchData = JsonConvert.DeserializeObject<Dictionary<string, PIBatchResponse>>(response);

            //Process batchData and fill data into imageAttributes and piValues

            PIListObject imageAttributes;
            PIValue[] piValues;
            //Cities cities = new Cities(imageAttributes, piValues);
            //return cities;
            return null;
        }


        private async Task<Cities> GetCitiesDataNoBatch()
        {
            //Exercise 1           
            //First HTTP request: Get the WebId of the AF database Weather

            string url = baseUrl + @"assetdatabases?path=\\pisrv01\Weather";
            string response = await DownloadWebData(url);
            PIObject dbData = JsonConvert.DeserializeObject<PIObject>(response);

            //Second HTTP request:  Find all the 50 attributes called Wikipedia Thumbnail Url 
            //HINT: https://pisrv01.pischool.int/piwebapi/help/controllers/element/actions/findelementattributes
            //HINT: Use JsonConvert.DeserializeObject to convert a JSON string to C# object
            PIListObject imageAttributes; //Retrieve the response and deserialize into imageAttributes



            //Third HTTP request: Get the values of those 50 attributes by making a bulk call
            //HINT: https://pisrv01.pischool.int/piwebapi/help/controllers/streamset/actions/getvaluesadhoc
            PIListObject imagesValues; //Retrieve the response and deserialize into imagesValues

            //Using both PIListObjects as inputs, the Cities object is created.
            //Cities cities = new Cities(imageAttributes, imagesValues);
            //return cities;
            return null;
        }

        public async Task<Cities> GetCitiesData(bool useBatch = false)
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
