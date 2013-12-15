using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    abstract class WrapperValue<T>:Value
    {
        public T Value { get; set; }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
    class Integer : WrapperValue<int> { }
    class Boolean : WrapperValue<bool> { }
    class String : WrapperValue<string> { }
}
