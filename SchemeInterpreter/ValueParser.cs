using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    abstract class ValueParser
    {
        public abstract bool TryParse(string str, out Value value);
    }

    class IntegerParser : ValueParser
    {
        public override bool TryParse(string str, out Value value)
        {
            value = null;
            int res;
            if (int.TryParse(str, out res))
            {
                value = new Integer() { Value = res };
                return true;
            }
            return false;
        }
    }

    class StringParser : ValueParser
    {
        public override bool TryParse(string str, out Value value)
        {
            value = null;
            if (str[0] != '"')
                return false;
            bool escaped = false;
            int i;
            for (i=0; i<str.Length && (escaped || str[i] != '"'); i++)
            {
                if (str[i] == '\\')
                    escaped = !escaped;
            }
            if(i==str.Length)
                throw new ParseException(str,"A string must end in '\"'");
            value = new String() { Value = str.Substring(1, str.Length - 2) };
            return true;
        }
    }
}
