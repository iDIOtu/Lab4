using System;
using System.Globalization;
using System.Xml.Linq;

namespace LabTwo
{
    internal class Program
    {
        public static List<object> ParseMathExpression(string mathExpression)
        {
            mathExpression = mathExpression.Replace(" ", "").Replace(",", ".");
            List<object> tokens = new List<object>();

            for (int i = 0; i < mathExpression.Length; i++)
            {
                char c = mathExpression[i];
                if (Char.IsDigit(c) || c == '.')
                {
                    string numberString = c.ToString();

                    while (i + 1 < mathExpression.Length && (Char.IsDigit(mathExpression[i + 1]) || mathExpression[i + 1] == '.'))
                    {
                        numberString += mathExpression[i + 1];
                        i++;
                    }

                    tokens.Add(float.Parse(numberString, CultureInfo.InvariantCulture.NumberFormat));
                }

                else
                {
                    tokens.Add(c);
                }
            }
            return tokens;
        }

        public static List<object> RevPolNotConverter(List<object> tokens)
        {
            Stack<object> operators = new Stack<object>();
            List<object> output = new List<object>();

            Dictionary<object, int> priority = new Dictionary<object, int>()
            {
                { '*', 2 },
                { '/', 2 },
                { '+', 1 },
                { '-', 1 },
                { '(', 0 },
            };

            foreach (object token in tokens)
            {
                if (token is float)
                {
                    output.Add(token);
                }

                else if (priority.ContainsKey(token) && (char)token != '(') 
                {
                    while (operators.Count > 0 && priority[operators.Peek()] >= priority[token])
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Push(token);
                }

                else if ((char)token == '(')
                {
                    operators.Push(token);
                }

                else if ((char)token == ')')
                {
                    while ((char)operators.Peek() != '(')
                    {
                        output.Add(operators.Pop());
                    }
                    operators.Pop();
                }
            }

            while (operators.Count > 0)
            {
                output.Add(operators.Pop());
            }

            return output;
        }

        public static float CalculateRevPolNot(List<object> tokens)
        {
            Stack<float> numbers = new Stack<float>();

            foreach (object token in tokens)
            {
                if (token is float) 
                { 
                    numbers.Push((float)token);
                }

                else switch (token)
                {
                    case '*':
                        numbers.Push(numbers.Pop() * numbers.Pop());
                        break;
                    case '/':
                        numbers.Push((1/numbers.Pop()) / (1/numbers.Pop()));
                        break;
                    case '+':
                        numbers.Push(numbers.Pop() + numbers.Pop());
                        break;
                    case '-':
                        numbers.Push(-numbers.Pop() + numbers.Pop());
                        break;
                }
            }
            return numbers.Pop();
        }

        public static void Main()
        {
            Console.Write("Введите математическое выражение: ");
            List<object> tokens = RevPolNotConverter(ParseMathExpression(Console.ReadLine()));
            float number = CalculateRevPolNot(tokens);

            Console.WriteLine($"Обратная Польская Запись: {string.Join(" ", tokens)}");
            Console.Write($"Ваш результат: {number}");
        }
    }
}