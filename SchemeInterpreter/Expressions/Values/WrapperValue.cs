using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class PrimitiveWrapper<T>:Value
    {
        public T Value { get; set; }
        public override string ToString()
        {
            return Value.ToString();
        }
        public override bool Equals(object obj)
        {
            return obj is PrimitiveWrapper<T> && this.Value.Equals((obj as PrimitiveWrapper<T>).Value);
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}