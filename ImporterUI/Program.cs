using System;
using System.Threading.Tasks;
using ImporterClassLibrary;
using ImporterClassLibrary.DAL;

namespace ImporterUI
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //todo 
            //User can be prompted to enter the csv file path to make the program more frienldy 
            DataImpoter.ConvertToCsvToExcel();
            Console.WriteLine("---------------------");

            DataOperation.ImportDataFromExcel();
            Console.WriteLine("finished import data from excel ");

            Console.WriteLine("------------------------------");
            DataOperation.DisplayInvoiceSummaries();
            Console.WriteLine("------------------------------");
            DataOperation.VerifyBalance();
            Console.WriteLine("Press Enter to Exit Program");
            Console.ReadKey();


        }
    }
}
