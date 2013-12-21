using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Define : SpecialForm
    {
        public Expression DefinedName { get; private set; }
        public Expression DefinedValue { get; private set; }

        protected override string GetName()
        {
            return "define";
        }

        public Define(Expression name, Expression value)
        {
            this.DefinedName = name;
            this.DefinedValue = value;
        }

        public override Value Eval(Environment environment)
        {
            if (DefinedName is Variable)
                DefineVariable(environment);
            else if (DefinedName is Application)
                DefineProcedure(environment);
            return new PrimitiveWrapper<bool>() { Value = false };
        }

        private void DefineProcedure(Environment environment)
        {
            var expressions = (DefinedName as Application).Expressions;
            var procName = (expressions[0] as Variable).Name;
            var proc = new UserDefinedProcedure(DefinedValue, false, expressions.Skip(1).Select(v => (v as Variable).Name).ToList());
            var closure = new Closure(environment, proc);
            environment[procName] = closure;
        }

        private void DefineVariable(Environment environment)
        {
            var value = DefinedValue.Eval(environment);
            environment[(DefinedName as Variable).Name] = value;
        }

        
    }
}
