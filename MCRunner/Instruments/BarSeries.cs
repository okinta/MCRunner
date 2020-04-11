using PowerLanguage;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MCRunner.Instruments
{
    /// <summary>
    /// Interface to reference historical Bar data. 
    /// </summary>
    /// <typeparam name="T">The type of Bar data to reference.</typeparam>
    public class BarSeries<T> : ISeries<T>
    {
        private IList<Bar> Bars { get; }
        private FieldInfo Field { get; }

        private readonly Type type = typeof(T);

        /// <summary>
        /// Returns data for the Bar located a specified number of bars back from the
        /// current Bar.
        /// </summary>
        /// <param name="barsAgo">The numerical expression, specifying the number of bars
        /// back.</param>
        /// <returns>The referenced data.</returns>
        public T this[int barsAgo]
        {
            get
            {
                if (barsAgo < 0)
                {
                    throw new IndexOutOfRangeException("Can't look into the future!");
                }

                var count = Bars.Count;
                var index = count - 1 - barsAgo;
                
                if (index < 0)
                {
                    throw new IndexOutOfRangeException(
                        String.Format("{0} is too far back! There are only {1} bars",
                        index, count));
                }

                var bar = Bars[index];
                var value = Field.GetValue(bar);
                return (T)Convert.ChangeType(value, type);
            }
        }

        /// <summary>
        /// Returns data for the current Bar.
        /// </summary>
        public T Value
        {
            get
            {
                return this[0];
            }
        }

        /// <summary>
        /// Instantiates a new instance.
        /// </summary>
        /// <param name="bars">The list of Bar instances to reference</param>
        /// <param name="fieldName">The name of the Bar field to reference.</param>
        public BarSeries(IList<Bar> bars, string fieldName)
        {
            if (bars is null)
            {
                throw new ArgumentNullException("bars must not be null");
            }

            Bars = bars;
            Field = typeof(Bar).GetField(
                fieldName, BindingFlags.Public | BindingFlags.Instance);

            if (Field is null)
            {
                throw new ArgumentException(
                    string.Format("{0} is not a valid Bar field", fieldName));
            }
            
            if (Field.FieldType != type)
            {
                throw new ArgumentException(
                    string.Format("{0} is not the same type as {1}", fieldName, type));
            }
        }
    }
}
