using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_OCB
{
    public class View
    {//класс для отображения модели в ListView
        public View(int accountNumber, decimal openingBalance, decimal debitAmount, decimal creditAmount, decimal closingBalance, string className, DateTime lastDate, DateTime startDate)
        {
            AccountNumber = accountNumber;
            OpeningBalance = openingBalance;
            Debit = debitAmount;
            Credit = creditAmount;
            ClosingBalance = closingBalance;
            ClassName = className;
            StartDate = startDate;
            LastDate = lastDate;
        }
        public int AccountNumber { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal ClosingBalance { get; set; }
        public string ClassName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastDate { get; set; }

    }
}
