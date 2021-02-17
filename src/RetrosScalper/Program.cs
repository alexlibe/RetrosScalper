using System;
using System.Threading.Tasks;

namespace RetrosScalper
{
    class Program
    {
        static void Started()
        {
            Console.WriteLine("Scan Started");
        }

        static void Finished()
        {
            Console.WriteLine("Scan Finished");
        }
        
        static async Task Main()
        {
            StockBot sb = new StockBot();
            sb.ScanStarted += Started;
            sb.ScanFinished += Finished;
            await sb.Start();
        }
    }
}