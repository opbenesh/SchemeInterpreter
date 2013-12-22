using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SchemeInterpreter
{
    class Runtime
    {
        internal static TopLevelEnvironment TopLevelEnvironment;
        public static void InitializeTopLevelEnvironment()
        {
            if (TopLevelEnvironment != null)
                return;
            var runtime = new Runtime();
            foreach (var primitiveValue in Predefined.PrimitiveValues)
                runtime._environment[primitiveValue.Key] = primitiveValue.Value;
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
                    input += System.Environment.NewLine;
                    Console.Write(MORE_PROMPT);
                    var padding = Math.Max(0, (PROMPT.Length - MORE_PROMPT.Length) + Parser.CalcPadding(input));
                    Console.Write(new string(' ', padding));
                    input += Console.ReadLine();
                }
                input=NormalizeInput(input);
                try
                {
                    var res = Execute(input);
                    OutputResult(res);
                }
                catch (InterpreterException ex)
                {
                    WriteInColor(ex.Message, ConsoleColor.Red);
                    Console.WriteLine();
                }
            }
        }

        private void OutputResult(Value res)
        {
            if (res is Void)
                return;
            WriteInStyle(res);
            Console.WriteLine();
        }

        private string NormalizeInput(string input)
        {
            //input = Regex.Replace(input, @";.*?($|\r\n)", "",RegexOptions.Singleline);
            bool isInString = false;
            bool isEscaping = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (isEscaping)
                    isEscaping = false;
                else if (input[i] == '"')
                    isInString = !isInString;
                else if (input[i] == '\\')
                    isEscaping = true;

                if (!isInString && input[i]==';')
                {
                    int nextNewline = input.IndexOf("\r\n", i);
                    if (nextNewline == -1)
                        input = input.Remove(i);
                    else
                        input = input.Remove(i, nextNewline - i);
                }
            }
            return input.Replace("\r\n", " ").Trim();
        }
        public void ExecuteFile(string file)
        {
            var reader = new StreamReader(file);
            int line=0;
            while (!reader.EndOfStream)
            {
                string input;
                for (input = reader.ReadLine(), line++; !reader.EndOfStream && !Parser.IsInputComplete(input); input += "\r\n" + reader.ReadLine(), line++)
                    ;
                input = NormalizeInput(input);
                if (input.Length == 0)
                    continue;
                try
                {
                    Execute(input);
                }
                catch (NativeSchemeException ex)
                {
                    WriteInColor(ex.Message, ConsoleColor.Red);
                    Console.WriteLine();
                }
                catch (InterpreterException ex)
                {
                    throw new InterpreterException(string.Format("Exception in file {0}, line {1}:",file, line),ex);
                }
            }
        }

        private void WriteInStyle(Value res)
        {
            if (res is PrimitiveWrapper<string>)
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
