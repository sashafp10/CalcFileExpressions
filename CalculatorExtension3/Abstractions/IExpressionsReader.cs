using System.Collections.Generic;

namespace CalculatorExtension3.Abstractions
{
    public interface IExpressionsReader : IEnumerable<string>
    {
        /// <summary>
        /// Load data from the source
        /// </summary>
        void Load(params string[] args);
    }
}
