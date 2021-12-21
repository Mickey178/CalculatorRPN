using System;
using System.Collections.Generic;

namespace CalculatorRPN
{
    public class Program
    {
        static void Main(string[] args)
        {
            string presentationString = Console.ReadLine();
            List<string> regularViewList = new List<string>();

            for (int i = 0; i < presentationString.Length; i++)
            {
                regularViewList.Add(presentationString.Substring(i, 1));
            }

            for (int i = 0; i < regularViewList.Count - 1; i++)
            {
                if (IsNumeric(regularViewList[i]) && IsNumeric(regularViewList[i + 1]))
                {
                    regularViewList[i + 1] = regularViewList[i] + regularViewList[i + 1];
                    regularViewList.RemoveAt(i);
                    i--;
                }
            }

            List<string> charListOnStack = new List<string>();
            List<string> presentationStringRPN = new List<string>();

            for (int i = 0; i < regularViewList.Count; i++)
            {
                if (IsNumeric(regularViewList[i]))
                {
                    presentationStringRPN.Add(regularViewList[i]);
                }
                else
                {
                    if (charListOnStack.Count == 0)
                    {
                        charListOnStack.Add(regularViewList[i]);
                    }
                    else
                    {
                        if (Priority(regularViewList[i]) == 2)
                        {
                            if (Priority(charListOnStack[charListOnStack.Count - 1]) == 2)
                            {
                                presentationStringRPN.Add(charListOnStack[charListOnStack.Count - 1]);
                                charListOnStack.RemoveAt(charListOnStack.Count - 1);
                                charListOnStack.Add(regularViewList[i]);
                            }

                            if (Priority(charListOnStack[charListOnStack.Count - 1]) == 1)
                            {
                                int parentheses = charListOnStack.LastIndexOf("(");
                                for (int k = 0; k < charListOnStack.Count - 1 - parentheses;)
                                {
                                    presentationStringRPN.Add(charListOnStack[charListOnStack.Count - 1]);
                                    charListOnStack.RemoveAt(charListOnStack.Count - 1);
                                }
                                charListOnStack.Add(regularViewList[i]);
                            }

                            if (Priority(charListOnStack[charListOnStack.Count - 1]) == 3)
                            {
                                charListOnStack.Add(regularViewList[i]);
                            }
                        }

                        if (Priority(regularViewList[i]) == 1)
                        {
                            if (Priority(charListOnStack[charListOnStack.Count - 1]) == 1)
                            {
                                presentationStringRPN.Add(charListOnStack[charListOnStack.Count - 1]);
                                charListOnStack.RemoveAt(charListOnStack.Count - 1);
                                charListOnStack.Add(regularViewList[i]);
                            }
                            if (Priority(charListOnStack[charListOnStack.Count - 1]) == 2 || Priority(charListOnStack[charListOnStack.Count - 1]) == 3)
                            {
                                charListOnStack.Add(regularViewList[i]);
                            }
                        }

                    }

                    if (Priority(regularViewList[i]) == 3)
                    {
                        if (Priority(charListOnStack[charListOnStack.Count - 1]) != 3)
                        {
                            charListOnStack.Add(regularViewList[i]);
                        }
                    }

                    if (Priority(regularViewList[i]) == 4)
                    {
                        int parentheses = charListOnStack.LastIndexOf("(");
                        for (int e = 0; e < charListOnStack.Count - 1 - parentheses;)
                        {
                            presentationStringRPN.Add(charListOnStack[charListOnStack.Count - 1]);
                            charListOnStack.RemoveAt(charListOnStack.Count - 1);
                        }
                        charListOnStack.RemoveAt(charListOnStack.Count - 1);
                    }

                }

                if (i == regularViewList.Count - 1)
                {
                    for (int j = 0; j < charListOnStack.Count;)
                    {
                        presentationStringRPN.Add(charListOnStack[charListOnStack.Count - 1]);
                        charListOnStack.RemoveAt(charListOnStack.Count - 1);
                    }
                }
            }

            foreach (var item in presentationStringRPN)
            {
                Console.Write(item);
            }

            double firstValue, secondValue;

            for (int i = 0; i < presentationStringRPN.Count; i++)
            {
                if (IsNumeric(presentationStringRPN[i]) == false)
                {
                    switch (presentationStringRPN[i])
                    {
                        case "+":
                            firstValue = double.Parse(presentationStringRPN[i - 2]);
                            secondValue = double.Parse(presentationStringRPN[i - 1]);
                            presentationStringRPN[i] = Sum(firstValue, secondValue).ToString();
                            presentationStringRPN.RemoveAt(i - 2);
                            presentationStringRPN.RemoveAt(i - 2);
                            break;
                        case "-":
                            firstValue = double.Parse(presentationStringRPN[i - 2]);
                            secondValue = double.Parse(presentationStringRPN[i - 1]);
                            presentationStringRPN[i] = Subtract(firstValue, secondValue).ToString();
                            presentationStringRPN.RemoveAt(i - 2);
                            presentationStringRPN.RemoveAt(i - 2);
                            break;
                        case "*":
                            firstValue = double.Parse(presentationStringRPN[i - 2]);
                            secondValue = double.Parse(presentationStringRPN[i - 1]);
                            presentationStringRPN[i] = Multiply(firstValue, secondValue).ToString();
                            presentationStringRPN.RemoveAt(i - 2);
                            presentationStringRPN.RemoveAt(i - 2);
                            break;
                        case "/":
                            firstValue = double.Parse(presentationStringRPN[i - 2]);
                            secondValue = double.Parse(presentationStringRPN[i - 1]);
                            presentationStringRPN[i] = Divide(firstValue, secondValue).ToString();
                            presentationStringRPN.RemoveAt(i - 2);
                            presentationStringRPN.RemoveAt(i - 2);
                            break;
                        default:
                            Console.WriteLine("Что-то пошло не так");
                            break;
                    }
                    i--;
                    i--;
                }
            }
            Console.WriteLine("\nОтвет: " + presentationStringRPN[0]);
            Console.ReadLine();
        }

        public static int Priority(string value)
        {
            if (value == "*" || value == "/")
                return 1;
            else if (value == "+" || value == "-")
                return 2;
            else if (value == "(")
                return 3;
            else
                return 4;
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
                Console.WriteLine("Нельзя делить на 0");
                return a;
            }
        }
    }
}
