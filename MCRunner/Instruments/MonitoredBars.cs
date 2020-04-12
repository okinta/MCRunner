using System.Collections.Generic;
using System;

namespace MCRunner.Instruments
{
    /// <summary>
    /// Allows subscribers to be notified when a new Bar is added to the chart.
    /// </summary>
    public class MonitoredBars : LoadableBars, IMonitoredInstrument
    {
        /// <summary>
        /// Triggered when a new Bar is added to the chart.
        /// </summary>
        public event Action<Bar> Updated;

        /// <summary>
        /// Adds a Bar to the chart. Triggers the Updated event to notify subscribers
        /// that the chart has been updated.
        /// </summary>
        /// <param name="bar">The Bar to add to the chart.</param>
        public override void AddBar(Bar bar)
        {
            base.AddBar(bar);
            Updated.SafeTrigger(bar);
        }

        /// <summary>
        /// Converts this instance into a datastream item that can be loaded into
        /// StrategyRunner.
        /// </summary>
        /// <returns>A datastream with this Bars instance as its single source of
        /// data.</returns>
        public new IEnumerable<IMonitoredInstrument> ToSingleDataStream()
        {
            return new List<IMonitoredInstrument> { this };
        }
    }
}
