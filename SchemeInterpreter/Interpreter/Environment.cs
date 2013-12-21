using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Environment
    {
        private Environment _base;
        private Dictionary<string, Value> _inner;
        public Environment(Environment baseEnv)
        {
            _base = baseEnv;
            _inner = new Dictionary<string, Value>();
        }
        public Environment():this(null)
        {
        }
        public Value this[string name]
        {
            get
            {
                if (_inner.ContainsKey(name))
                    return _inner[name];
                if (_base != null)
                    return _base[name];
                throw new UndefinedVariableException(name);
            }
            set { _inner[name] = value; }
        }
    }
}
