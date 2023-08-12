using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_B1_OCB
{
    internal class File
    {
        public File(string name, Period period)
        {
            Name = name;
            Period = period;
        }     
        public string Name { get; private set; }
        public Period Period { get; private set; }
       
    }
}
