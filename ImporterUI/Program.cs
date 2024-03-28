using System;
using ImporterClassLibrary;
using ImporterClassLibrary.DAL;

namespace ImporterUI
{
    internal class Program
    {
        static void Main(string[] args)
        {


            //DataImpoter.ConvertToCsvToExcel();

            DataOperation.ImportDataFromExcel();
            Console.ReadKey();


        }
    }
}
