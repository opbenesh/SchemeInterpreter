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
            new PrimitiveProcedure("+",vl=>new Integer(){Value=vl.Sum(v=>v.SafeCastAs<Integer>().Value)},1,true),
            new PrimitiveProcedure("*",vl=>new Integer(){Value=vl.Select(v=>v.SafeCastAs<Integer>().Value).Aggregate((i,j)=>i*j)},1,true),
            new PrimitiveProcedure("-",vl=>new Integer(){Value=vl[0].SafeCastAs<Integer>().Value -vl.Skip(1).Sum(v=>v.SafeCastAs<Integer>().Value)},1,true),
            new PrimitiveProcedure("=",vl=>new Boolean(){Value=vl.Select(v=>v.SafeCastAs<Integer>().Value).All(i=>i==vl[0].SafeCastAs<Integer>().Value)},1,true),
            new PrimitiveProcedure("cons",vl=>new Pair(vl[0],vl[1]),2,false),
            new PrimitiveProcedure("car",vl=>vl.Single().SafeCastAs<Pair>().Car.AsValue(),1,false),
            new PrimitiveProcedure("cdr",vl=>vl.Single().SafeCastAs<Pair>().Cdr.AsValue(),1,false)
        };

    
    }
}
