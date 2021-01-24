using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Currency
    {
        public string Name { get; set; }
        public double Rate { get; set; }
        public Currency(string inputName, double inputRate)
        {
            Name = inputName;
            Rate = inputRate;
        }
    }
}
