using ConsumerLinks.Models;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace ConsumerLinks.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<int> Send(string url)
        {
            System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            webRequest.Method = "HEAD";

            try
            {
                System.Net.HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                int statusCode = (int)webResponse.StatusCode;
                return statusCode;
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Response != null)
                {
                    var response = (System.Net.HttpWebResponse)ex.Response;
                    int statusCode = (int)response.StatusCode;
                    return statusCode;
                }
                else
                {
                    return 500;
                }

            }

        }

        public async Task UpdateLink(StatusUpdate statusUpdate)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, "http://nginx:80/api/link");
                string jsonString = System.Text.Json.JsonSerializer.Serialize(statusUpdate);
                Console.WriteLine(jsonString);
                request.Content = new StringContent(
                    jsonString, Encoding.UTF8, "application/json"
                );

                Console.WriteLine(request.Content);

                var response = await _httpClient.SendAsync(request);

                Console.WriteLine($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
    }
}
