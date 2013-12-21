using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    abstract class SpecialForm:Expression
    {
        protected abstract string GetName();
        public string Name
        {
            get { return GetName(); } 
        }
    }
}
