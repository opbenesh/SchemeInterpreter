using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Pair:Value
    {
        public Expression Car { get; set; }
        public Expression Cdr { get; set; }

        public Pair(Expression car, Expression cdr)
        {
            this.Car = car;
            this.Cdr = cdr;
        }

        public override Value Eval(Environment environment)
        {
            Car = Car.Eval(environment);
            Cdr = Cdr.Eval(environment);
            return this;
        }
        public override string ToString()
        {
            if (IsProperList())
                return string.Format("({0})", string.Join<Value>(" ", Util.ToList<Value>(this)));
            return string.Format("({0} . {1}", this.Car, this.Cdr);
        }

        private bool IsProperList()
        {
            return Cdr is Null || (Cdr is Pair && (Cdr as Pair).IsProperList());
        }
    }
}
