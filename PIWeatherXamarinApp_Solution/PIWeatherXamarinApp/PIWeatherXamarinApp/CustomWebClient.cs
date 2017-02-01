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
            Dictionary<string, PIBatchRequest> globalBatch = new Dictionary<string, PIBatchRequest>();
            PIBatchRequest batchGetDbWebId = new PIBatchRequest()
            {
                Method = "GET",
                Resource = baseUrl + @"assetdatabases?path=\\pisrv01\Weather"
            };

            PIBatchRequest batchGetElementsWebIds = new PIBatchRequest()
            {
                Method = "GET",
                Resource = baseUrl + "assetdatabases/{0}/elementattributes?attributeNameFilter=*Wikipedia%20Thumbnail%20Url*",
                Parameters = new List<string>() { "$.1.Content.WebId" },
                ParentIds = new List<string>() { "1" }
            };

            PIBatchRequest batchGetAttributesValues = new PIBatchRequest()
            {
                Method = "GET",
                RequestTemplate = new PIRequestTemplate()
                {
                    Resource = baseUrl + "streams/{0}/value"
                },
                Parameters = new List<string>() { "$.2.Content.Items[*].WebId" },
                ParentIds = new List<string>() { "2" }
            };

            globalBatch.Add("1", batchGetDbWebId);
            globalBatch.Add("2", batchGetElementsWebIds);
            globalBatch.Add("3", batchGetAttributesValues);
            string json = JsonConvert.SerializeObject(globalBatch);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            string url = baseUrl + "/batch";
            string response = await PostWebData(url, httpContent);
            Dictionary<string, PIBatchResponse> batchData = JsonConvert.DeserializeObject<Dictionary<string, PIBatchResponse>>(response);
            PIListObject imageAttributes = JsonConvert.DeserializeObject<PIListObject>(batchData["2"].Content.ToString());
            JArray items = (JArray)(batchData["3"].Content["Items"]);
            PIValue[] piValues = new PIValue[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                piValues[i] = JsonConvert.DeserializeObject<PIValue>(items[i]["Content"].ToString());
            }
            Cities cities = new Cities(imageAttributes, piValues);
            return cities;
        }


        private async Task<Cities> GetCitiesDataNoBatch()
        {
            //Exercise 1
            string url = baseUrl + @"assetdatabases?path=\\pisrv01\Weather";
            string response = await DownloadWebData(url);
            PIObject dbData = JsonConvert.DeserializeObject<PIObject>(response);


            url = baseUrl + "assetdatabases/" + dbData.WebId + "/elementattributes?attributeNameFilter=*Wikipedia%20Thumbnail%20Url*";
            response = await DownloadWebData(url);

            PIListObject imageAttributes = JsonConvert.DeserializeObject<PIListObject>(response);
            List<string> webIds = imageAttributes.GetItemsWebIds();
            url = baseUrl + "streamsets/value?";
            foreach (string webId in webIds)
            {
                url = url + string.Format("webId={0}&", webId);
            }
            url = url.Substring(0, url.Length - 1);


            response = await DownloadWebData(url);
            PIListObject imagesValues = JsonConvert.DeserializeObject<PIListObject>(response);
            Cities cities = new Cities(imageAttributes, imagesValues);
            return cities;
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
