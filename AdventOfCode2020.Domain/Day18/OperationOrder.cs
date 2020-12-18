using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static AdventOfCode2020.Domain.Result;

namespace AdventOfCode2020.Domain.Day18
{
    public class OperationOrder : BaseChallenge
    {
        private IEnumerable<string> _input;

        protected override void Initialize()
        {
            _input = InputReader.ReadFile("input.txt");
        }

        protected override PartialResult SolveFirst()
        {
            var sum = 0L;
            foreach (var line in _input)
            {
                sum += SimpleEvaluation(line);
            }

            return new PartialResult(1, sum, Stopwatch.Elapsed);
        }

        protected override PartialResult SolveSecond()
        {
            var sum = 0L;
            foreach (var line in _input)
            {
                sum += AdvancedEvaluation(line);
            }

            return new PartialResult(2, sum, Stopwatch.Elapsed);
        }

        private long SimpleEvaluation(string expression)
        {
            expression = expression.Replace(" ", "");
            expression = FlattenExpression(SimpleEvaluation, expression);

            var unset = ' ';
            var left = "";
            var right = "";
            var operation = unset;

            foreach (var e in expression)
            {
                if (int.TryParse(e.ToString(), out int number))
                {
                    if (operation == unset) left += number;
                    else right += number;
                }
                else if (operation == unset)
                {
                    operation = e;
                }
                else
                {
                    left = Calculate(long.Parse(left), operation, long.Parse(right)).ToString();
                    right = "";
                    operation = e;
                }
            }

            return Calculate(long.Parse(left), operation, long.Parse(right));
        }

        private long AdvancedEvaluation(string expression)
        {
            expression = expression.Replace(" ", "");
            expression = FlattenExpression(AdvancedEvaluation, expression);

            var terms = expression
                .Split('*')
                .Where(s => s.Contains('+'))
                .ToList();

            foreach (var term in terms)
            {
                var sum = term
                    .Split('+')
                    .Select(s => long.Parse(s))
                    .Sum();

                if (Equals(term, expression))
                    return sum;

                expression = new Regex(Regex.Escape(term)).Replace(expression, sum.ToString(), 1);
            }

            return expression
                .Split('*')
                .Select(s => long.Parse(s))
                .Aggregate((a, b) => a * b);
        }

        private string FlattenExpression(Func<string, long> eval, string expression)
        {
            while (expression.Contains('('))
            {
                var leftParentheses = 0;
                var rightParentheses = 0;
                var indexOfEndingParenthesis = 0;

                foreach (var index in Enumerable.Range(0, expression.Length))
                {
                    var evaluatedChar = expression[index];

                    if (evaluatedChar == '(') 
                        leftParentheses++;
                    else if (evaluatedChar == ')') 
                        rightParentheses++;

                    if (leftParentheses > 0 && leftParentheses == rightParentheses)
                    {
                        indexOfEndingParenthesis = index;
                        break;
                    }
                }

                var indexOfStartingParenthesis = expression.IndexOf('(');
                var subExpression = expression.Substring(indexOfStartingParenthesis + 1, indexOfEndingParenthesis - indexOfStartingParenthesis - 1);
                var subResult = eval.Invoke(subExpression);
                expression = expression.Replace("(" + subExpression + ")", subResult.ToString());
            }

            return expression;
        }

        private long Calculate(long left, char operation, long right)
        {
            return operation switch
            {
                '+' => left + right,
                '*' => left * right,
                _ => throw new InvalidOperationException($"The operator '{operation}' is not allowed.")
            };
        }
    }
}
