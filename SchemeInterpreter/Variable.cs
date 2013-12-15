using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Variable:Expression
    {
        private string _name;
        public Variable(string name)
        {
            _name = name;
        }
        public override Value Eval(Environment environment)
        {
            return environment[_name];
        }
    }
}
