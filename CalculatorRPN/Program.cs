using NLog;
using System;
using System.Collections.Generic;

namespace CalculatorRPN
{
    public class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            string presentationString = Console.ReadLine();

            Console.WriteLine("\nОтвет: " + Calculate(presentationString));
            Console.ReadLine();
        }

        public static string Calculate(string input)
        {
            List<string> representationRPN = new List<string>(Converting(input));

            foreach (var item in representationRPN)
            {
                Console.Write(item);
            }

            return MathematicalOperation(representationRPN);
        }

        public static string MathematicalOperation(List<string> representationRPN)
        {
            for (int i = 0; i < representationRPN.Count; i++)
            {
                if (IsNumeric(representationRPN[i]) == false)
                {
                    switch (representationRPN[i])
                    {
                        case "+":
                            representationRPN[i] = MathematicAction(representationRPN[i - 2], representationRPN[i - 1], representationRPN[i]);
                            representationRPN.RemoveAt(i - 2);
                            representationRPN.RemoveAt(i - 2);
                            break;
                        case "-":
                            representationRPN[i] = MathematicAction(representationRPN[i - 2], representationRPN[i - 1], representationRPN[i]);
                            representationRPN.RemoveAt(i - 2);
                            representationRPN.RemoveAt(i - 2);
                            break;
                        case "*":
                            representationRPN[i] = MathematicAction(representationRPN[i - 2], representationRPN[i - 1], representationRPN[i]);
                            representationRPN.RemoveAt(i - 2);
                            representationRPN.RemoveAt(i - 2);
                            break;
                        case "/":
                            representationRPN[i] = MathematicAction(representationRPN[i - 2], representationRPN[i - 1], representationRPN[i]);
                            representationRPN.RemoveAt(i - 2);
                            representationRPN.RemoveAt(i - 2);
                            break;
                        default:
                            Console.WriteLine("Что-то пошло не так");
                            logger.Error("Введен некорректный символ математической операции");
                            continue;
                    }
                    i -= 2;
                }

            }
            return representationRPN[0];
        }

        public static List<string> Converting(string mathematicalOrdinaryRepresentation)
        {
            List<string> calculateViewList = new List<string>();

            for (int i = 0; i < mathematicalOrdinaryRepresentation.Length; i++)
            {
                calculateViewList.Add(mathematicalOrdinaryRepresentation.Substring(i, 1));
            }

            for (int i = 0; i < calculateViewList.Count - 1;)
            {
                if (IsNumeric(calculateViewList[i]) && IsNumeric(calculateViewList[i + 1]))
                {
                    calculateViewList[i + 1] = calculateViewList[i] + calculateViewList[i + 1];
                    calculateViewList.RemoveAt(i);
                }
                i++;
            }

            List<string> mathematicalCharListOnStack = new List<string>();
            List<string> representationRPN = new List<string>();

            for (int i = 0; i < calculateViewList.Count; i++)
            {
                if (IsNumeric(calculateViewList[i]))
                {
                    representationRPN.Add(calculateViewList[i]);
                }
                else
                {
                    if (mathematicalCharListOnStack.Count == 0)
                    {
                        mathematicalCharListOnStack.Add(calculateViewList[i]);
                    }
                    else
                    {
                        if (Priority(calculateViewList[i]) == 2)
                        {
                            if (Priority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 2)
                            {
                                representationRPN.Add(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]);
                                mathematicalCharListOnStack.RemoveAt(mathematicalCharListOnStack.Count - 1);
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }

                            if (Priority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 1)
                            {
                                int parentheses = mathematicalCharListOnStack.LastIndexOf("(");
                                for (int k = 0; k < mathematicalCharListOnStack.Count - 1 - parentheses;)
                                {
                                    representationRPN.Add(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]);
                                    mathematicalCharListOnStack.RemoveAt(mathematicalCharListOnStack.Count - 1);
                                }
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }

                            if (Priority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 3)
                            {
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }
                        }

                        if (Priority(calculateViewList[i]) == 1)
                        {
                            if (Priority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 1)
                            {
                                representationRPN.Add(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]);
                                mathematicalCharListOnStack.RemoveAt(mathematicalCharListOnStack.Count - 1);
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }
                            if (Priority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 2 || Priority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 3)
                            {
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }
                        }

                    }

                    if (Priority(calculateViewList[i]) == 3)
                    {
                        if (Priority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) != 3)
                        {
                            mathematicalCharListOnStack.Add(calculateViewList[i]);
                        }
                    }

                    if (Priority(calculateViewList[i]) == 4)
                    {
                        int parentheses = mathematicalCharListOnStack.LastIndexOf("(");
                        for (int e = 0; e < mathematicalCharListOnStack.Count - 1 - parentheses;)
                        {
                            representationRPN.Add(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]);
                            mathematicalCharListOnStack.RemoveAt(mathematicalCharListOnStack.Count - 1);
                        }
                        mathematicalCharListOnStack.RemoveAt(mathematicalCharListOnStack.Count - 1);
                    }

                }

                if (i == calculateViewList.Count - 1)
                {
                    for (int j = 0; j < mathematicalCharListOnStack.Count;)
                    {
                        representationRPN.Add(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]);
                        mathematicalCharListOnStack.RemoveAt(mathematicalCharListOnStack.Count - 1);
                    }
                }
            }
            return representationRPN;
        }

        public static string MathematicAction(string firstInput,string secondInput,string mathematicChar)
        {
            if (mathematicChar == "+")
            {
                return Sum(double.Parse(firstInput), double.Parse(secondInput)).ToString();
            }
            else if(mathematicChar == "-")
            {
                return Subtract(double.Parse(firstInput), double.Parse(secondInput)).ToString();
            }
            else if(mathematicChar == "*")
            {
                return Multiply(double.Parse(firstInput), double.Parse(secondInput)).ToString();
            }
            else 
            {
                return Divide(double.Parse(firstInput), double.Parse(secondInput)).ToString();
            }
        }

        public static int Priority(string value)
        {
            if (value == "*" || value == "/")
                return 1;
            else if (value == "+" || value == "-")
                return 2;
            else if (value == "(")
                return 3;
            else if (value == ")")
                return 4;
            else
                return 1;
        }

        public static bool IsNumeric(string value)
        {
            try
            {
                if (value == ",")
                    return true;

                var numericValue = double.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static double Sum(double a, double b)
        {
            return a + b;
        }

        public static double Subtract(double a, double b)
        {
            return a - b;
        }

        public static double Multiply(double a, double b)
        {
            return a * b;
        }

        public static double Divide(double a, double b)
        {
            if (b != 0)
            {
                return a / b;
            }
            else
            {
                logger.Fatal("деление на 0");
                throw new ArgumentNullException("cannot be divided by zero");
            }
        }
    }
}
