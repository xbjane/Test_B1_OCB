using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_OCB
{
    internal class Period
    {
        public Period() { }
        public Period(DateTime startDate, DateTime endDate) 
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public Period(int periodId, DateTime startDate, DateTime endDate)
        {
            PeriodId= periodId;
            StartDate = startDate;
            EndDate = endDate;
        }
        public int PeriodId { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
    }
}
