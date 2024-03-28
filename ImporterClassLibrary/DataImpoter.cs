using ClosedXML.Excel;
using System;
using System.IO;

namespace ImporterClassLibrary
{
    public static class DataImpoter
    {
        public static void ConvertToCsvToExcel() 
        {
            
            Console.WriteLine("Starting operation ");
            string csvFilePath = @"C:\Users\tafad\OneDrive\Documents\data.csv";
            string excelFilePath = @"C:\Users\tafad\OneDrive\Documents\data.xlsx";
            //"data.xlsx";

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Data");

                    using (var reader = new StreamReader(csvFilePath))
                    {
                        int row = 1;
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string[] values = line.Split(',');

                            for (int i = 0; i < values.Length; i++)
                            {
                                worksheet.Cell(row, i + 1).Value = values[i];
                            }

                            row++;
                        }
                    }

                    workbook.SaveAs(excelFilePath);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"There was an error comverting file{ex.Message}"); ;
            }
            
            
            Console.WriteLine("completed Operation ");
        }
    }
}

