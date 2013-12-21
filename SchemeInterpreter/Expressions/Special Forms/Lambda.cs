using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Lambda : SpecialForm
    {
        public Variable[] FormalArguments { get; private set; }
        public Expression DefinedValue { get; private set; }

        protected override string GetName()
        {
            return "lambda";
        }

        public Lambda(Variable[] formalArgs, Expression value)
        {
            this.FormalArguments = formalArgs;
            this.DefinedValue = value;
        }

        public override Value Eval(Environment environment)
        {
            var proc = new UserDefinedProcedure(DefinedValue, false, FormalArguments.Select(v => v.Name).ToList());
            return new Closure(environment, proc);
        }
    }
}
