namespace MCRunner.Instruments
{
    /// <summary>
    /// Allows adding of bars to a chart.
    /// </summary>
    public class LoadableBars : Bars
    {
        /// <summary>
        /// Adds a Bar to the chart.
        /// </summary>
        /// <param name="bar">The Bar to add to the chart.</param>
        public virtual void AddBar(Bar bar)
        {
            bars = bars.Add(bar);
            ReloadBarSeries();
        }
    }
}
