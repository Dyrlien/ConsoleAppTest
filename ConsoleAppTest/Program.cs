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
