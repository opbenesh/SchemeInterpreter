using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    abstract class Procedure : Value
    {
        public bool IsLastArgList {get; private set;}
        public int ArgumentCount { get; private set; }
        public Procedure(bool isLastArgList, int argCount)
        {
            this.IsLastArgList = isLastArgList;
            this.ArgumentCount = argCount;
        }
        public abstract Value Apply(List<Value> args, Environment environment);
    }
    class UserDefinedProcedure : Procedure
    {
        public List<string> FormalArgs { get; private set; }
        public Expression Body { get; private set; }
        public UserDefinedProcedure(Expression body, bool isLastArgList, params string[] formalArgs)
            :base(isLastArgList,formalArgs.Length)
        {
            this.Body = body;
        }

        public override Value Apply(List<Value> args, Environment environment)
        {
            return Body.Eval(environment);
        }
    }
    class PrimitiveProcedure : Procedure
    {
        public string Name { get; private set; }
        private Func<List<Value>, Value> _inner;
        public PrimitiveProcedure(string name, Func<List<Value>, Value> function, int argCount, bool isLastArgList=false)
            : base(isLastArgList, argCount)
        {
            this.Name = name;
            this._inner = function;
        }
        public override Value Apply(List<Value> args, Environment environment)
        {
            return _inner(args);
        }
    }
}
