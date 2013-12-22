using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class SpecialToken:Value
    {
        public string Token { get; private set; }
        public SpecialToken(string token)
        {
            this.Token = token;
        }
        public override Value Eval(Environment environment)
        {
            throw new SpecialTokenEvaluationException(this);
        }
        public override string ToString()
        {
            return string.Format("#<{0}>",Token);
        }
    }
    class SpecialFormToken : SpecialToken
    {
        public SpecialFormToken(string token):base(token)
        {
        }
    }

}
