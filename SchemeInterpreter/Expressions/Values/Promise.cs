using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Promise : Value
    {
        public Expression Expression { get; private set; }
        public bool IsEvaled { get; private set; }
        public Promise(Expression expression)
        {
            this.Expression = expression;
            this.IsEvaled = false;
        }

        public Value Force(Environment env)
        {
            if(!IsEvaled)
            {
                Expression = Expression.Eval(env);
                IsEvaled = true;
            }
            return Expression as Value;
        }
        public override string ToString()
        {
            return "#<promise>";
        }
    }
}
