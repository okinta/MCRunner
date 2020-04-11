using FluentAssertions.Events;
using FluentAssertions.Execution;
using System.Linq;

namespace Tests
{
    /// <summary>
    /// Extends classes.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Asserts that only one event was raised.
        /// </summary>
        /// <param name="eventRecorder">The IEventRecorder to check.</param>
        /// <returns>The IEventRecorder instance for chaining.</returns>
        public static IEventRecorder OnlyOnce(this IEventRecorder eventRecorder)
        {
            var count = eventRecorder.Count();
            if (count != 1)
            {
                Execute.Assertion.FailWith(
                    "Expected only 1 event to be raised, but found {0}.", count);
            }

            return eventRecorder;
        }
    }
}
