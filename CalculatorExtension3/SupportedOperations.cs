using System;

namespace CalculatorExtension3
{
    public class SupportedOperations
    {
        public static readonly Operation Plus = new Operation("+", 3);
        public static readonly Operation Minus = new Operation("-", 3);
        public static readonly Operation Multiply = new Operation("*", 2);
        public static readonly Operation Divide = new Operation("/", 2);

        public static readonly Operation[] Operations = new [] { Plus, Minus, Multiply, Divide };

        //TODO: akolyada: add injection of calculation way

        public static Operation FromString(string operation)
        {
            switch (operation.Trim())
            {
                case "+":
                    return Plus;
                case "-":
                    return Minus;
                case "*":
                    return Multiply;
                case "/":
                    return Divide;
                default:
                    throw new NotSupportedException($"Operation '{operation}' is not supported");
            }
        }
    }
}
