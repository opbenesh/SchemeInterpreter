using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Closure:Value
    {
        private Environment _environment;
        private Procedure _procedure;
        private string _procedureName;
        public Closure(Environment environment, Procedure procedure, string procedureName)
        {
            this._environment = environment;
            this._procedure = procedure;
            this._procedureName = procedureName;
        }
        public Value Apply(List<Expression> args)
        {
            AssertProperArgsList(args);
            var formal = _procedure.FormalArgs;
            var evaled = args.Select(e => e.Eval(_environment)).ToList();
            var newEnv = new Environment(_environment);
            newEnv[_procedureName] = _procedure;
            AddArgsToEnvironment(formal, evaled, newEnv);
            return _procedure.Apply(evaled, newEnv);
        }

        private void AddArgsToEnvironment(List<string> formal, List<Value> evaled, Environment newEnv)
        {
            if (formal.Count == 0)
                return;
            for (int i = 0; i < formal.Count - 1; i++)
            {
                newEnv[formal[i]] = evaled[i];
            }
            if (_procedure.IsLastArgList)
                newEnv[formal.Last()] = Util.ToSchemeList(evaled.Skip(formal.Count - 1));
            else
                newEnv[formal.Last()] = evaled.Last();
        }

        private void AssertProperArgsList(List<Expression> actual)
        {
            var formal = _procedure.FormalArgs;
            if (!_procedure.IsLastArgList)
            {
                if (formal.Count != actual.Count)
                    throw new InvalidArgumentCountException(formal.Count, actual.Count);
            }
            else
            {
                if (actual.Count < formal.Count - 1)
                    throw new InvalidArgumentCountException(formal.Count, actual.Count);
            }
        }
    }
}
