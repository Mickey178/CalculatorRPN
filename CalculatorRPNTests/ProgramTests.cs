using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CalculatorRPN.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [DataRow("1+1","2")]
        [DataRow("5*(1+3)", "20")]
        [DataRow("5*(5+(1+3)/2)", "35")]
        [DataRow("27-(5+(12/3)*3)", "10")]
        [DataTestMethod]
        public void CalculateTests(string input,string assertResult)
        {
            var actualResult = Program.Calculate(input);
            Assert.AreEqual(assertResult,actualResult);
        }
        
        [DataRow("1","1","+", "2")]
        [DataRow("15","3","-", "12")]
        [DataRow("7","0,5","*", "3,5")]
        [DataRow("12","0,5","/", "24")]
        [DataTestMethod]
        public void MathematicActionTests(string inputFirst, string inputSecond,string inputChar, string assertResult)
        {
            var actualResult = Program.MathematicAction(inputFirst,inputSecond,inputChar);
            Assert.AreEqual(assertResult, actualResult);
        }

        [DataRow("1+1", "2")]
        [DataRow("5*(1+3)", "20")]
        [DataRow("5*(5+(1+3)/2)", "35")]
        [DataRow("27-(5+(12/3)*3)", "10")]
        [DataTestMethod]
        public void MathematicOperationTests(string input, string assertResult)
        {
            var actualResult = Program.MathematicalOperation(Program.Converting(input));
            Assert.AreEqual(assertResult, actualResult);
        }

        [DataRow("27-(5+(12/3)*3)")]
        [TestMethod]
        public void ConvertingTests(string input)
        {
            List<string> assertResult = new List<string>
            {
                "27",
                "5",
                "12",
                "3",
                "/",
                "3",
                "*",
                "+",
                "-"
            };
            var actualResult = Program.Converting(input);
            CollectionAssert.AreEqual(assertResult, actualResult);
        }

        [ExpectedException(typeof(ArgumentNullException), "exception was not throw")]
        [TestMethod]
        public void DivideByZeroExceptionReturned()
        {
            double x = 10;
            double y = 0;
            Program.Divide(x, y);
        }
    }
}