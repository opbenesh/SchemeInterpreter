using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    static class Util
    {
        public static Pair ToSchemeList<T>(IEnumerable<T> list) where T : Expression
        {
            var result = new Pair(null, null);
            var current = result;
            foreach (var expr in list)
            {
                current.Cdr = new Pair(expr, null);
                current = (Pair)current.Cdr;
            }
            return result;
        }
        public static List<T> ToList<T>(Pair list) where T : Expression
        {
            var result = new List<T>();
            for (Pair current = list; current!=null ; current=current.Cdr as Pair)
            {
                if (!(current.Car is T))
                    throw new TypeMismatchException(current.Car, typeof(T));
                result.Add(current.Car as T);
            }
            return result;
        }
    }
}
