using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.IO;

using RetrosScalper.Utilities;
using RetrosScalper.Data;

namespace RetrosScalper
{
    class StockBot
    {
        public enum Error
        {
            URL_NOT_SUPPORTED = 1,
            FILE_NOT_CREATED = 2
        }

        public delegate void ScanEvent();
        public delegate void BotErrorEvent(Error botError);
        public delegate void ScanDataEvent(IList<IItem> itemsFound);

        public event ScanEvent ScanStarted;
        public event ScanEvent ScanFinished;
        public event BotErrorEvent ErrorEvent;
        public event ScanDataEvent StockDataEvent;

        private List<Uri> urlsToScan;

        public async Task Start()
        {
            if (!LoadUrls())
            {
                return;
            }
            
            while (true)
            {
                ScanStarted?.Invoke();
                foreach (var url in urlsToScan)
                {
                    bool cardInStock = false;
                    List<IItem> items;

                    switch (url.Host)
                    {
                        case "www.bestbuy.com":
                            using (var htmlResponse = await HttpHelper.GetResponse(url))
                            {
                                var html = await htmlResponse.Content.ReadAsStringAsync();
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
                            ErrorEvent?.Invoke(Error.URL_NOT_SUPPORTED);
                            return;
                    }

                    StockDataEvent?.Invoke(items);
                    Thread.Sleep(250);
                }

                ScanFinished?.Invoke();
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
                ErrorEvent?.Invoke(Error.FILE_NOT_CREATED);

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
                }
            }

            return true;
        }
    }
}
