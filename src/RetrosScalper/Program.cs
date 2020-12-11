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
            while (true)
            {
                Console.WriteLine("Getting page data...");
                bool cardInStock = false;

                foreach(var url in urlsToScan)
                {
                    var htmlResponse = await HttpHelper.GetContentResponse(url);
                    var items = await nScan.Scan(htmlResponse);
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

                    Thread.Sleep(500);
                }

                if (cardInStock)
                {
                    Console.Beep();
                }

                Console.WriteLine("Finished getting page data...\n");
                Thread.Sleep(4000);
            }
        }
    }
}