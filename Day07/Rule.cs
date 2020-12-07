using System.Collections.Generic;

namespace Day07
{
    public class Rule
    {
        public string Type { get; private set; }
        public int Count { get; set; }
        public ICollection<Rule> Parents { get; set; } = new List<Rule>();
        public IDictionary<Rule, int> Children { get; set; } = new Dictionary<Rule, int>();
        public string Contents;

        public Rule(string type, string contents)
        {
            Type = type;
            Contents = contents;
        }
    }
}
