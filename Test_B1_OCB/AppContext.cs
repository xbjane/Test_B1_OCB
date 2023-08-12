using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Test_B1_OCB
{
    internal class AppContext: DbContext
    {
        private static AppContext context;
        public AppContext() : base("DefaultConnection")
        {

        }
        public static AppContext GetContext()
        {
            if(context == null)
            {
                context = new AppContext();
            }
            return context;
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CashFlow> CashFlows { get; set; }
        public DbSet<Period> Periods { get; set; } 
        public DbSet<Balance> Balances { get; set; }
    }
}
