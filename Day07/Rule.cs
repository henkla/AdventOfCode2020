using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day07
{
    public class Rule
    {
        public string Type { get; private set; }
        public ICollection<Rule> Parents { get; set; } = new List<Rule>();
        public ICollection<Rule> Children { get; set; } = new List<Rule>();
        public string Contents;

        public Rule(string type, string contents)
        {
            Type = type;
            Contents = contents;
        }
    }
}
