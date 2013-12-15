using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Application : Expression
    {
        public Expression[] Expressions { get; private set; }
        public Application(Expression[] expressions)
        {
            this.Expressions = expressions;
        }
        public override Value Eval(Environment environment)
        {
            var closure = Expressions[0].Eval(environment);
            if (!(closure is Closure))
                throw new EvaluationException(closure,"Cannot apply to a non-closure");
            return (closure as Closure).Apply(Expressions.Skip(1).ToList());
        }
    }
}
