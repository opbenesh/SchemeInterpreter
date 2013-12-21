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
            runtime.Repl();
        }
    }
}
