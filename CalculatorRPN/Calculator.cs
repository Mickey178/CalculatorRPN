using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorRPN
{
    public class Calculator
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
            List<string> representationRPN = new List<string>(ConvertToRPN(input));

            foreach (var item in representationRPN)
            {
                Console.Write(item);
            }

            return CalculateRPN(representationRPN);
        }
        
        public static string CalculateRPN(List<string> representationRPN)
        {
            for (int i = 0; i < representationRPN.Count; i++)
            {
                if (IsPartOfNumeric(representationRPN[i]) == false)
                {
                    if (representationRPN[i] == "+" || representationRPN[i] == "-" || representationRPN[i] == "*" || representationRPN[i] == "/")
                    {
                        representationRPN[i] = CalculateMathAction(representationRPN[i - 2], representationRPN[i - 1], representationRPN[i]);
                        representationRPN.RemoveAt(i - 2);
                        representationRPN.RemoveAt(i - 2);
                        i -= 2;
                    }
                    else
                    {
                        Console.WriteLine("Что-то пошло не так");
                        logger.Error("Введен некорректный символ математической операции");
                        continue;
                    }
                }
            }
            return representationRPN[0];
        }

        public static List<string> ConvertToRPN(string mathematicalOrdinaryRepresentation)
        {
            List<string> calculateViewList = mathematicalOrdinaryRepresentation.Select(i=> new string(i,1)).ToList();

            for (int i = 0; i < calculateViewList.Count - 1; i++)
            {
                if (IsPartOfNumeric(calculateViewList[i]) && IsPartOfNumeric(calculateViewList[i + 1]))
                {
                    calculateViewList[i + 1] = calculateViewList[i] + calculateViewList[i + 1];
                    calculateViewList.RemoveAt(i);
                    i--;
                }
            }

            List<string> mathematicalCharListOnStack = new List<string>();
            List<string> representationRPN = new List<string>();

            for (int i = 0; i < calculateViewList.Count; i++)
            {
                if (IsPartOfNumeric(calculateViewList[i]))
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
                        if (GetOperatorPriority(calculateViewList[i]) == 2)
                        {
                            if (GetOperatorPriority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 2)
                            {
                                representationRPN.Add(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]);
                                mathematicalCharListOnStack.RemoveAt(mathematicalCharListOnStack.Count - 1);
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }

                            if (GetOperatorPriority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 1)
                            {
                                int parentheses = mathematicalCharListOnStack.LastIndexOf("(");
                                for (int k = 0; k < mathematicalCharListOnStack.Count - 1 - parentheses;)
                                {
                                    representationRPN.Add(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]);
                                    mathematicalCharListOnStack.RemoveAt(mathematicalCharListOnStack.Count - 1);
                                }
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }

                            if (GetOperatorPriority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 3)
                            {
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }
                        }

                        if (GetOperatorPriority(calculateViewList[i]) == 1)
                        {
                            if (GetOperatorPriority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 1)
                            {
                                representationRPN.Add(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]);
                                mathematicalCharListOnStack.RemoveAt(mathematicalCharListOnStack.Count - 1);
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }
                            if (GetOperatorPriority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 2 || GetOperatorPriority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) == 3)
                            {
                                mathematicalCharListOnStack.Add(calculateViewList[i]);
                            }
                        }

                    }

                    if (GetOperatorPriority(calculateViewList[i]) == 3)
                    {
                        if (GetOperatorPriority(mathematicalCharListOnStack[mathematicalCharListOnStack.Count - 1]) != 3)
                        {
                            mathematicalCharListOnStack.Add(calculateViewList[i]);
                        }
                    }

                    if (GetOperatorPriority(calculateViewList[i]) == 4)
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

        public static string CalculateMathAction(string operand1,string operand2,string action)
        {
            switch (action)
            {
                case "+":
                    return (double.Parse(operand1) + double.Parse(operand2)).ToString();
                case "-":
                    return (double.Parse(operand1) - double.Parse(operand2)).ToString();
                case "*":
                    return (double.Parse(operand1) * double.Parse(operand2)).ToString();
                default:
                    return Divide(double.Parse(operand1), double.Parse(operand2)).ToString();
            }
        }

        public static int GetOperatorPriority(string @operator)
        {
            if (@operator == "*" || @operator == "/")
                return 1;
            else if (@operator == "+" || @operator == "-")
                return 2;
            else if (@operator == "(")
                return 3;
            else if (@operator == ")")
                return 4;
            else
                return 1;
        }

        public static bool IsPartOfNumeric(string value)
        {
            if (value == ",")
                return true;
            return double.TryParse(value, out _);
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
                throw new DivideByZeroException("cannot be divided by zero");
            }
        }
    }
}
