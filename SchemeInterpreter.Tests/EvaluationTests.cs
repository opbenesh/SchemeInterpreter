using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter;

namespace SchemeInterpreter.Tests
{
    [TestClass]
    public class EvaluationTests
    {

        [TestMethod]
        public void TestDefineVariable()
        {
            var runtime = new Runtime();
            runtime.Execute("(define a 1)");
            Assert.IsTrue(runtime.Execute("a").Equals(new Integer() { Value = 1 }));
        }
        [TestMethod]
        public void TestDefineProcedure()
        {
            var runtime = new Runtime();
            runtime.Execute("(define (f x) (+ x x))");
            Assert.IsTrue(runtime.Execute("(f 1)").Equals(new Integer() { Value = 2 }));
        }
    }
}
