using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RetrosScalper.Utilities
{
    static class HttpHelper
    {
        private static HttpClient client;

        static HttpHelper()
        {
            client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            client.Timeout = TimeSpan.FromSeconds(15);
            client.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.5");
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:83.0) Gecko/20100101 Firefox/83.0");
            client.DefaultRequestHeaders.Add("connection", "keep-alive");
            client.DefaultRequestHeaders.Add("te", "Trailers");
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
            client.DefaultRequestHeaders.CacheControl.MaxAge = TimeSpan.Zero;
        }

        public static async Task<HttpResponseMessage> GetResponse(Uri url)
        {
            HttpResponseMessage response = null;
            client.DefaultRequestHeaders.Host = url.Host;

            try
            {
                response = await client.GetAsync("https://www.bestbuy.com/fgf");
                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);

                if (response != null)
                {
                    response.Dispose();
                }
            }

            return null;
        }
    }
}
