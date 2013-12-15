using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SchemeInterpreter
{
    static class Parser
    {
        private static List<ValueParser> ValueParsers = new List<ValueParser>()
        {
            new IntegerParser(),
            new StringParser()
        };

        public static Expression ParseExpression(string expression)
        {
            expression = expression.Trim();
            var captured = CaptureToken(expression);
            return ParseExpression(captured);
        }

        private static Expression ParseExpression(CapturedToken captured)
        {
            if (captured is SimpleToken)
                return ParseSimpleToken(captured as SimpleToken);
            return ParseSExpression(captured as SExpression);
        }

        private static Expression ParseSExpression(SExpression sExpression)
        {
            var car = sExpression.Tokens.First();
            if (sExpression.Tokens.First() is SimpleToken)
            {
                var f
            }
            var expressions = sExpression.Tokens.Select(ct=>ParseExpression(ct)).ToList();
            return new Application(expressions);
        }

        private static Value ParseSimpleToken(SimpleToken simpleToken)
        {
            Value value;
            foreach (var parser in ValueParsers)
                if (parser.TryParse(simpleToken.Value, out value))
                    return value;
            throw new ParseException(simpleToken.Value, "Unknown token");
        }
        private static CapturedToken CaptureToken(string expression)
        {
            int index=0;
            expression = expression.Trim();
            return CaptureToken(expression, ref index);
        }

        public static bool IsInputComplete(string input)
        {
            throw new NotImplementedException();
        }

        public static int CalcPadding(string input)
        {
            throw new NotImplementedException();
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
            return new SimpleToken(new string(str.Skip(i).TakeWhile(c => !char.IsWhiteSpace(c)).ToArray()));
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
            List<CapturedToken> tokens = new List<CapturedToken>();
            index++;
            int parens=1;
            int init=index;
            for (; parens > 0 && !char.IsWhiteSpace(str[index]); index++)
            {
                if (str[index] == '(')
                    parens++;
                else if (str[index] == ')')
                {
                    if (parens == 0)
                        throw new ParseException(SliceString(str,init, index), "Unmatched ')'");
                    parens--;
                }
            }
            return new SExpression(ExtractTokens(SliceString(str, init, index-1)));
        }

        private static string SliceString(string str, int start, int end)
        {
            return str.Substring(start, end - start + 1);
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
    }

    class SExpression : CapturedToken
    {
        public CapturedToken[] Tokens { get; private set; }
        public SExpression(CapturedToken[] tokens)
        {
            this.Tokens = tokens;
        }
    }
}
