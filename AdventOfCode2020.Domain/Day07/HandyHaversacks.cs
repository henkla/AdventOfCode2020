using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Domain.Day07
{
    public class HandyHaversacks : BaseChallenge
    {
        private List<Rule> _parsedRules;
        private string _targetRule;

        protected override void Initialize()
        {
            var input = _inputHelper.GetInputAsLines("input.txt");
            _parsedRules = ParseAllRules(input);
            _targetRule = "shiny gold";
        }

        protected override void SolveFirst()
        {
            var possibleContainers = new HashSet<string>();
            var targetRule = _parsedRules.Single(r => string.Equals(r.Type, _targetRule));
            ExtractParents(ref possibleContainers, targetRule);

            _result[0] = possibleContainers.Count();
        }

        protected override void SolveSecond()
        {
            var ruleStack = new Stack<Rule>(_parsedRules.Where(r => string.Equals(r.Type, _targetRule)));
            var sum = 0;

            while (ruleStack.Any())
            {
                var currentRule = ruleStack.Pop();
                sum += currentRule.Children.Values.Sum();

                foreach (var rule in currentRule.Children.Keys)
                {
                    for (var _ = 0; _ < currentRule.Children[rule]; _++)
                    {
                        ruleStack.Push(rule);
                    }
                }
            }

            _result[1] = sum;
        }

        private List<Rule> ParseAllRules(IEnumerable<string> input)
        {
            var parsedRules = new List<Rule>();
            foreach (var line in input)
            {
                var ls = line.Split(" bags contain");
                parsedRules.Add(new Rule(ls[0], ls[1]));
            }

            foreach (var rule in parsedRules)
            {
                if (rule.Contents.Contains("no"))
                    continue;

                var splitContents = rule.Contents.Split(", ");
                foreach (var content in splitContents)
                {
                    var contentKeyValue = content.Trim().Split(" bag")[0].Trim().Split(' ');
                    var contentKey = $"{contentKeyValue[1]} {contentKeyValue[2]}";
                    var contentValue = int.Parse(contentKeyValue[0]);

                    var contentRule = parsedRules.Single(r => string.Equals(r.Type, contentKey));

                    contentRule.Parents.Add(rule);
                    rule.Children.Add(contentRule, contentValue);

                    rule.Contents = string.Empty;
                }
            }

            return parsedRules;
        }

        private void ExtractParents(ref HashSet<string> possibleContainers, Rule rule)
        {
            foreach (var r in rule.Parents)
            {
                possibleContainers.Add(r.Type);
                ExtractParents(ref possibleContainers, r);
            }
        }
    }
}
