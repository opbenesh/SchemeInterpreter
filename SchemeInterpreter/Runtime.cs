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
                //while (!Parser.IsInputComplete(input))
                //{
                //    var padding = PROMPT.Length;// + Parser.CalcPadding(input);
                //    Console.Write(new string(' ', padding));
                //    input += Console.ReadLine();
                //}
                try
                {
                    var expr = Parser.ParseExpression(input);
                    var res = expr.Eval(_environment);
                    WriteInStyle(res);
                    Console.WriteLine();
                }
                catch(InterpreterException ex)
                {
                    WriteInColor(ex.Message, ConsoleColor.Red);
                    Console.WriteLine();
                }
            }
        }

        private void WriteInStyle(Value res)
        {
            if (res is String)
                WriteInColor(string.Format("\"{0}\"", res), ConsoleColor.Green);
            else
                WriteInColor(res.ToString(),ConsoleColor.Cyan);
        }


        private void WriteInColor(string message, ConsoleColor consoleColor)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.Write(message);
            Console.ForegroundColor = previousColor;
        }

        private void Initialize()
        {
            this._environment = new Environment();
            foreach (var primitiveProcedure in Predefined.PrimitiveProcedures)
                _environment[primitiveProcedure.Name] = new Closure(_environment,primitiveProcedure,primitiveProcedure.Name);
        }
    }
}
