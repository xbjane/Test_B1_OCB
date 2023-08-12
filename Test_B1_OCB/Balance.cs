using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace Test_B1_OCB
{
    internal class Balance
    {
        public Balance() { }
        public Balance(Period period, Account account, decimal openingAmount, decimal closingAmount)
        {
            Period = period;
            Account = account;
            OpeningAmount = openingAmount;
            ClosingAmount = closingAmount;
        }
        public Balance(int balanceId, Period period, Account account, decimal openingAmount, decimal closingAmount)
        {
            BalanceId=balanceId;
            Period=period;
            Account=account;
            OpeningAmount = openingAmount;
            ClosingAmount=closingAmount;
        }
        public int BalanceId { get; set; }
        public Period Period { get;  set; }
        public Account Account { get; set; }
        [Column(TypeName = "money")]
        public decimal OpeningAmount { get; set; }
        public decimal ClosingAmount { get; set; }
    }
}
