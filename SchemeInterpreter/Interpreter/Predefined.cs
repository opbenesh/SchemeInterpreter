using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    static class Predefined
    {
        public static Dictionary<string, Value> PrimitiveValues = new Dictionary<string, Value>
        {
            {"else",new SpecialToken("else")}
        };
        public static List<PrimitiveProcedure> PrimitiveProcedures = new List<PrimitiveProcedure>
        {
            new PrimitiveProcedure("+",WrapMethod<int,int>(vl=>vl.Sum()),1,true),
            new PrimitiveProcedure("*",WrapMethod<int,int>(vl=>vl.Aggregate((i,j)=>i*j)),1,true),
            new PrimitiveProcedure("/",WrapMethod<int,int>(vl=>vl.Aggregate((i,j)=>i/j)),1,true),
            new PrimitiveProcedure("-",WrapMethod<int,int>(vl=>vl[0]-vl.Skip(1).Sum()),1,true),
            new PrimitiveProcedure("=",WrapMethod<int,bool>(vl=>vl.All(i=>i==vl[0])),1,true),
            new PrimitiveProcedure(">",WrapMethod<int,bool>(vl=>vl[0]>vl[1]),2),
            new PrimitiveProcedure("<",WrapMethod<int,bool>(vl=>vl[0]<vl[1]),2),
            
            new PrimitiveProcedure("eqv?",Eqv,2),
            new PrimitiveProcedure("eq?",Eq,2),
            
            new PrimitiveProcedure("number?",CheckPrimitiveType<int>,1),
            new PrimitiveProcedure("string?",CheckPrimitiveType<string>,1),
            new PrimitiveProcedure("symbol?",CheckType<Symbol>,1),
            new PrimitiveProcedure("pair?",CheckType<Pair>,1),
            new PrimitiveProcedure("eof-object?",CheckType<EOF>,1),

            new PrimitiveProcedure("cons",vl=>new Pair(vl[0],vl[1]),2),
            new PrimitiveProcedure("car",vl=>vl.Single().SafeCastAs<Pair>().Car.AsValue(),1),
            new PrimitiveProcedure("cdr",vl=>vl.Single().SafeCastAs<Pair>().Cdr.AsValue(),1),

            new PrimitiveProcedure("set-car!",WrapVoid(vl=>vl[0].SafeCastAs<Pair>().Car=vl[1]),2),
            new PrimitiveProcedure("set-cdr!",WrapVoid(vl=>vl[0].SafeCastAs<Pair>().Cdr=vl[1]),2),
            
            new PrimitiveProcedure("apply",Apply,2),
            

            new PrimitiveProcedure("newline",WrapVoid(vl=>Console.WriteLine()),0),
            new PrimitiveProcedure("print",WrapVoid(vl=>Console.WriteLine(vl.Single())),1),
            new PrimitiveProcedure("display",WrapVoid(vl=>Console.WriteLine(vl.Single())),1),

            new PrimitiveProcedure("read",Read,0),

            new PrimitiveProcedure("error",WrapVoid(vl=>Error(vl)),1,true)
        };

        private static Value Eqv(List<Value> vl)
        {
            return new PrimitiveWrapper<bool>() { Value = vl[0].Equals(vl[1]) };
        }

        private static Value Read(List<Value> arg)
        {
            string input = Console.ReadLine();
            if (input == null)
                return EOF.Instance;
            for (string current;!Parser.IsInputComplete(input);)
            {
                current = Console.ReadLine();
                if (current == null)
                    break;
                input += current;
            }
            try
            {
                return Parser.ParseValue(input);
            }
            catch(ParseException ex)
            {
                throw new NativeSchemeException(ex.Message);
            }
        }
        private static void Error(List<Value> values)
        {
            string error;
            if (values[0] is PrimitiveWrapper<string>)
            {
                var message = (values[0] as PrimitiveWrapper<string>).Value;
                values = values.Skip(1).ToList();
                error = string.Format("{0} \"{1}\"",message,string.Join(" ",values.Select(v=>v.SafeCastAs<Symbol>().Name)));
            }
            else
            {
                error = string.Join(" ", values.Select(v=>v.SafeCastAs<Symbol>().Name));
            }
            throw new NativeSchemeException(error);
        }
        private static Value CheckType<T>(List<Value> values) where T : Value
        {
            return new PrimitiveWrapper<bool>() { Value = values.Single() is T };
        }
        private static Value CheckPrimitiveType<T>(List<Value> values)
        {
            return new PrimitiveWrapper<bool>() { Value = values.Single() is PrimitiveWrapper<T> };
        }
        private static Value Eq(List<Value> arg)
        {
            var first = arg[0];
            var second = arg[1];
            if ((first is Symbol || first is PrimitiveWrapper))
                return new PrimitiveWrapper<bool>() { Value = first.Equals(second) };
            return new PrimitiveWrapper<bool>() { Value = ReferenceEquals(first, second) };
        }
        private static Func<List<Value>,Value> WrapVoid(Action<List<Value>> action)
        {
            return vl =>
                {
                    action(vl);
                    return Void.Instance;
                };
        }
        private static Value Apply(List<Value> args)
        {
            var closure = args[0].SafeCastAs<Closure>();
            var pair = args[1].SafeCastAs<Pair>();
            var applyArgs = Util.ToList<Value>(pair);
            return closure.Apply(applyArgs);
        }

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
