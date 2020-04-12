using MCRunner.Instruments;
using PowerLanguage;
using System.Collections.Generic;
using System.Collections.Immutable;
using System;

namespace MCRunner
{
    /// <summary>
    /// Extends classes.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Triggers the action in a thread-safe manner.
        /// </summary>
        /// <typeparam name="T">The type of argument to send with the Action.</typeparam>
        /// <param name="action">The action to trigger.</param>
        /// <param name="arg">The argument to send with the Action.</param>
        public static void SafeTrigger(this Action action)
        {
            action?.Invoke();
        }

        /// <summary>
        /// Triggers the action in a thread-safe manner.
        /// </summary>
        /// <typeparam name="T">The type of argument to send with the Action.</typeparam>
        /// <param name="action">The action to trigger.</param>
        /// <param name="arg">The argument to send with the Action.</param>
        public static void SafeTrigger<T>(this Action<T> action, T arg)
        {
            action?.Invoke(arg);
        }

        /// <summary>
        /// Gets the size of contracts.
        /// </summary>
        /// <param name="contracts">The Contracts instance to get the size for.</param>
        /// <returns>The number of contracts.</returns>
        public static double GetSize(this Contracts contracts)
        {
            var size = 100;

            if (contracts.IsUserSpecified)
            {
                size = contracts.Contract;
            }

            return size;
        }

        /// <summary>
        /// Gets the size of contracts.
        /// </summary>
        /// <param name="contracts">The Contracts instance to get the size for.</param>
        /// <param name="numLots">The number of Lots.</param>
        /// <returns>The number of contracts.</returns>
        public static double GetSize(this Contracts contracts, int numLots)
        {
            var size = contracts.GetSize();

            if (contracts.IsUserSpecified)
            {
                size = numLots;
            }

            return size;
        }

        /// <summary>
        /// Performs the specified action on each element of the list.
        /// </summary>
        /// <typeparam name="T">The generic type of the IEnumerable.</typeparam>
        /// <param name="source">The list to perform actions on.</param>
        /// <param name="action">The action to perform. The element is provided as an
        /// argument.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }
    }
}
