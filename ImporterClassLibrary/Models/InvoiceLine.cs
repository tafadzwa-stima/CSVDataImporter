using System.ComponentModel.DataAnnotations;

namespace ImporterClassLibrary.Models
{
    public class InvoiceLine
    {
        [Key]
        public int LineId { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
        public double? Quantity { get; set; }
        public double? UnitSellingPriceExVAT { get; set; }
    }
}
