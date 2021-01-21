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
            http://data.fixer.io/api/latest?access_key=a811a7a4f347a1c280eaf781ed121ccb

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

        public bool CheckCurrency(string inputName, CurrencyConverter converter, int iteration)
        {
            bool currencyExists = false;
            foreach (Currency aCurrency in CurrenciesList)
            {
                if (aCurrency.Name.Equals(inputName))
                {
                    Console.WriteLine("Selected currency is: " + aCurrency.Name); 
                    if (iteration == 1)
                    {
                        currencyExists = true;
                        converter.FromCurrency = aCurrency;                        
                    }
                    //Checks if toCurrency is a duplicate of fromCurrency
                    else if (iteration == 2 && inputName != converter.FromCurrency.Name)
                    {
                        currencyExists = true;
                        converter.ToCurrency = aCurrency;                        
                    }
                }                             
            }
            return currencyExists;
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
        public bool doubleParse(string inputValue)
        {
            if (!double.TryParse(inputValue, out amount))
            {                
                return false;
            }
            else
            {                
                return true;
            }

        }
    }    

    public class Run
    {
        ListCurrencies test = new ListCurrencies();
        CurrencyConverter converter = new CurrencyConverter();
        public void Start()
        {
            test.AddCurrencies();
            Console.WriteLine("Welcome to the currency calculator");
            test.AvailiableCurrencies();
            From();
        }
        public void From()
        {
            Console.WriteLine("Input currency code you converting from?");            
            if(!test.CheckCurrency(Console.ReadLine(), converter, 1))
            {
                Console.WriteLine("Invalid currency, please select from the list of currencies");
                From();
            }
            else
            {
                To();
            }
        }
        public void To()
        {
            Console.WriteLine("Input currency code you converting to?");
            if (!test.CheckCurrency(Console.ReadLine(), converter, 2))
            {
                Console.WriteLine("Invalid currency, please select from the list of currencies. You cannot convert to the same currency as you convert from");
                To();
            }
            else
            {
                Amount();
            }
        }
        public void Amount()
        {
            Console.WriteLine("What amount are you converting?");
            if (!converter.doubleParse(Console.ReadLine())){
                Console.Error.WriteLine("Invalid number, please try again");
                Amount();
            }
            else
            {
                Console.WriteLine("Selected amount is " + converter.Amount);
                Console.WriteLine(converter.FromCurrency);
                Console.WriteLine(converter.ToCurrency);
                Console.WriteLine(converter.Amount);
            }
        }
    }

    class Program
    {        
        static void Main(string[] args)
        {
            Run runprogram = new Run();
            runprogram.Start();
          
            Console.ReadLine();

        }
    }
}
