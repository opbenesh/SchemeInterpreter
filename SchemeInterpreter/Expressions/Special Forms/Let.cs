using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    abstract class LetBase : SpecialForm
    {
        public List<Tuple<Variable,Expression>> Variables { get; private set; }
        public Expression Body { get; private set; }

        public LetBase(List<Tuple<Variable, Expression>> variables, Expression body)
        {
            this.Variables = variables;
            this.Body = body;
        }

        public override Value Eval(Environment environment)
        {
            var newEnv = GetNewEnvironment(environment);
            return Body.Eval(newEnv);
        }

        protected abstract Environment GetNewEnvironment(Environment environment);
    }
    class Let : LetBase
    {
        protected override string GetName()
        {
            return "let";
        }

        public Let(List<Tuple<Variable,Expression>> variables, Expression body)
            :base(variables,body)
        {
        }

        protected override Environment GetNewEnvironment(Environment environment)
        {
            var newEnv = new Environment(environment);
            foreach (var tuple in Variables)
                newEnv[tuple.Item1.Name] = tuple.Item2.Eval(environment);
            return newEnv;
        }
    }
    class LetStar:Let
    {
        protected override string GetName()
        {
            return "let*";
        }
        public LetStar(List<Tuple<Variable,Expression>> variables, Expression body)
            :base(variables,body)
        {
        }
        protected override Environment GetNewEnvironment(Environment environment)
        {
            var newEnv = new Environment(environment);
            foreach (var tuple in Variables)
                newEnv[tuple.Item1.Name] = tuple.Item2.Eval(newEnv);
            return newEnv;
        }
    }
}
