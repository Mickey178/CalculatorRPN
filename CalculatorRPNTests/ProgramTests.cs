using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorRPN.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void PriorityTest_signMultiplyOrDivide_1returned()
        {
            string value1 = "*";
            string value2 = "/";
            int excepted = 1;
            int actual = Program.Priority(value1);
            Assert.AreEqual(excepted, actual);
            int excepeted2 = 1;
            int actual2 = Program.Priority(value2);
            Assert.AreEqual(excepeted2, actual2);
        }

        public void PriorityTest_signSumOrSubtract_2returned()
        {
            string value1 = "+";
            string value2 = "-";
            int excepted = 2;
            int actual = Program.Priority(value1);
            Assert.AreEqual(excepted, actual);
            int excepeted2 = 2;
            int actual2 = Program.Priority(value2);
            Assert.AreEqual(excepeted2, actual2);
        }

        public void PriorityTest_openParenthesis_3returned()
        {
            string value = "(";
            int excepted = 3;
            int actual = Program.Priority(value);
            Assert.AreEqual(excepted, actual);
        }

        public void PriorityTest_closeParenthesis_4returned()
        {
            string value = ")";
            int excepted = 4;
            int actual = Program.Priority(value);
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod()]
        public void IsNumericTest_comma_trueReturned()
        {
            string value = ",";
            bool excepted = true;
            bool actual = Program.IsNumeric(value);
            Assert.AreEqual(excepted,actual);
        }

        [TestMethod()]
        public void IsNumericTest_5_trueReturned()
        {
            string value = "5";
            bool excepted = true;
            bool actual = Program.IsNumeric(value);
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod()]
        public void IsNumericTest_mathematicalSign_falseReturned()
        {
            string value = "*";
            bool excepted = false;
            bool actual = Program.IsNumeric(value);
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod()]
        public void SumTest_05and10_105returned()
        {
            double x = 0.5;
            double y = 10;
            double excepted = 10.5;
            double actual = Program.Sum(x,y);
            Assert.AreEqual(excepted,actual);
        }

        [TestMethod()]
        public void SubtractTest_20and10_10returned()
        {
            double x = 20;
            double y = 10;
            double excepted = 10;
            double actual = Program.Subtract(x, y);
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod()]
        public void MultiplyTest_5and5_25returned()
        {
            double x = 5;
            double y = 5;
            double excepted = 25;
            double actual = Program.Multiply(x, y);
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod()]
        public void DivideTest_10and5_2returned()
        {
            double x = 10;
            double y = 5;
            double excepted = 2;
            double actual = Program.Divide(x, y);
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod()]
        public void DivideTest_10and0_10returned()
        {
            double x = 10;
            double y = 0;
            double excepted = 10;
            double actual = Program.Divide(x, y);
            Assert.AreEqual(excepted, actual);
        }
    }
}