using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetrosScalper.Scanners
{
    public interface IScanner<T>
    {
        Task<List<T>> Scan(string html); 
    }
}
