using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_OCB
{
    internal class ClassInfo
    {
        public ClassInfo(int row, string note)
        {
            Row = row;
            Note = note;
        }
        public int Row { get; set; }
        public string Note { get; set; }
    }
}
