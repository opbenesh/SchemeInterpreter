using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Symbol : Value
    {
        public string Name { get; private set; }

        public Symbol(string name)
        {
            this.Name = name;
        }
        public override string ToString()
        {
            return string.Format("'{0}", Name);
        }
        public override bool Equals(object obj)
        {
            return obj is Symbol && this.Name == (obj as Symbol).Name;
        }
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
