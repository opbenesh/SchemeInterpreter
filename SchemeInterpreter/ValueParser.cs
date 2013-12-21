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
                value = new PrimitiveWrapper<int>() { Value = res };
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
            for (i = 0; i < str.Length && (escaped || str[i] != '"'); i++)
            {
                if (str[i] == '\\')
                    escaped = !escaped;
            }
            if (i == str.Length)
                throw new ParseException(str, "A string must end in '\"'");
            value = new PrimitiveWrapper<string>() { Value = str.Substring(1, str.Length - 2) };
            return true;
        }
    }
    class BooleanParser : ValueParser
    {
        public override bool TryParse(string str, out Value value)
        {
            value = null;
            if (str == "#t")
            {
                value = new PrimitiveWrapper<bool>() { Value = true };
                return true;
            }
            if (str == "#f")
            {
                value = new PrimitiveWrapper<bool>() { Value = false };
                return true;
            }
            return false;
        }
    }
}
