using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.IO;

using RetrosScalper.Utilities;
using RetrosScalper.Scanners;

namespace RetrosScalper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var urlsToScan = new List<Uri>();
            const string URL_FILE = "Urls.txt";

            // Creates the url file if it doesn't exist
            if (!File.Exists(URL_FILE))
            {
                File.Create(URL_FILE).Dispose();
                Console.WriteLine("Urls.txt has been created. Input all your urls to scan there line by line.");

                return;
            }

            // Reads through each line in the url file and scans them later
            using (var sr = new StreamReader(URL_FILE))
            {
                string urlLine;
                while((urlLine = sr.ReadLine()) != null)
                {
                    urlsToScan.Add(new Uri(urlLine));
                    Console.WriteLine(urlLine);
                }
            }

            var nScan = new NeweggScanner();
            var bScan = new BestBuyScanner();
            while (true)
            {
                Console.WriteLine("Getting page data...");

                foreach(var url in urlsToScan)
                {
                    bool cardInStock = false;

                    switch (url.Host)
                    {
                        case "www.bestbuy.com":
                            var htmlResponse = await HttpHelper.GetBestBuyResponse(url);
                            var html = await htmlResponse.Content.ReadAsStringAsync();
                            var items = await bScan.Scan(html);
                            foreach (var i in items)
                            {
                                if (i.InStock)
                                {
                                    Console.WriteLine("Name: " + i.Name);
                                    Console.WriteLine("Is in stock: " + i.InStock.ToString());
                                    Console.WriteLine("URL: " + i.URL);
                                    Console.WriteLine("Price: " + (i.Price == null ? "N/A" : "$" + i.Price.ToString()) + "\n");

                                    cardInStock = true;
                                }
                            }

                            if (cardInStock)
                            {
                                Console.Beep();
                            }

                            htmlResponse.Dispose();
                            break;
                        case "www.newegg.com":
                            var bhtmlResponse = await HttpHelper.GetNeweggResponse(url);
                            var bhtml = await bhtmlResponse.Content.ReadAsStringAsync();
                            var bitems = await nScan.Scan(bhtml);
                            foreach (var i in bitems)
                            {
                                if (i.InStock)
                                {
                                    Console.WriteLine("Name: " + i.Name);
                                    Console.WriteLine("Is in stock: " + i.InStock.ToString());
                                    Console.WriteLine("URL: " + i.URL);
                                    Console.WriteLine("Price: " + (i.Price == null ? "N/A" : "$" + i.Price.ToString()) + "\n");

                                    cardInStock = true;
                                }
                            }

                            if (cardInStock)
                            {
                                Console.Beep();
                            }

                            bhtmlResponse.Dispose();
                            break;
                        default:
                            Console.WriteLine("Invalid URL");
                            return;
                    }

                    Thread.Sleep(250);
                }

                Console.WriteLine("Finished getting page data...\n");
                Thread.Sleep(2000);
            }
        }
    }
}