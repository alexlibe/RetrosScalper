using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RetrosScalper.Data;

using AngleSharp;
using AngleSharp.Html.Dom;

namespace RetrosScalper.Scanners
{
    class BestBuyScanner : IScanner
    {
        public async Task<List<IItem>> Scan(string html)
        {
            var nItems = new List<IItem>();
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(html));

            var items = document.QuerySelectorAll("li.sku-item"); // Gets the item list
            foreach (var item in items)
            {
                IItem bItem = new BestBuyItem();
                
                // Gets the link and name of the GPU thats been scanned
                var gpuLinkElement = (IHtmlAnchorElement)item.QuerySelector("h4.sku-header").QuerySelector("a");
                string gpuFullLink = "www.bestbuy.com" + gpuLinkElement.PathName;

                // Gets the price of the GPU by searching for the element with a dollar sign and removing that dollar sign to parse it
                var gpuPriceElement = item.QuerySelectorAll("span[aria-hidden='true']").Where(m => m.TextContent.Contains("$")).Single();
                var gpuPrice = float.Parse(gpuPriceElement.TextContent.Substring(1, gpuPriceElement.TextContent.Length - 1));

                // Checks if the GPU is in stock
                var gpuInStockElement = item.QuerySelector("div.fulfillment-add-to-cart-button").QuerySelector("button");
                if (gpuInStockElement.TextContent == "Add to Cart")
                {
                    bItem.InStock = true;
                }
                else
                {
                    bItem.InStock = false;
                }

                bItem.Name = gpuLinkElement.TextContent;
                bItem.Price = gpuPrice;
                bItem.URL = gpuFullLink;

                nItems.Add(bItem);
            }

            return nItems;
        }
    }
}
