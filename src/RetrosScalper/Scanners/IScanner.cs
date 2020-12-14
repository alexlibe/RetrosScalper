using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RetrosScalper.Data;

namespace RetrosScalper.Scanners
{
    public interface IScanner
    {
        Task<List<IItem>> Scan(string html); 
    }
}
