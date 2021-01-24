using ConsoleAppTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace ConsoleAppTest
{
    public class Currency
    {
        public string Name { get; set; }
        public double Rate { get; set; }
        public Currency(string inputName, double inputRate)
        {
            Name = inputName;
            Rate = inputRate;
        }
    }

    public class ListCurrencies
    {       
        public static List<JToken> currencies = new List<JToken>();
        public async Task LoadCurrencies(string date)
        {
            currencies.Clear();
            HttpClient client = new HttpClient();

            string url = "http://data.fixer.io/api/"+date+"?access_key=a811a7a4f347a1c280eaf781ed121ccb";
            var response = await client.GetAsync(string.Format(url));

            string result = await response.Content.ReadAsStringAsync();

            JObject data = JObject.Parse(result);            
                       
            foreach (var i in data["rates"])
            {
                currencies.Add(i);
            }                        
        }
        public void PrintCurrencies()
        {
            foreach (var i in currencies.OfType<JProperty>())
            {
                Console.WriteLine(i.Name + "  :  " +  i.Value);
            }            
        }
      
        public bool CheckCurrency(string inputName, CurrencyConverter converter, int iteration)
        {            
            bool currencyExists = false;
            double FromDouble;
            double ToDouble;
            foreach (var aCurrency in ListCurrencies.currencies.OfType<JProperty>())
            {
                if (aCurrency.Name.Equals(inputName))
                {
                    Console.WriteLine("Selected currency is: " + aCurrency.Name);
                    if (iteration == 1)
                    {
                        FromDouble = double.Parse(aCurrency.Value.ToString());
                        currencyExists = true;
                        Currency newCurrency = new Currency(aCurrency.Name, FromDouble);
                        converter.FromCurrency = newCurrency;
                    }
                    //Checks if toCurrency is a duplicate of fromCurrency
                    else if (iteration == 2 && inputName != converter.FromCurrency.Name)
                    {
                        ToDouble = double.Parse(aCurrency.Value.ToString());
                        currencyExists = true;
                        Currency newCurrency = new Currency(aCurrency.Name, ToDouble);
                        converter.ToCurrency = newCurrency;
                    }
                }
            }
            return currencyExists;
        }
    }           
    public class CurrencyConverter
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
    public class Run
    {
        ListCurrencies Check = new ListCurrencies();
        CurrencyConverter Converter = new CurrencyConverter();        
        private static string strRegex = @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))";
        Regex Re = new Regex(strRegex);
        public void Start()
        {                        
            Console.WriteLine("Select date for exchange rate, format: YYYY-MM-DD \nMake sure it does not preceed 1999-01-01 or exceeds todays date.\nOr press enter for latest exchange rates");            
            string date = Console.ReadLine();
            if (date.Equals(""))
            {
                date = "latest";
                Task task = Check.LoadCurrencies(date);
                Console.WriteLine("Updating currency rates, please wait.");
                task.Wait();
                Console.WriteLine("Currencies updated.");
                From();
            }
            else if(Re.IsMatch(date)){
                Task task = Check.LoadCurrencies(date);
                Console.WriteLine("Updating currency rates, please wait.");
                task.Wait();
                Console.WriteLine("Currencies updated.");
                From();
            }
            else
            {
                Console.WriteLine("Invalid date format, format: YYYY-MM-DD");
                Start();
            }            
        }
        public void From()
        {
            Console.WriteLine("Input currency code you are converting from. Make sure to write in uppercase");            
            if(!Check.CheckCurrency(Console.ReadLine(), Converter, 1))
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
            Console.WriteLine("Input currency code you are converting to. Make sure to write in uppercase");
            if (!Check.CheckCurrency(Console.ReadLine(), Converter, 2))
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
            if (!Converter.DoubleParse(Console.ReadLine())){
                Console.Error.WriteLine("Invalid number, please try again");
                Amount();
            }
            else
            {
                Console.WriteLine("Selected amount: " + Converter.Amount + " " + Converter.FromCurrency.Name);
                Convert();
            }
        }
        public void Convert()
        {
            Converter.Convert();
            Console.WriteLine("Press enter to start over, otherwise type QUIT");
            if (!Console.ReadLine().Equals("QUIT"))
            {
                Start();
            }
            else
            {
            }
        }
    }   
    class Program
    {        
        static void Main(string[] args)
        {
            Run runprogram = new Run();
            Console.WriteLine("Welcome to the currency calculator");
            runprogram.Start();                   
        }       
    }
}
