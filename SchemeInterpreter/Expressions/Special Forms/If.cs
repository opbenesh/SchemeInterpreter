using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class If:SpecialForm
    {
        private Expression _condition;
        private Expression _trueBranch;
        private Expression _falseBranch;

        public If(Expression condition, Expression trueBranch, Expression falseBranch)
        {
            this._condition = condition;
            this._trueBranch = trueBranch;
            this._falseBranch = falseBranch;
        }
        public override Value Eval(Environment environment)
        {
            var evaledCondition = _condition.Eval(environment);
            if (evaledCondition is PrimitiveWrapper<bool> && !(evaledCondition as PrimitiveWrapper<bool>).Value)
                return _falseBranch.Eval(environment);
            return _trueBranch.Eval(environment);
        }

        protected override string GetName()
        {
            return "if";
        }
    }
}
