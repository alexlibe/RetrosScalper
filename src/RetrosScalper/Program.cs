using System;
using System.Threading.Tasks;

// TODO: Create a method that returns the URLS
// TODO: Create a return type for Start() possibly
// TODO: Unit test
// TODO: Create a console UI
// TODO: Update the scanners to work with dependency injection
// TODO: Dependency Injection

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