using PowerLanguage;
using System;

namespace MCRunner.Instruments
{
    /// <summary>
    /// Interface for series data access that can be monitored for updates.
    /// </summary>
    public interface IMonitoredInstrument : IInstrument
    {
        /// <summary>
        /// Triggered when the instrument is updated.
        /// </summary>
        public event Action<Bar> Updated;
    }
}
