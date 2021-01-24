using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class CurrencyConverter
    {
        public Currency FromCurrency { get; set; }
        public Currency ToCurrency { get; set; }
        public double Amount;
        private double result;

        public bool DoubleParse(string inputValue)
        {
            if (!double.TryParse(inputValue, out Amount))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void Convert()
        {
            result = (Amount / FromCurrency.Rate) * ToCurrency.Rate;
            Console.WriteLine(FromCurrency.Name + "  :  " + Amount);
            Console.WriteLine(ToCurrency.Name + "  :  " + result);
        }
    }
}
