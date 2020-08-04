using System;
using System.Collections;
using System.Collections.Generic;
using CalculatorExtension3.Abstractions;

namespace CalculatorExtension3.Implementation
{
    public class HardcodedExpressionsReader: IExpressionsReader
    {
        private List<string> _innerList;

        public void Load(params string[] args)
        {
            _innerList = new List<string>()
            {
                "1+1",
                "2-5",
                "18*3",
                "2 * (18-3) + 7 *(2 + 3) - 1",
                "2 * ((18-3) + 7) *(2 + 3) - 1",
                "2 * ((2+3) + 7) *(2 + 3) - 1",
                "2 * ((2+3) + 7.25) / (2 + 3) / 2 * 10"
            };
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _innerList?.GetEnumerator() ?? throw new InvalidOperationException("The collection contains no elements");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
