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
            InvoiceContext context = new InvoiceContext();

            using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Skip header row
                reader.Read();

                while (reader.Read())
                {
                    
                    
                        string invoiceNumber = reader.GetValue(0)?.ToString();
                        string address = reader.GetValue(2)?.ToString();

                        string dateString = reader.GetValue(1)?.ToString().Trim();
                        DateTime invoiceDate;

                        if (DateTime.TryParseExact(dateString, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate)
                            || DateTime.TryParseExact(dateString, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out invoiceDate))
                        {
                            // Parsing successful
                            Console.WriteLine(invoiceDate);
                        }
                        else
                        {
                            // Parsing failed
                            Console.WriteLine("Failed to parse date string: " + dateString);
                        }

                        string invoiceTotalStringValue = reader.GetValue(3)?.ToString();
                        double invoiceTotalExVAT = double.Parse(invoiceTotalStringValue.Trim(), CultureInfo.InvariantCulture);

                        string lineDescription = reader.GetValue(4)?.ToString();

                        double invoiceQuantity = double.Parse(reader.GetValue(5)?.ToString());
                        
                        string unitSellingStringVal = reader.GetValue(6)?.ToString();   
                        double unitSellingPriceExVAT = double.Parse(unitSellingStringVal.Trim(), CultureInfo.InvariantCulture);

                    // Add data to the database
                    using (var dbContext = new InvoiceContext())
                        {
                            var existingInvoice = dbContext.InvoiceHeaders.FirstOrDefault(i => i.InvoiceNumber == invoiceNumber);
                            // var eInvoive = dbContext.InvoiceHeaders.FirstOrDefault(i => i.InvoiceNumber == invoiceNumber);

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
                                dbContext.SaveChanges();
                            }

                            var newInvoiceLine = new InvoiceLine
                            {
                                InvoiceNumber = invoiceNumber,
                                Description = lineDescription,
                                Quantity = invoiceQuantity,
                                UnitSellingPriceExVAT = unitSellingPriceExVAT
                            };

                            dbContext.InvoiceLines.Add(newInvoiceLine);
                            dbContext.SaveChanges();
                            Console.WriteLine("done with import operation");
                        }
                  //  }
                   

                    
                }
            }
        }
    }
}
