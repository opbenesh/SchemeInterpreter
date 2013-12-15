using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class InvalidArgumentCountException:Exception
    {
        public InvalidArgumentCountException(int expected, int found, bool lastArgList = false)
            : base(string.Format("Illegal procedure call: Expected {0} arguments, found {1}", (lastArgList ? "at least " : "") + expected, found))
        { }
    }
    class UndefinedVariableException : Exception
    {
        public UndefinedVariableException(string name)
            : base(string.Format("Undeclared variable '{0}'", name))
        { }
    }
    class ParseException : Exception
    {
        public ParseException(string str, string error)
            : base(string.Format("Could not parse string \"{0}\": {1}", str, error))
        { }
    }
    class EvaluationException : Exception
    {
        public EvaluationException(Expression expr, string error)
            : base(string.Format("Could not evaluate expression \"{0}\": {1}", expr, error))
        { }
    }
}
