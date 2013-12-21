using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Runtime
    {
        internal static TopLevelEnvironment TopLevelEnvironment;
        public static void InitializeTopLevelEnvironment()
        {
            var runtime = new Runtime();
            foreach (var primitiveProcedure in Predefined.PrimitiveProcedures)
                runtime._environment[primitiveProcedure.Name] = new Closure(runtime._environment, primitiveProcedure);
            runtime.ExecuteFile(@"Scheme\init.scm");
            TopLevelEnvironment = new TopLevelEnvironment(runtime._environment);
        }
        internal Environment _environment;
        private const string PROMPT = ">>> ";
        private const string MORE_PROMPT = "... ";
        public Runtime()
        {
            Initialize();
        }
        public Value Execute(string input)
        {
            var expr = Parser.ParseExpression(input);
            var res = expr.Eval(_environment);
            return res;
        }
        public void Repl()
        {
            while (true)
            {
                Console.Write(PROMPT);
                string input = Console.ReadLine();
                while (!Parser.IsInputComplete(input))
                {
                    Console.Write(MORE_PROMPT);
                    var padding = Math.Max(0, (PROMPT.Length - MORE_PROMPT.Length) + Parser.CalcPadding(input));
                    Console.Write(new string(' ', padding));
                    input += Console.ReadLine();
                }
                try
                {
                    var res = Execute(input);
                    WriteInStyle(res);
                    Console.WriteLine();
                }
                catch (InterpreterException ex)
                {
                    WriteInColor(ex.Message, ConsoleColor.Red);
                    Console.WriteLine();
                }
            }
        }
        public void ExecuteFile(string file)
        {
            var sb = new StreamReader(file);
            int line=0;
            while (!sb.EndOfStream)
            {
                string input;
                for (input = sb.ReadLine(), line++; !sb.EndOfStream && !Parser.IsInputComplete(input); input += sb.ReadLine(), line++)
                    ;
                try
                {
                    Execute(input);
                }
                catch (InterpreterException ex)
                {
                    throw new InterpreterException(string.Format("Exception in file {0}, line {1}:",file, line),ex);
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
            this._environment = new Environment(TopLevelEnvironment);
        }
    }
}
