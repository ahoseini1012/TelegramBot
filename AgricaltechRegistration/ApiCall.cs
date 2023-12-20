using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RegisterBulk
{

    public static class ApiCall<T>
    {
        public static async Task<ApiResult<T>> SendRequest(string payload, string apiPath, string accessToken)
        {
            ApiResult<T> apiresutlt = new();
            try
            {
                HttpClient client = new();
                HttpRequestMessage message = new()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(apiPath)
                };
                // if (accessToken != null) message.Headers.Add("Authorization", accessToken);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                message.Content = content;
                var response = await client.SendAsync(message);
                response.EnsureSuccessStatusCode();
                var contentString = await response.Content.ReadAsStringAsync();
                apiresutlt = JsonConvert.DeserializeObject<ApiResult<T>>(contentString);
                return apiresutlt;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                return new ApiResult<T>
                {
                    Message = $"خطا در عملیات: {e.Message}"
                };
            }

        }

    }
}