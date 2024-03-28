using ExcelDataReader;
using ImporterClassLibrary.Models;
using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ImporterClassLibrary.DAL
{
    public class DataOperation : DropCreateDatabaseIfModelChanges<InvoiceContext>
    {
        public static void ImportDataFromExcel()
        {
            Console.WriteLine("Starting import data from excel processs");
            string excelFilePath = @"C:\Users\tafad\OneDrive\Documents\data.xlsx";

            using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Skip header row
                reader.Read();

                using (var dbContext = new InvoiceContext())
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            string invoiceNumber = reader.GetValue(0)?.ToString();
                            string address = reader.GetValue(2)?.ToString();
                            DateTime invoiceDate;
                            double invoiceTotalExVAT;
                            string lineDescription;
                            double invoiceQuantity;
                            double unitSellingPriceExVAT;

                            //Parse Values
                            string dateString = reader.GetValue(1)?.ToString().Trim();
                            if (!DateTime.TryParseExact(dateString, new[] { "dd/MM/yyyy HH:mm", "yyyy/MM/dd HH:mm:ss" },
                                CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate))
                            {
                                Console.WriteLine("Failed to parse date string: " + dateString);
                                continue; 
                            }

                            
                            if (!double.TryParse(reader.GetValue(3)?.ToString().Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out invoiceTotalExVAT))
                            {
                                Console.WriteLine("Failed to parse invoice total: " + reader.GetValue(3)?.ToString().Trim());
                                continue; 
                            }

                            lineDescription = reader.GetValue(4)?.ToString();

                            
                            if (!double.TryParse(reader.GetValue(5)?.ToString().Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out invoiceQuantity))
                            {
                                Console.WriteLine("Failed to parse invoice quantity: " + reader.GetValue(5)?.ToString().Trim());
                                continue;
                            }
                                                        
                            if (!double.TryParse(reader.GetValue(6)?.ToString().Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out unitSellingPriceExVAT))
                            {
                                Console.WriteLine("Failed to parse unit selling price: " + reader.GetValue(6)?.ToString().Trim());
                                continue; 
                            }

                            var existingInvoice = dbContext.InvoiceHeaders.FirstOrDefault(i => i.InvoiceNumber == invoiceNumber);

                            if (existingInvoice == null)
                            {
                                var newInvoice = new InvoiceHeader
                                {
                                    InvoiceNumber = invoiceNumber,
                                    InvoiceDate = invoiceDate,
                                    Address = address,
                                    InvoiceTotal = invoiceTotalExVAT
                                };

                                dbContext.InvoiceHeaders.Add(newInvoice);
                            }

                            var newInvoiceLine = new InvoiceLine
                            {
                                InvoiceNumber = invoiceNumber,
                                Description = lineDescription,
                                Quantity = invoiceQuantity,
                                UnitSellingPriceExVAT = unitSellingPriceExVAT
                            };

                            dbContext.InvoiceLines.Add(newInvoiceLine);
                        }

                        dbContext.SaveChanges();
                        transaction.Commit();
                                                
                        var invoiceSummaries = dbContext.InvoiceLines
                            .GroupBy(l => l.InvoiceNumber)
                            .Select(g => new
                            {
                                InvoiceNumber = g.Key,
                                TotalQuantity = g.Sum(l => l.Quantity)
                            });

                        foreach (var summary in invoiceSummaries)
                        {
                            Console.WriteLine($"Invoice Number: {summary.InvoiceNumber}, Total Quantity: {summary.TotalQuantity}");
                        }

                        Console.WriteLine("Import operation completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("An error occurred during the import process: " + ex.Message);
                    }
                }
            }
        }

    }
}
