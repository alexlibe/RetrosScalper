using System.Threading.Tasks;

using NUnit.Framework;

using RetrosScalper;

namespace RetrosScalper.Tests
{
    [TestFixture]
    public class ScannerFactoryTests
    {
        [Test]
        public void CreateScanner_ValidUrl_ReturnsProperScanner()
        {
            ScannerFactory.CreateScanner();
        }
    }
}
