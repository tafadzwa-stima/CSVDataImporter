using ImporterClassLibrary.Models;
using System.Data.Entity;

namespace ImporterClassLibrary.DAL
{
    public class InvoiceContext: DbContext
    {

        public InvoiceContext() : base("InvoiceContext")
        {

        }

        public DbSet<InvoiceHeader> InvoiceHeaders { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InvoiceHeader>().HasKey(i => i.InvoiceId);
            modelBuilder.Entity<InvoiceLine>().HasKey(i => i.LineId);
        }

    }
}
