using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class SpecialFormToken:Expression
    {
        public string Token { get; private set; }
        public SpecialFormToken(string token)
        {
            this.Token = token;
        }

        public override Value Eval(Environment environment)
        {
            throw new NotSupportedException("SpecialFormToken is a parsing temporary value and should not be evaluated");
        }
    }
}
