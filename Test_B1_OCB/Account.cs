using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_OCB
{
    internal class Account
    {
        public Account() { }
        public Account(int accountNumber, string className, bool IsActive) 
        {
            AccountNumber=accountNumber;
            ClassName=className;
            AccountType = IsActive ? "Active" : "Passive";
        }
        public int AccountId { get; private set; }
        public int AccountNumber { get; private set; }
        public string AccountType { get;private set; }
        public string ClassName { get; private set; }
    }
}
