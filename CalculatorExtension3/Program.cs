using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CalculatorExtension3.Abstractions;
using CalculatorExtension3.Implementation;

namespace CalculatorExtension3
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: akolyada: introduce DI
            var se = new SimpleEvaluator();
            IExpressionsReader rdr = new FileExpressionsReader("../../input_numbers.txt");
            List<Tuple<string, decimal>> results = new List<Tuple<string, decimal>>();
            foreach (string expression in rdr)
            {
                Console.Write($"{expression} = ");
                string expr = expression;
                var res = se.Evaluate(expr);
                results.Add(new Tuple<string, decimal>(expr, res));
                Console.WriteLine(res);
            }

            File.WriteAllLines($"../../calculationResults_{DateTime.Now.Ticks}.txt", results.Select(n => $"{n.Item1} = {n.Item2}").ToArray());

            Console.WriteLine();
            Console.Write("Press any key...");

            Console.ReadKey();
        }
    }
}
