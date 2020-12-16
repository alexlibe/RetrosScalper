using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.IO;

using RetrosScalper.Utilities;
using RetrosScalper.Data;

// TODO: Setup events for the *UI* class and exceptions

namespace RetrosScalper
{
    public delegate void ScanStarted();

    class StockBot
    {
        private List<Uri> urlsToScan;

        public async Task Start()
        {
            if (!LoadUrls())
            {
                return;
            }
            
            while (true)
            {
                Console.WriteLine("Getting page data...");
                foreach (var url in urlsToScan)
                {
                    bool cardInStock = false;
                    List<IItem> items = new List<IItem>();

                    switch (url.Host)
                    {
                        case "www.bestbuy.com":
                            using (var htmlResponse = await HttpHelper.GetResponse(url))
                            {
                                string html = await htmlResponse.Content.ReadAsStringAsync();
                                items = await StockScanner.ScanBestBuy(html);
                            }

                            break;
                        case "www.newegg.com":
                            using (var htmlResponse = await HttpHelper.GetResponse(url))
                            {
                                var html = await htmlResponse.Content.ReadAsStringAsync();
                                items = await StockScanner.ScanNewegg(html);
                            }

                            break;
                        default:
                            Console.WriteLine("Invalid URL");
                            return;
                    }

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

                    Thread.Sleep(250);
                }

                Console.WriteLine("Finished getting page data...\n");
                Thread.Sleep(2000);
            }
        }

        private bool LoadUrls()
        {
            const string URL_FILE = "Urls.txt";

            // Creates the url file if it doesn't exist
            if (!File.Exists(URL_FILE))
            {
                File.Create(URL_FILE).Dispose();
                Console.WriteLine(URL_FILE + " has been created. Input all your urls to scan there line by line.");

                return false;
            }

            // Reads through each line in the url file and scans them later
            urlsToScan = new List<Uri>();
            using (var sr = new StreamReader(URL_FILE))
            {
                string urlLine;
                while ((urlLine = sr.ReadLine()) != null)
                {
                    urlsToScan.Add(new Uri(urlLine));
                    Console.WriteLine(urlLine + '\n');
                }
            }

            return true;
        }
    }
}
