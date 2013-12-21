using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    public class InternalException : Exception
    {
        public InternalException() { }
        public InternalException(string message) : base(message) { }
        public InternalException(string message, Exception inner) : base(message, inner) { }
    }
    class InterpreterException : ApplicationException
    {
        public InterpreterException() : base() { }
        public InterpreterException(string message) : base(message) { }
        public InterpreterException(string message,Exception ex) : base(message,ex) { } 
    }
    class InvalidArgumentCountException:Exception
    {
        public InvalidArgumentCountException(int expected, int found, bool lastArgList = false)
            : base(string.Format("Illegal procedure call: Expected {0} arguments, found {1}", (lastArgList ? "at least " : "") + expected, found))
        { }
    }
    class UndefinedVariableException : EvaluationException
    {
        public UndefinedVariableException(string name)
            : base(string.Format("Undeclared variable '{0}'", name))
        { }
    }
    class UnevaluatedExpressionException : EvaluationException
    {
        public UnevaluatedExpressionException(Expression expr)
            : base(string.Format("Cannot reference unevaluated expression '{0}'", expr))
        { }
    }
    class ParseException : InterpreterException
    {
        public ParseException(string str, string error)
            : base(string.Format("Could not parse string \"{0}\": {1}", str, error))
        { }
    }
    class EvaluationException : InterpreterException
    {
        protected EvaluationException(string message) : base(message) { }
        public EvaluationException(Expression expr, string error)
            : base(string.Format("Could not evaluate expression \"{0}\": {1}", expr, error))
        { }
    }
    class SpecialFormException : InterpreterException
    {
        protected SpecialFormException(string message) : base(message) { }
        public SpecialFormException(string specialForm, string error)
            : base(string.Format("Could not evaluate special form \"{0}\": {1}", specialForm, error))
        { }
    }
    class TypeMismatchException : EvaluationException
    {
        public TypeMismatchException(Expression expr, Type expectedType)
            : base(string.Format("Type mismatch: expected: {0}\n found: {1}\n in expression {2}", expr.GetType(), expectedType, expr))
        { }
    }
}
