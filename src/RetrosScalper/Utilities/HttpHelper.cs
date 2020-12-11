using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RetrosScalper.Utilities
{
    class HttpHelper
    {
        static HttpClient client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        public static async Task<string> GetContentResponse(Uri url)
        {
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:83.0) Gecko/20100101 Firefox/83.0");
            client.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
       //     client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.5");
            client.DefaultRequestHeaders.Add("connection", "keep-alive");
            client.DefaultRequestHeaders.Add("te", "Trailers");
            client.DefaultRequestHeaders.Add("referer", "https://www.bestbuy.com/site/searchpage.jsp?st=rtx+3080&_dyncharset=UTF-8&_dynSessConf=&id=pcat17071&type=page&sc=Global&cp=1&nrp=&sp=&qp=&list=n&af=true&iht=y&usc=All+Categories&ks=960&keys=keys");
            client.DefaultRequestHeaders.Host = "www.bestbuy.com";

            try
            {
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }
    }
}
