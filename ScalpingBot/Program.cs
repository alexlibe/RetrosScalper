using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

using ScalpingBot.Utilities;
using ScalpingBot.Data;

namespace ScalpingBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string NEWEGG_RTX3000_RX6800_URL = @"https://www.newegg.com/p/pl?N=100007709%20601359422%20601357250%20601357247&PageSize=96&Order=0";

            while (true)
            {
                Console.WriteLine("Getting page data...");

                var htmlResponse = await HttpHelper.GetContentResponse(NEWEGG_RTX3000_RX6800_URL);
                var items = await NeweggScanner.Scan(htmlResponse);
                foreach (var i in items)
                {
                    if (i.InStock)
                    {
                        Console.WriteLine("Name: " + i.Name);
                        Console.WriteLine("Is in stock: " + i.InStock.ToString());
                        Console.WriteLine("URL: " + i.URL);
                        Console.WriteLine("Price: " + (i.Price == null ? "N/A" : "$" + i.Price.ToString()) + "\n");

                        Console.Beep();
                    }
                }

                Console.WriteLine("Finished getting page data...\n");
                Thread.Sleep(5000);
            }
        }
    }
}
