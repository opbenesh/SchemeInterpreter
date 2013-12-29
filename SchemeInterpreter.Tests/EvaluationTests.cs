using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemeInterpreter;
using System.IO;

namespace SchemeInterpreter.Tests
{
    [TestClass]
    public class EvaluationTests : TestsBase
    {
        [TestMethod]
        public void TestCons()
        {
            var runtime = new Runtime();
            runtime.Execute("(define cell (cons 1 2))");
            Assert.IsTrue(runtime.Execute("(car cell)").Equals(new PrimitiveWrapper<int>() { Value = 1 }));
            Assert.IsTrue(runtime.Execute("(cdr cell)").Equals(new PrimitiveWrapper<int>() { Value = 2 }));
        }
        [TestMethod]
        public void TestMutation()
        {
            var runtime = new Runtime();
            runtime.Execute("(define cell (cons 1 2))");
            runtime.Execute("(set-car! cell 2)");
            runtime.Execute("(set-cdr! cell 1)");
            Assert.IsTrue(runtime.Execute("(car cell)").Equals(new PrimitiveWrapper<int>() { Value = 2 }));
            Assert.IsTrue(runtime.Execute("(cdr cell)").Equals(new PrimitiveWrapper<int>() { Value = 1 }));
        }
        [TestMethod]
        public void TestRecursion()
        {
            var runtime = new Runtime();
            runtime.Execute("(define (fact x) (if (= x 0) 1 (* x (fact (- x 1)))))");
            Assert.IsTrue(runtime.Execute("(fact 5)").Equals(new PrimitiveWrapper<int>() { Value = 120 }));
        }
        [TestMethod]
        public void TestVariadic()
        {
            var runtime = new Runtime();
            runtime.Execute("(define (first . xs) (car xs))");
            Assert.IsTrue(runtime.Execute("(first 1 2 3)").Equals(new PrimitiveWrapper<int>() { Value = 1 }));
        }
        [TestMethod]
        public void TestApply()
        {
            var runtime = new Runtime();
            Assert.IsTrue(runtime.Execute("(apply + (list 1 2 3))").Equals(new PrimitiveWrapper<int>() { Value = 6 }));
        }
        [TestMethod]
        public void TestTailCallOptimization()
        {
            int benchmark = 10000;
            var runtime = new Runtime();
            runtime.Execute("(define (f n) (if (= n 0) #t (f (- n 1))))");
            runtime.Execute(string.Format("(f {0})",benchmark));
        }
    }
}
