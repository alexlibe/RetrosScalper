using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RetrosScalper.Utilities
{
    static class HttpHelper
    {
        public static HttpClient Client { get; }

        static HttpHelper()
        {
            Client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

            Client.Timeout = TimeSpan.FromSeconds(15);
            Client.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            Client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate, br");
            Client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.5");
            Client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:83.0) Gecko/20100101 Firefox/83.0");
            Client.DefaultRequestHeaders.Add("connection", "keep-alive");
            Client.DefaultRequestHeaders.Add("te", "Trailers");
            Client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue();
            Client.DefaultRequestHeaders.CacheControl.MaxAge = TimeSpan.Zero;
        }

        public static async Task<HttpResponseMessage> GetResponse(Uri url)
        {
            HttpResponseMessage response = null;
            Client.DefaultRequestHeaders.Host = url.Host;

            try
            {
                response = await Client.GetAsync(url);
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
