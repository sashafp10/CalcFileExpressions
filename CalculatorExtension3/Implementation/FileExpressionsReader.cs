using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalculatorExtension3.Abstractions;

namespace CalculatorExtension3.Implementation
{
    public class FileExpressionsReader : IExpressionsReader
    {
        private List<string> _innerList = new List<string>();

        public FileExpressionsReader(string filePath)
        {
            Load(filePath);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _innerList?.GetEnumerator() ?? throw new InvalidOperationException("The collection contains no elements");
        }

        public void Load(params string[] args)
        {
            if (args.Length < 1)
                throw new ArgumentException("Please specify path to expressions file");

            _innerList = File.ReadAllLines(args[0]).ToList();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
