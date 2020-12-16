using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RetrosScalper.Data;

using AngleSharp;
using AngleSharp.Html.Dom;

namespace RetrosScalper.Utilities
{
    static class StockScanner
    {
        public static async Task<List<IItem>> ScanNewegg(string html)
        {
            var neweggItems = new List<IItem>();
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(html));

            var items = document.QuerySelectorAll("div.item-container"); // Gets the item list
            foreach (var item in items)
            {
                IItem neweggItem = new NeweggItem();

                var priceElementTree = item.QuerySelector("li.price-current"); // Gets the item price
                if (priceElementTree.TextContent != string.Empty)
                {
                    string dollars = priceElementTree.QuerySelector("strong").TextContent; // Gets the dollar price
                    string cents = priceElementTree.QuerySelector("sup").TextContent; // Gets the cent price

                    neweggItem.Price = float.Parse(dollars + cents);
                }

                string outOfStockText = item.QuerySelector("p.item-promo")?.TextContent; // Checks if the "Out of stock" message is showing
                if (outOfStockText == null)
                {
                    neweggItem.InStock = true;
                }
                else
                {
                    neweggItem.InStock = false;
                }

                var linkElement = (AngleSharp.Html.Dom.IHtmlAnchorElement)item.QuerySelector("a.item-title");
                neweggItem.Name = linkElement.TextContent; // The name of the item
                neweggItem.URL = linkElement.Href;
                neweggItems.Add(neweggItem);
            }

            return neweggItems;
        }

        public static async Task<List<IItem>> ScanBestBuy(string html)
        {
            var bestBuyItems = new List<IItem>();
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(html));

            var items = document.QuerySelectorAll("li.sku-item"); // Gets the item list
            foreach (var item in items)
            {
                IItem bestBuyItem = new BestBuyItem();

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
                    bestBuyItem.InStock = true;
                }
                else
                {
                    bestBuyItem.InStock = false;
                }

                bestBuyItem.Name = gpuLinkElement.TextContent;
                bestBuyItem.Price = gpuPrice;
                bestBuyItem.URL = gpuFullLink;

                bestBuyItems.Add(bestBuyItem);
            }

            return bestBuyItems;
        }
    }
}
