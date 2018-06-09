namespace Demo.QuickPay.Data.Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class QuickPayDb : DbContext
    {
        public QuickPayDb()
            : base("name=QuickPayDb")
        {
        }

        public virtual DbSet<Fee> Fees { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
