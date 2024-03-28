using System;
using System.ComponentModel.DataAnnotations;

namespace ImporterClassLibrary.Models
{
    public class InvoiceHeader
    {

        [Key]
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Address { get; set; }
        public double? InvoiceTotal { get; set; }
    }
}
