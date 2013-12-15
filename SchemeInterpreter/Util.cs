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
    }
}
