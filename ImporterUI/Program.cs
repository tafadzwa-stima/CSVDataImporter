using DataImporterCL.DAL;
using System;

namespace ImporterUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
			try
			{
				DataImporter.ConvertCsvToExcel();
			}
			catch (System.Exception ex)
			{

				Console.WriteLine(ex.Message);
			}

        }
    }
}
