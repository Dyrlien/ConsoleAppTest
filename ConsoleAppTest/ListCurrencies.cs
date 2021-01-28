using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class ListCurrencies
    {
        public static List<JToken> currencies = new List<JToken>();
        public string CurrentDate;

        //Retrieves currency rates for the selected date and adds them to a list
        public async Task LoadCurrencies(string date)
        {
            currencies.Clear();
            HttpClient client = new HttpClient();

            string url = "http://data.fixer.io/api/" + date + "?access_key=a811a7a4f347a1c280eaf781ed121ccb";
            var response = await client.GetAsync(string.Format(url));

            string result = await response.Content.ReadAsStringAsync();

            JObject data = JObject.Parse(result);
            CurrentDate = data["date"].ToString();            

            foreach (var i in data["rates"])
            {
                currencies.Add(i);
            }
        }
        //Prints availiable currencies (NOT IMPLEMENTED)
        public void PrintCurrencies()
        {
            foreach (var i in currencies.OfType<JProperty>())
            {
                Console.WriteLine(i.Name + "  :  " + i.Value);
            }
        }

        //Sjekker om den valgte valutaen finnes i valutalisten.
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
}
