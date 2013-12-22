using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeInterpreter.Tests
{
    public class TestsBase
    {
        public TestsBase()
        {
            Runtime.InitializeTopLevelEnvironment();
        }
    }
}
