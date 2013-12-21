using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    abstract class Expression
    {
        public abstract Value Eval(Environment environment);

        internal Value AsValue()
        {
            if (this is Value)
                return this as Value;
            throw new UnevaluatedExpressionException(this);
        }
    }
}
