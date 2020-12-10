using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

using RetrosScalper.Utilities;
using RetrosScalper.Scanners;

namespace RetrosScalper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Uri NEWEGG_RTX3000_RX6800_URL = new Uri(@"https://www.newegg.com/p/pl?N=100007709%20601359422%20601357250%20601357247&PageSize=96&Order=0");
            Uri BESTBUY_URL = new Uri(@"https://www.bestbuy.com/site/searchpage.jsp?st=graphics+card&_dyncharset=UTF-8&_dynSessConf=&id=pcat17071&type=page&sc=Global&cp=1&nrp=&sp=&qp=&list=n&af=true&iht=y&usc=All+Categories&ks=960&keys=keys");
            Uri AMD_URL = new Uri(@"https://www.amd.com/en/direct-buy/us");
            Uri Test_url = new Uri(@"https://www.evga.com/");
            var nScan = new NeweggScanner();

            while (true)
            {
                Console.WriteLine("Getting page data...");

                var htmlResponse = await HttpHelper.GetContentResponse(BESTBUY_URL);
                var items = await nScan.Scan(htmlResponse);
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