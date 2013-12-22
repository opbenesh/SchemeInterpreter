using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Begin:SpecialForm
    {
        private List<Expression> _expressions;

        public Begin(List<Expression> expressions)
        {
            this._expressions = expressions;
        }
        public override Value Eval(Environment environment)
        {
            if (_expressions.Count == 0)
                return Void.Instance;
            for (int i = 0; i < _expressions.Count - 1; i++)
                _expressions[i].Eval(environment);
            return _expressions.Last().Eval(environment);
        }

        protected override string GetName()
        {
            return "if";
        }
    }
}
