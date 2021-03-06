﻿using System;
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
            new StringParser(),
            new BooleanParser()
        };
        private static List<ValueParser> ExtendedParsers = new List<ValueParser>
        {
            new SymbolParser()
        };

        private static List<string> SpecialForms = new List<string>()
        {
            "if","define","let","let*","lambda","begin", "cond", "delay"
        };

        public static Expression ParseExpression(string expression)
        {
            expression = expression.Trim();
            var captured = Lexer.CaptureToken(expression);
            return ParseExpression(captured);
        }

        private static Expression ParseExpression(CapturedToken captured)
        {
            if (captured is SimpleToken)
                return ParseSimpleToken(captured as SimpleToken);
            var sExpression = captured as SExpression;
            return ParseSExpression(sExpression);
        }

        private static Expression ParseSExpression(SExpression sExpression)
        {
            var expressions = sExpression.Tokens.Select(ct => ParseExpression(ct)).ToArray();
            if (expressions.Any() && expressions.First() is SpecialFormToken)
                return ParseSpecialForm((expressions.First() as SpecialFormToken).Token, expressions.Skip(1).ToList(), sExpression);
            return new Application(expressions);
        }

        private static Expression ParseSpecialForm(string specialForm, List<Expression> expressions, SExpression sExpression)
        {
            if(specialForm=="if")
            {
                if (expressions.Count < 2)
                    throw new ParseException(sExpression.ToString(), "An \"if\" expression must contains at least 2 arguments");
                if (expressions.Count > 3)
                    throw new ParseException(sExpression.ToString(), "An \"if\" expression must contains at most 3 arguments");
                var falseBranch = expressions.Count == 3 ? expressions[2] : new PrimitiveWrapper<bool>() { Value = false };
                return new If(expressions[0], expressions[1], falseBranch);
            }
            if (specialForm == "define")
            {
                if (expressions[0] is Variable)
                {
                    if (expressions.Count != 2)
                        throw new ParseException(sExpression.ToString(), "A variable type \"define\" expression must contains exactly 2 arguments");
                    return new Define(expressions[0], expressions[1]);
                }
                if (expressions[0] is Application)
                {
                    if (expressions.Count == 1)
                        throw new ParseException(sExpression.ToString(), "A procedure type \"define\" expression must contains at least 2 arguments");
                    var name = expressions[0];
                    var defined = MakeBeginIfNeeded(expressions.Skip(1).ToList());
                    return new Define(name, defined);
                }
                throw new ParseException(sExpression.ToString(), "A \"define\" expression's first part must be either a string or a list");
            }
            if (specialForm == "lambda")
            {
                if (expressions.Count == 1)
                    throw new ParseException(sExpression.ToString(), "A \"lambda\" expression must contains at least 2 arguments"); 
                var defined = MakeBeginIfNeeded(expressions.Skip(1).ToList());
                if (!(expressions[0] is Application))
                    throw new ParseException(sExpression.ToString(), "A \"lambda\" expression's first part must be a list");
                var formalArgs = (expressions[0] as Application).Expressions;
                if (formalArgs.Any(e => !(e is Variable)))
                    throw new ParseException(sExpression.ToString(), "A \"lambda\" expression's first part must be a list of arguments");
                return new Lambda((expressions[0] as Application).Expressions.Cast<Variable>().ToArray(), defined);
            }
            if (specialForm == "let" || specialForm == "let*")
            {
                if (expressions.Count < 2)
                    throw new ParseException(sExpression.ToString(), string.Format("A \"{0}\" expression must contains at least 2 arguments", specialForm));
                if (!(expressions[0] is Application))
                    throw new ParseException(sExpression.ToString(), string.Format("A \"{0}\" expression's first part must be a list", specialForm));
                var tuples = new List<Tuple<Variable, Expression>>();
                foreach (var expression in (expressions[0] as Application).Expressions)
                {
                    if (!(expression is Application && (expression as Application).Expressions.Length == 2))
                        throw new ParseException(sExpression.ToString(), string.Format("A \"{0}\" definition must be a two-element list", specialForm));
                    var applcation = expression as Application;
                    if (!(applcation.Expressions[0] is Variable))
                        throw new ParseException(sExpression.ToString(), string.Format("A \"{0}\" definition must begin with a variable", specialForm));
                    tuples.Add(new Tuple<Variable, Expression>(applcation.Expressions[0] as Variable, applcation.Expressions[1]));
                }
                var body = MakeBeginIfNeeded(expressions.Skip(1).ToList());
                if (specialForm == "let")
                    return new Let(tuples, body);
                else if (specialForm == "let*")
                    return new LetStar(tuples, body);

            }
            if (specialForm == "begin")
            {
                return new Begin(expressions);
            }
            if(specialForm=="cond")
            {
                var last = expressions.Last() as Application;
                Expression condition = last.Expressions[0];
                if (last.Expressions[0] is SpecialToken && (last.Expressions[0] as SpecialToken).Token == "else")
                    condition = new PrimitiveWrapper<bool>() { Value = true };
                var accIf = new If(condition, last.Expressions[1], Void.Instance);
                return expressions.Cast<Application>().Reverse().Skip(1)
                    .Aggregate(accIf,(acc,x)=>new If(x.Expressions[0],x.Expressions[1],acc));

            }
            if(specialForm=="delay")
            {
                var expr = expressions.Single();
                return new Promise(expr);
            }
            throw new InternalException(string.Format("Illegal special form {0}",specialForm));
        }

        private static Expression MakeBeginIfNeeded(List<Expression> expressions)
        {
            var defined = expressions.Count == 1 ? expressions[0] : new Begin(expressions.ToList());
            return defined;
        }

        private static Expression ParseSimpleToken(SimpleToken simpleToken)
        {
            var text = simpleToken.Value;
            Value value;
            if (text == "'()")
                return Null.Instance;
            if (text == ".")
                return new SpecialToken(".");
            if (SpecialForms.Exists(s => s == text))
                return new SpecialFormToken(text);
            foreach (var parser in ValueParsers.Union(ExtendedParsers))
                if (parser.TryParse(text, out value))
                    return value;
            return new Variable(text);
        }

        public static bool IsInputComplete(string input)
        {
            return input.Count(c => c == '(') == input.Count(c => c == ')');
        }

        public static int CalcPadding(string input)
        {
            return 0;
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
        private static string SliceString(string str, int start, int end)
        {
            return str.Substring(start, end - start);
        }
        
        internal static Value ParseValue(string input)
        {
            return ParseValue(Lexer.CaptureToken(input));
        }
        internal static Value ParseValue(CapturedToken token)
        {
            if (token is SimpleToken)
            {
                Value value;
                var text = (token as SimpleToken).Value;
                foreach (var parser in ValueParsers)
                    if (parser.TryParse(text, out value))
                        return value;
                return new Symbol(text);
            }
            var sExpression = token as SExpression;
            var parsed = sExpression.Tokens.Select(t => ParseValue(t));
            return Util.ToSchemeList(parsed);

        }
    }

    
}
