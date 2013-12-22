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
        public Closure(Environment environment, Procedure procedure)
        {
            this._environment = environment;
            this._procedure = procedure;
        }
        public Value Apply(Environment environment, List<Expression> args)
        {
            AssertProperArgsCount(args.Count);
            var evaled = args.Select(e => e.Eval(environment)).ToList();
            var newEnv = new Environment(_environment);
            if (_procedure is UserDefinedProcedure)
                AddArgsToEnvironment((_procedure as UserDefinedProcedure).FormalArgs, evaled, newEnv);
            return _procedure.Apply(evaled, newEnv);
        }
        public Value Apply(List<Value> args)
        {
            AssertProperArgsCount(args.Count);
            var newEnv = new Environment(_environment);
            if (_procedure is UserDefinedProcedure)
                AddArgsToEnvironment((_procedure as UserDefinedProcedure).FormalArgs, args, newEnv);
            return _procedure.Apply(args, newEnv);
        }

        private void AddArgsToEnvironment(List<string> formal, List<Value> evaled, Environment environment)
        {
            if (formal.Count == 0)
                return;
            for (int i = 0; i < formal.Count - 1; i++)
            {
                environment[formal[i]] = evaled[i];
            }
            if (_procedure.IsLastArgList)
                environment[formal.Last()] = Util.ToSchemeList(evaled.Skip(formal.Count - 1));
            else
                environment[formal.Last()] = evaled.Last();
        }

        private void AssertProperArgsCount(int argCount)
        {
            if (!_procedure.IsLastArgList)
            {
                if (_procedure.ArgumentCount != argCount)
                    throw new InvalidArgumentCountException(_procedure.ArgumentCount, argCount);
            }
            else
            {
                if (argCount < _procedure.ArgumentCount - 1)
                    throw new InvalidArgumentCountException(_procedure.ArgumentCount, argCount);
            }
        }
        public override string ToString()
        {
            return "#<procedure>";
        }
    }
}
