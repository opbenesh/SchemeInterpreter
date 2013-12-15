using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    abstract class Procedure : Value
    {
        public List<string> FormalArgs { get; private set; }
        public bool IsLastArgList = false;
        public abstract Value Apply(List<Value> args, Environment environment);
        public Procedure(params string[] formalArgs)
        {
            this.FormalArgs = formalArgs.ToList();
        }
        public Procedure(bool isLastArgList, params string[] formalArgs)
            :this(formalArgs)
        {
            this.IsLastArgList = isLastArgList;
        }
    }
}
