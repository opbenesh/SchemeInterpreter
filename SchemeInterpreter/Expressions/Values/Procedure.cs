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
        public UserDefinedProcedure(Expression body, bool isLastArgList,List<string> formalArgs)
            :base(isLastArgList,formalArgs.Count)
        {
            this.Body = body;
            this.FormalArgs = formalArgs.ToList();
        }

        public override Value Apply(List<Value> args, Environment environment)
        {
            return Body.Eval(environment);
        }
    }
    class PrimitiveProcedure : Procedure
    {
        public string Name { get; private set; }
        private Func<List<Value>, Environment, Value> _inner;
        public PrimitiveProcedure(string name, Func<List<Value>, Value> function, int argCount, bool isLastArgList = false)
            : base(isLastArgList, argCount)
        {
            this.Name = name;
            this._inner = (vl,env)=>function(vl);
        }
        public PrimitiveProcedure(string name, Func<List<Value>, Environment, Value> function, int argCount, bool isLastArgList = false)
            : base(isLastArgList, argCount)
        {
            this.Name = name;
            this._inner = function;
        }
        public override Value Apply(List<Value> args, Environment environment)
        {
            return _inner(args,environment);
        }
    }
}
