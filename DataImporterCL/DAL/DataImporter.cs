using ClosedXML.Excel;
using System;
using System.IO;

namespace DataImporterCL.DAL
{
    public static class DataImporter
    {
        public static void ConvertCsvToExcel()
        {

            Console.WriteLine("Starting operation ");
            string csvFilePath = @"C:\src\Decofurn\DecofurnDataImporter\datasample.csv";
            string excelFilePath = @"C:\src\Decofurn\DecofurnDataImporter\data.xlsx";
            
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
            Console.WriteLine("completed Operation ");
        }
    }
}

