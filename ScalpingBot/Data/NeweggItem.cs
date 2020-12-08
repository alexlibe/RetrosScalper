using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalpingBot.Data
{
    class NeweggItem
    {
        public string Name { get; set; }
        public float? Price { get; set; } = null;
        public bool InStock { get; set; }
        public string SKU { get; set; }
        public string URL { get; set; }
    }
}
