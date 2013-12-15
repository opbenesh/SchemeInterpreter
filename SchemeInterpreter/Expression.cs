using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    abstract class Expression
    {
        public abstract Value Eval(Environment environment);
    }
}
