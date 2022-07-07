using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Raden_Booster
{
    public class VarDump
    {
        public static void var_dump(object obj)
        {
            if (obj is List<String>)
            {
                foreach (object o in ((List<String>)obj))
                {
                    Debug.WriteLine(o.ToString());
                }
            }
            else if (obj is List<int>)
            {
                foreach (object o in ((List<int>)obj))
                {
                    Debug.WriteLine(o.ToString());
                }
            }

        }
    }
}
