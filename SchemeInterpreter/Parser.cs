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

        private static List<string> SpecialForms = new List<string>()
        {
            "if","define"
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
            var expressions = sExpression.Tokens.Select(ct => ParseExpression(ct)).ToArray() ;
            if (expressions.First() is SpecialFormToken)
                return ParseSpecialForm((expressions.First() as SpecialFormToken).Token, expressions.Skip(1).ToList(), sExpression);
            return new Application(expressions);
        }

        private static Expression ParseSpecialForm(string specialForm, List<Expression> expressions, SExpression sExpression)
        {
            if(specialForm=="if")
            {
                if (expressions.Count != 3)
                    throw new ParseException(sExpression.ToString(), "An \"if\" expression must contains exactly 3 arguments");
                return new If(expressions[0], expressions[1], expressions[2]);
            }
            if(specialForm=="define")
            {
                if (expressions.Count != 2)
                    throw new ParseException(sExpression.ToString(), "A \"define\" expression must contains exactly 2 arguments");
                if (!(expressions[0] is Variable || expressions[0] is Application))
                    throw new ParseException(sExpression.ToString(), "A \"define\" expression's first part must be either a string or a list");

                return new Define(expressions[0], expressions[1]);

            }
            throw new InternalException(string.Format("Illegal special form {0}",specialForm));
        }

        private static Expression ParseSimpleToken(SimpleToken simpleToken)
        {
            var text = simpleToken.Value;
            Value value;
            if (SpecialForms.Exists(s => s == text))
                return new SpecialFormToken(text);
            foreach (var parser in ValueParsers)
                if (parser.TryParse(text, out value))
                    return value;
            return new Variable(text);
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
            return new SExpression(ExtractTokens(SliceString(str, init, index - 1)));
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
