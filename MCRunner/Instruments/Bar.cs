using System;

namespace MCRunner.Instruments
{
    /// <summary>
    /// Represents the type of bar on a chart.
    /// </summary>
    public enum BarType
    {
        /// <summary>
        /// Indicates that the bar is a real-time bar pulled from a data feed.
        /// </summary>
        Live,

        /// <summary>
        /// Indicates that the bar is a historic bar.
        /// </summary>
        Historic
    }

    /// <summary>
    /// Represents a bar on a chart.
    /// </summary>
    public struct Bar
    {
        /// <summary>
        /// The time of this Bar.
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// The high of this Bar
        /// </summary>
        public double High;

        /// <summary>
        /// The low of this Bar.
        /// </summary>
        public double Low;

        /// <summary>
        /// The open of this Bar.
        /// </summary>
        public double Open;

        /// <summary>
        /// The close of this Bar.
        /// </summary>
        public double Close;
        
        /// <summary>
        /// The volume of this Bar.
        /// </summary>
        public double Volume;

        /// <summary>
        /// Whether the bar is live or historic.
        /// </summary>
        public BarType BarType;
    }
}
