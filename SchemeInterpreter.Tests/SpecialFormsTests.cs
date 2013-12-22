using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter;

namespace SchemeInterpreter.Tests
{
    [TestClass]
    public class SpecialFormsTest:TestsBase
    {
        [TestMethod]
        public void TestDefineVariable()
        {
            var runtime = new Runtime();
            runtime.Execute("(define a 1)");
            Assert.IsTrue(runtime.Execute("a").Equals(new PrimitiveWrapper<int>() { Value = 1 }));
        }
        [TestMethod]
        public void TestDefineProcedure()
        {
            var runtime = new Runtime();
            runtime.Execute("(define (f x) (+ x x))");
            Assert.IsTrue(runtime.Execute("(f 1)").Equals(new PrimitiveWrapper<int>() { Value = 2 }));
        }
        [TestMethod]
        public void TestIf()
        {
            var runtime = new Runtime();
            Assert.IsTrue(runtime.Execute("(if #t 1 2)").Equals(new PrimitiveWrapper<int>() { Value = 1 }));
            Assert.IsTrue(runtime.Execute("(if #f 1 2)").Equals(new PrimitiveWrapper<int>() { Value = 2 }));
        }
        [TestMethod]
        public void TestLet()
        {
            var runtime = new Runtime();
            Assert.IsTrue(runtime.Execute("(let ((x 1)) x)").Equals(new PrimitiveWrapper<int>() { Value = 1 }));
            Assert.IsTrue(runtime.Execute("(let ((x 1)) (let ((x 2) (y x)) y))").Equals(new PrimitiveWrapper<int>() { Value =1 }));
        }
        [TestMethod]
        public void TestLetStar()
        {
            var runtime = new Runtime();
            Assert.IsTrue(runtime.Execute("(let ((x 1)) (let* ((x 2) (y x)) y))").Equals(new PrimitiveWrapper<int>() { Value = 2 }));
        }
        [TestMethod]
        public void TestLambda()
        {
            var runtime = new Runtime();
            Assert.IsTrue(runtime.Execute("((lambda (x) (+ x x)) 2)").Equals(new PrimitiveWrapper<int>() { Value = 4 }));
        }
        [TestMethod]
        public void TestApply()
        {
            var runtime = new Runtime();
            Assert.IsTrue(runtime.Execute("(apply + (list 1 2 3))").Equals(new PrimitiveWrapper<int>() { Value = 6 }));
        }
        [TestMethod]
        public void TestBegin()
        {
            var runtime = new Runtime();
            Assert.IsTrue(runtime.Execute("(begin (define x 1) x)").Equals(new PrimitiveWrapper<int>() { Value = 1 }));
        }
    }
}
