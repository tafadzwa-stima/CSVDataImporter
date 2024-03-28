using System;
using ImporterClassLibrary;

namespace ImporterUI
{
    internal class Program
    {
        static void Main(string[] args)
        {


            DataImpoter.ConvertToCsvToExcel();
            Console.ReadKey();


        }
    }
}
