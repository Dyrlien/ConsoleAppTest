using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Run
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
            else if (Re.IsMatch(date))
            {
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
            if (!Check.CheckCurrency(Console.ReadLine(), Converter, 1))
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
            if (!Converter.DoubleParse(Console.ReadLine()))
            {
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
}
