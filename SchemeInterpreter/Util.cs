using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    static class Util
    {
        public static Pair ToSchemeList(IEnumerable<Expression> list)
        {
            if (!list.Any())
                throw new InternalException("Cannot create a scheme list from an empty list");
            return list.Reverse().Skip(1).Aggregate(new Pair(list.Last(),Null.Instance),(acc, x) => new Pair(x, acc));
        }
        public static List<T> ToList<T>(Pair list) where T : Expression
        {
            var result = new List<T>();
            for (Expression current = list; !(current is Null) ; current=(current as Pair).Cdr)
            {
                if (!((current as Pair).Car is T))
                    throw new TypeMismatchException((current as Pair).Car, typeof(T));
                result.Add((current as Pair).Car as T);
            }
            return result;
        }
    }
}
