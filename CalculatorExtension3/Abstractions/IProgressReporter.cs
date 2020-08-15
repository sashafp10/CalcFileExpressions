namespace CalculatorExtension3.Abstractions
{
    public interface IProgressReporter
    {
        /// <summary>
        /// Progress max value
        /// </summary>
        int Max { get; set; }

        /// <summary>
        /// Progress current value
        /// </summary>
        int Current { get; set; }

        /// <summary>
        /// Start action
        /// </summary>
        void Start();

        /// <summary>
        /// Increase current value to a specified number
        /// </summary>
        /// <param name="delta"></param>
        void Step(int delta);
        
        /// <summary>
        /// Stop the progress. This is a perfect place to clear canvas/console etc.
        /// </summary>
        void Stop();
    }
}
