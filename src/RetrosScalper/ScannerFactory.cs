using System;
using System.Collections.Generic;
using System.Text;

using RetrosScalper.Scanners;
using RetrosScalper.Data;

namespace RetrosScalper
{
    public static class ScannerFactory
    {
        public static IScanner CreateScanner()
        {
            return new NeweggScanner();
        }
    }
}
