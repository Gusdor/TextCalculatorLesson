﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalculatorLesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Operator[] operators = new []
            {
                new Operator { Symbol = '+', Operation = (left, right) => left + right },
                new Operator { Symbol = '-', Operation = (left, right) => left - right },
                new Operator { Symbol = '*', Operation = (left, right) => left * right },
                new Operator { Symbol = '/', Operation = (left, right) => left / right },
            };

            while (true)
            {
                try
                {
                    Console.WriteLine("Please type a calculation and press ENTER.");
                    string input = Console.ReadLine();

                    int result;
                    result = ParseWithRegex(input, operators);
                    //result = ParseWithStringSplit(input, operators);

                    Console.WriteLine($"Result: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        
        
        private static int ParseWithStringSplit(string input, Operator[] availableOperators)
        {
            // Parse the user input to get 2 operands and a an operator
            string[] operands = input.Split('+');

            if (operands.Length != 2)
            {
                throw new Exception($"Input string must contain 2 operands. {operands.Length} were detected.");
            }
            else
            {
                int left;
                int right;
                if (int.TryParse(operands[0], out left) && int.TryParse(operands[1], out right))
                {
                    return left + right;
                }
                else
                {
                    throw new Exception("One or more operands was not a number.");
                }
            }
        }

        private static int ParseWithRegex(string input, Operator[] availableOperators)
        {
            string symbolRegex = string.Join("|", availableOperators.Select(p => "\\" + p.Symbol));
            var match = Regex.Match(input, $@"(\d+)({symbolRegex})(\d+)");
            if (match.Success)
            {
                int left = int.Parse(match.Groups[1].Value);
                int right = int.Parse(match.Groups[3].Value);

                Operator matchingOperator = availableOperators.FirstOrDefault(p => p.Symbol == match.Groups[2].Value[0]);

                if (matchingOperator != null)
                {
                    return matchingOperator.Operation(left, right);
                }
                else
                {
                    throw new Exception("Operator not supported.");
                }
            }
            else
            {
                throw new Exception("Input string must contain 2 numeric operands and an operator.");
            }
        }
    }

    class Operator
    {
        public char Symbol { get; set; }

        public Func<int, int, int> Operation { get; set; }
    }
}
