using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter;

namespace SchemeInterpreter.Tests
{
    [TestClass]
    public class EvaluationTests
    {
        public EvaluationTests()
        {
            Runtime.InitializeTopLevelEnvironment();
        }
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
        public void TestCons()
        {
            var runtime = new Runtime();
            runtime.Execute("(define cell (cons 1 2))");
            Assert.IsTrue(runtime.Execute("(car cell)").Equals(new PrimitiveWrapper<int>() { Value = 1 }));
            Assert.IsTrue(runtime.Execute("(cdr cell)").Equals(new PrimitiveWrapper<int>() { Value = 2 }));
        }
        [TestMethod]
        public void TestIf()
        {
            var runtime = new Runtime();
            Assert.IsTrue(runtime.Execute("(if #t 1 2)").Equals(new PrimitiveWrapper<int>() { Value = 1 }));
            Assert.IsTrue(runtime.Execute("(if #f 1 2)").Equals(new PrimitiveWrapper<int>() { Value = 2 }));
        }
        [TestMethod]
        public void TestRecursion()
        {
            var runtime = new Runtime();
            runtime.Execute("(define (fact x) (if (= x 0) 1 (* x (fact (- x 1)))))");
            Assert.IsTrue(runtime.Execute("(fact 5)").Equals(new PrimitiveWrapper<int>() { Value = 120 }));
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
    }
}
