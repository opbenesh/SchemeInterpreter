using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Runtime
    {
        private Environment _environment;
        private const string PROMPT = "> ";
        public void Repl()
        {
            Initialize();
            while (true)
            {
                Console.Write(PROMPT);
                string input = Console.ReadLine();
                while (!Parser.IsInputComplete(input))
                {
                    var padding = PROMPT.Length + Parser.CalcPadding(input);
                    Console.Write(new string(' ', padding));
                    input += Console.ReadLine();
                }
                try
                {
                    var expr = Parser.ParseExpression(input);
                    expr.Eval(_environment);
                }
                catch
                { }
            }
        }

        private void Initialize()
        {
            this._environment = new Environment();
        }
    }
}
