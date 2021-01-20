using ConsoleAppTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class Currency
    {
        private string name;
        private double rate;
        public Currency(string inputName, double inputRate)
        {
            name = inputName;
            rate = inputRate;
        }
        public string Name
        {
            get { return name; }            
        }
        public double Rate
        {
            get { return rate; }
        }
        public override string ToString()
        {
            return Name + "  :  " + Rate;
        }
    }

    public class ListCurrencies
    {
        public List<Currency> CurrenciesList = new List<Currency>();
               
        public void AddCurrencies()
        {
            Currency NOK = new Currency("NOK", 1);
            Currency SEK = new Currency("SEK", 2);
            Currency USD = new Currency("USD", 3);
            CurrenciesList.Add(NOK);
            CurrenciesList.Add(SEK);
            CurrenciesList.Add(USD);
        }
        public void AvailiableCurrencies()
        {
            Console.WriteLine("Current availiable currencies:");
            foreach (Currency aCurrency in CurrenciesList)
            {                
                Console.WriteLine(aCurrency);
            }            
        }

        public void CheckCurrency(string inputName, CurrencyConverter converter, int iteration)
        {
            foreach (Currency aCurrency in CurrenciesList)
            {
                if (aCurrency.Name == inputName)
                {
                    Console.WriteLine("Selected currency is: " + aCurrency.Name);
                    if (iteration == 1)
                    {
                        converter.FromCurrency = aCurrency;
                    }
                    else if (iteration == 2)
                    {
                        converter.ToCurrency = aCurrency;
                    }
                }
            }
        }            
    }
       
    
    public class CurrencyConverter
    {
        private Currency fromCurrency;
        private Currency toCurrency;
        private double amount;

        public Currency FromCurrency
        {
            get { return fromCurrency; }
            set { fromCurrency = value; }
        }
        public Currency ToCurrency
        {
            get { return toCurrency; }
            set { toCurrency = value; }
        }
        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public void doubleParse(string inputValue)
        {
            if (!double.TryParse(inputValue, out amount))
            {
                Console.Error.WriteLine("Invalid input, please try again");
            }
            else
            {
                Console.WriteLine("Selected amount is " + Amount);
            }

        }
    }
    
    class Program
    {                       
        static void Main(string[] args)
        {
            ListCurrencies test = new ListCurrencies();
            CurrencyConverter test2 = new CurrencyConverter();
            test.AddCurrencies();

            Console.WriteLine("Welcome to the currency calculator");
            
            test.AvailiableCurrencies();
            Console.WriteLine("Which currency are you converting from?");
            test.CheckCurrency(Console.ReadLine(), test2, 1);
            
            Console.WriteLine("Which currency are you converting to?");
            test.CheckCurrency(Console.ReadLine(), test2, 2);

            Console.WriteLine("What amount are you converting?");
            test2.doubleParse(Console.ReadLine());

            Console.WriteLine(test2.FromCurrency);
            Console.WriteLine(test2.ToCurrency);
            Console.WriteLine(test2.Amount);

            Console.ReadLine();

        }
    }
}
