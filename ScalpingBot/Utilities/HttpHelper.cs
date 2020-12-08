using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace ScalpingBot.Utilities
{
    class HttpHelper
    {
        static HttpClient client = new HttpClient();

        public static async Task<string> GetContentResponse(string url)
        {
            try	
            {
                using(HttpResponseMessage response = await client.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }
    }
}
