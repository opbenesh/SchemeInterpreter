using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SchemeInterpreter
{
    static class Lexer
    {
        public static CapturedToken CaptureToken(string expression)
        {
            int index=0;
            expression = expression.Trim();
            return CaptureToken(expression, ref index);
        }

        private static string NormalizeString(string str)
        {
            return str.Trim();
        }
        private static bool TryUnparen(string str, out string unparend)
        {
            unparend=null;
            if (str.Length >= 2 && str[0] == '(' && str[str.Length - 1] == ')')
            {
                unparend = str.Substring(1, str.Length - 2);
                return true;
            }
            return false;
        }
        private static CapturedToken[] ExtractTokens(string str)
        {
            var tokens = new List<CapturedToken>();
            for (int i = 0; i < str.Length; i++)
            {
                if(char.IsWhiteSpace(str[i]))
                    continue;
                tokens.Add(CaptureToken(str, ref i));
            }
            return tokens.ToArray();
        }

        private static CapturedToken CaptureToken(string str, ref int i)
        {
            if (str[i] == '"')
                return CaptureStringToken(str, ref i);
            if (str[i] == '(')
                return CaptureSExpression(str, ref i);
            return CaptureSimpleToken(str, ref i);
        }

        private static CapturedToken CaptureSimpleToken(string str, ref int i)
        {
            var sb = new StringBuilder();
            for (; i < str.Length; i++)
            {
                if (char.IsWhiteSpace(str[i]))
                    break;
                sb.Append(str[i]);
            }
            return new SimpleToken(sb.ToString());
        }

        private static CapturedToken CaptureStringToken(string str, ref int index)
        {
            int init = index++;
            bool escaped = false;
            for (; escaped || str[index]!='"'; index++)
            {
                if (str[index] == '\\')
                    escaped = !escaped;
            }
            return new SimpleToken(SliceString(str,init, index));
        }
        private static SExpression CaptureSExpression(string str, ref int index)
        {
            index++;
            int parens = 1;
            int init = index;
            for (; parens > 0 && index < str.Length; index++)
            {
                if (str[index] == '(')
                    parens++;
                else if (str[index] == ')')
                {
                    if (parens == 0)
                        throw new ParseException(SliceString(str, init, index), "Unmatched ')'");
                    parens--;
                }
            }
            if (parens > 0)
                throw new ParseException(SliceString(str, init, index-1), "Unmatched '('");
            var tokens = ExtractTokens(SliceString(str, init, index - 1));
            return new SExpression(tokens);
        }

        private static string SliceString(string str, int start, int end)
        {
            return str.Substring(start, end - start);
        }
    }

    abstract class CapturedToken
    { }
    class SimpleToken : CapturedToken
    {
        public string Value { get; private set; }
        public SimpleToken(string str)
        {
            this.Value = str;
        }
        public override string ToString()
        {
            return Value;
        }
    }

    class SExpression : CapturedToken
    {
        public CapturedToken[] Tokens { get; private set; }
        public SExpression(CapturedToken[] tokens)
        {
            this.Tokens = tokens;
        }
        public override string ToString()
        {
            return string.Format("({0})", string.Join<CapturedToken>(" ", Tokens));
        }
    }
}
