using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Test_B1_OCB
{
    internal class CashFlow
    {
        public CashFlow() { }
        public CashFlow(DateTime transactionDate, decimal debit,decimal credit, Account account) 
        {
            TransactionDate = transactionDate;
            DebitAmount = debit;
            CreditAmount = credit;
            Account = account;
        }
        [Key]
        public int TransactionId { get; private set; }
        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; private set; }
        public Account Account { get; private set; }
       
        [Column(TypeName = "money")]
        public decimal DebitAmount { get; private set; }
        public decimal CreditAmount{get; private set; }

    }
}
