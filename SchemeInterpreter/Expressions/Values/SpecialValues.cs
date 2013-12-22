using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemeInterpreter
{
    class Null : Value
    {
        private Null() { }
        private static Null _instance = new Null();
        public static Null Instance { get { return _instance; } }
        public override string ToString()
        {
            return "()";
        }
    }
    class Void : Value
    {
        private Void() { }
        private static Void _instance = new Void();
        public static Void Instance { get { return _instance; } }
    }
    class EOF : Value
    {
        private EOF() { }
        private static EOF _instance = new EOF();
        public static EOF Instance { get { return _instance; } }
    }
}
