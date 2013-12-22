using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Environment
    {
        public bool IsTopLevel { get; set; }
        private Environment _base;
        private Dictionary<string, Value> _inner;
        public Environment(Environment baseEnv)
        {
            _base = baseEnv;
            _inner = new Dictionary<string, Value>();
            this.IsTopLevel = false;
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
            set
            {
                MutateEnvironment(name, value);
            }
        }

        protected virtual void MutateEnvironment(string name, Value value)
        {
            _inner[name] = value; 
        }
    }

    class TopLevelEnvironment:Environment
    {
        public TopLevelEnvironment(Environment environment)
            :base(environment)
        {
        }
        protected override void MutateEnvironment(string name, Value value)
        {
            throw new InterpreterException(string.Format("Top-level entry {0} is immutable", name));
        }
    }
}
