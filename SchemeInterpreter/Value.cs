using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    abstract class Value:Expression
    {
        public override Value Eval(Environment environment)
        {
            return this;
        }
    }
}
