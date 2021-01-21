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


    //DU MÅ FÅ MAIN GREIENE INN I METODER SÅ DE KAN KJØRES PÅ NYTT VED FEIL I INPUT

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
            Console.WriteLine("Which currency are you converting from?");            
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
            Console.WriteLine("Which currency are you converting to?");
            if (!test.CheckCurrency(Console.ReadLine(), converter, 2))
            {
                Console.WriteLine("Invalid currency, please select from the list of currencies");
                To();
            }
            else
            {
                Console.WriteLine(converter.FromCurrency);
                Console.WriteLine(converter.ToCurrency);
            }

        }
    }

    class Program
    {        
        static void Main(string[] args)
        {
            Run runprogram = new Run();
            runprogram.Start();

           /* 
            Console.WriteLine("Which currency are you converting to?");
            test.CheckCurrency(Console.ReadLine(), test2, 2);

            Console.WriteLine("What amount are you converting?");
            test2.doubleParse(Console.ReadLine());

            Console.WriteLine(test2.FromCurrency);
            Console.WriteLine(test2.ToCurrency);
            Console.WriteLine(test2.Amount);
           */
            Console.ReadLine();

        }
    }
}
