using PowerLanguage;
using System;
using System.IO;

namespace MCRunner
{
    /// <summary>
    /// Outputs messages to the console.
    /// </summary>
    public class ConsoleOutput : IOutput
    {
        /// <summary>
        /// A method for cleaning the console.
        /// </summary>
        public void Clear()
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
            }
        }

        /// <summary>
        /// A method that outputs to Output Window the resulting string of '_args'
        /// arguments application to '_format' formatting string.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="args">Arguments.</param>
        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        /// <summary>
        /// A method that outputs to Output Window the resulting string of the '_args'
        /// arguments application to the '_format' formatting string and then positions
        /// the cursor on a new line.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="args">Arguments.</param>
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
