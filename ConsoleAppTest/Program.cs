using ConsoleAppTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleAppTest
{
    public class Currency
    {
        private string name;
        private string rate;
        public Currency(string inputName, string inputRate)
        {
            name = inputName;
            rate = inputRate;
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        public override string ToString()
        {
            return Name + "  :  " + Rate;
        }
    }

    public class ListCurrencies
    {
        public List<Currency> CurrenciesList = new List<Currency>();
               
        /*public void AddCurrencies()
        {
            //http://data.fixer.io/api/latest?access_key=a811a7a4f347a1c280eaf781ed121ccb&symbols = GBP, EUR, USD, NOK;

            Currency NOK = new Currency("NOK", 1);
            Currency SEK = new Currency("SEK", 2);
            Currency USD = new Currency("USD", 3);
            CurrenciesList.Add(NOK);
            CurrenciesList.Add(SEK);
            CurrenciesList.Add(USD);
        }*/
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
            //test.AddCurrencies();
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
    public class WhatIWant
    {
        [JsonConverter(typeof(RateConverter))]
        public List<Currency> Currencies { get; set; }

        public WhatIWant (List<Currency> testList)
        {
            this.Currencies = testList;
        }
                 
    }

    public class RateConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var Currency in (List<Currency>)value)
            {
                writer.WriteRawValue(JsonConvert.SerializeObject(Currency));
            }
            writer.WriteEndArray();
        }

        // This is when you're reading the JSON object and converting it to C#
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var response = new List<Currency>();
            // Loading the JSON object
            JObject rates = JObject.Load(reader);
            // Looping through all the properties. C# treats it as key value pair
            foreach (var rate in rates)
            {
                // Finally I'm deserializing the value into an actual Player object
                var p = JsonConvert.DeserializeObject<Currency>(rate.Value.ToString());
                // Also using the key as the player Id
                p.Name = rate.Key;
                response.Add(p);
            }
            Console.WriteLine(response);
            return response;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(List<Currency>);
    }
    public class GetCurrencies
    {
        public async void LoadCurrencies()
        {
            HttpClient client = new HttpClient();

            string url = "http://data.fixer.io/api/latest?access_key=a811a7a4f347a1c280eaf781ed121ccb";
            var response = await client.GetAsync(string.Format(url));

            string result = await response.Content.ReadAsStringAsync();

            JObject data = JObject.Parse(result);

            Console.WriteLine(data["rates"]["USD"]);
            /*var test = data.Value<JObject>("rates").Properties();

            var test2 = test.ToDictionary(
                    k => k.Name,
                    v => v.Value.ToString()
            );*/


        }
        /*public async void LoadCurrencies()
        {
            string url = "http://data.fixer.io/api/latest?access_key=a811a7a4f347a1c280eaf781ed121ccb";
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    //string GiveIt = await response.Content.ReadAsStringAsync();
                    
                    WhatIWant GiveIt = await response.Content.ReadAsAsync<WhatIWant>();

                    //System.Threading.Thread.Sleep(20000);

                    string outString = "";
                    foreach (Currency aCurrency in GiveIt.Currencies)
                    {
                        outString += aCurrency.Name + "  :  " + aCurrency.Rate + "\n";
                    }
                    Console.WriteLine(outString);
                }
                else
                {
                    Console.WriteLine("did not work");
                }
            }
        }*/
    }

    class Program
    {        
        static void Main(string[] args)
        {

            //Run runprogram = new Run();
            //runprogram.Start();

            //ApiHelper.InitializeClient();
            GetCurrencies GetThem = new GetCurrencies();
            GetThem.LoadCurrencies();
            

            Console.ReadLine();

        }       
    }
}
