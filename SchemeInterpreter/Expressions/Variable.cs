using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Variable:Expression
    {
        public string Name { get; private set; }
        public Variable(string name)
        {
            Name = name;
        }
        public override Value Eval(Environment environment)
        {
            return environment[Name];
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
