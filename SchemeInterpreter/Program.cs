using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Runtime.InitializeTopLevelEnvironment();
            }
            catch(InterpreterException ex)
            {
                Console.WriteLine(ex);
                return;
            }
            var runtime = new Runtime();
            if (args.Length == 1)
                runtime.ExecuteFile(args[0]);
            else
                runtime.Repl();
        }
    }
}
