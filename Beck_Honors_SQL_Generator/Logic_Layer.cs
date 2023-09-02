using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beck_Honors_SQL_Generator
{
    internal class Logic_Layer
    {
        public static void pass_note(String s) {
            Data_Access.appendData(s);
        }
    }
}
