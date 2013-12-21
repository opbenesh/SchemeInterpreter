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
            new PrimitiveProcedure("+",WrapMethod<int,int>(vl=>vl.Sum()),1,true),
            new PrimitiveProcedure("*",WrapMethod<int,int>(vl=>vl.Aggregate((i,j)=>i*j)),1,true),
            new PrimitiveProcedure("-",WrapMethod<int,int>(vl=>vl[0]-vl.Skip(1).Sum()),1,true),
            new PrimitiveProcedure("=",WrapMethod<int,bool>(vl=>vl.All(i=>i==vl[0])),1,true),
            new PrimitiveProcedure(">",WrapMethod<int,bool>(vl=>vl[0]>vl[1]),2,false),
            new PrimitiveProcedure("<",WrapMethod<int,bool>(vl=>vl[0]<vl[1]),2,false),

            new PrimitiveProcedure("cons",vl=>new Pair(vl[0],vl[1]),2,false),
            new PrimitiveProcedure("car",vl=>vl.Single().SafeCastAs<Pair>().Car.AsValue(),1,false),
            new PrimitiveProcedure("cdr",vl=>vl.Single().SafeCastAs<Pair>().Cdr.AsValue(),1,false)
        };

        private static List<T> ExtractListValues<T>(List<Value> list)
        {
            return list.Select(v => v.SafeCastAs<PrimitiveWrapper<T>>().Value).ToList();
        }
        private static Func<List<Value>,Value> WrapMethod<TValue,TResult>(Func<List<TValue>,TResult> func)
        {
            return vl=> new PrimitiveWrapper<TResult>() {Value= func(ExtractListValues<TValue>(vl.Cast<Value>().ToList()))};

        }
    
    }
}
