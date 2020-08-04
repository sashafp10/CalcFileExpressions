using System;
using System.Text.RegularExpressions;

namespace CalculatorExtension3
{
    public class BasicMathExpression
    {
        private Regex rex = new Regex(@"([\d.,m]+)[ ]*([-+*/]{1})[ ]*([\d.,m]+)");

        public string Left { get; private set; }
        public decimal LeftValue { get; private set; }
        public string Right { get; private set; }
        public decimal RightValue { get; private set; }
        public Operation Operation { get; private set; }
        public string OriginalExpression { get; private set; }

        public BasicMathExpression(string expression)
        {
            OriginalExpression = expression;
            ParseExpression();
        }

        private void ParseExpression()
        {
            var groups = rex.Split(OriginalExpression);
            if (groups.Length != 5 || !string.IsNullOrEmpty(groups[0]) || !string.IsNullOrEmpty(groups[4]))
                throw new ArgumentException($"Expression '{OriginalExpression}' is not a proper one");

            Left = groups[1].Trim();
            Right = groups[3].Trim();
            Left = Left.Replace("m", "-");
            Right = Right.Replace("m", "-");
            Operation = SupportedOperations.FromString(groups[2]);
            
            LeftValue = decimal.Parse(Left);
            RightValue = decimal.Parse(Right);
        }
    }
}
