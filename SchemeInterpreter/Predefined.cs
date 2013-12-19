using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    static class Predefined
    {
        public static List<PrimitiveProcedure> PrimitiveProcedures = new List<PrimitiveProcedure>
        {
            new PrimitiveProcedure("+",vl=>new Integer(){Value=vl.Sum(v=>v.SafeCastAs<Integer>().Value)},1,true)
        };

    
    }
}
