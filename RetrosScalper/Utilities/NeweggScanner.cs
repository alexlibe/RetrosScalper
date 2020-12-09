using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RetrosScalper.Data;

using AngleSharp;

namespace RetrosScalper.Utilities
{
    class NeweggScanner
    {
        public static async Task<List<NeweggItem>> Scan(string html)
        {
            var nItems = new List<NeweggItem>();
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(html));

            var items = document.QuerySelectorAll("div.item-container"); // Gets the item list
            foreach (var item in items)
            {
                NeweggItem nItem = new NeweggItem();

                var priceElementTree = item.QuerySelector("li.price-current"); // Gets the item price
                if (priceElementTree.TextContent != string.Empty)
                {
                    string dollars = priceElementTree.QuerySelector("strong").TextContent; // Gets the dollar price
                    string cents = priceElementTree.QuerySelector("sup").TextContent; // Gets the cent price

                    nItem.Price = float.Parse(dollars + cents);
                }

                string outOfStockText = item.QuerySelector("p.item-promo")?.TextContent; // Checks if the "Out of stock" message is showing
                if (outOfStockText == null)
                {
                    nItem.InStock = true;
                }
                else
                {
                    nItem.InStock = false;
                }

                var linkElement = (AngleSharp.Html.Dom.IHtmlAnchorElement)item.QuerySelector("a.item-title");
                nItem.Name = linkElement.TextContent; // The name of the item
                nItem.URL = linkElement.Href;
                nItems.Add(nItem);
            }

            return nItems;
        }
    }
}
