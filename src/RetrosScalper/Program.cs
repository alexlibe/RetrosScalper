using System;
using System.Threading.Tasks;

namespace RetrosScalper
{
    class Program
    {
        static async Task Main()
        {
            StockBot sb = new StockBot();
            await sb.Start();

            Console.ReadKey();
        }
    }
}