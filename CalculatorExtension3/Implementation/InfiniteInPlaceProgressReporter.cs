using System;
using CalculatorExtension3.Abstractions;

namespace CalculatorExtension3.Implementation
{
    public class InfiniteInPlaceProgressReporter: IProgressReporter
    {
        private bool _isProgressWorks = true;
        public int Max { get; set; }
        public int Current { get; set; }
        public void Start()
        {
            Current = 0;
        }

        public void Step(int delta)
        {
            Current += delta;

            if (_isProgressWorks)
            {
                try
                {
                    var ct = Console.CursorLeft;

                    if (Current > 1)
                        Console.CursorLeft -= 1;

                    switch (Current % 4)
                    {
                        case 0:
                            Console.Write("/");
                            break;
                        case 1:
                            Console.Write("-");
                            break;
                        case 2:
                            Console.Write("\\");
                            break;
                        case 3:
                            Console.Write("|");
                            break;
                    }
                }
                catch (Exception)
                {
                    _isProgressWorks = false;
                }
            }
        }
        public void Stop()
        {
            if (_isProgressWorks && Current > 0)
                Console.CursorLeft -= 1;
        }
    }
}
