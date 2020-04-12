using System;
using System.Collections.Generic;

namespace MCRunner.Instruments
{
    /// <summary>
    /// Allows bars to be played sequentially on a chart. 
    /// </summary>
    public class PlayableBars : Bars
    {
        private IEnumerable<Bar> BarsToPlay { get; set; }

        /// <summary>
        /// Instantiates a new instance.
        /// </summary>
        /// <param name="barsToPlay">The collection of Bar instances to play
        /// sequentially.</param>
        public PlayableBars(IEnumerable<Bar> barsToPlay)
        {
            BarsToPlay = new List<Bar>(barsToPlay);
        }

        /// <summary>
        /// Loads the Bar instances sequentially and calls the given action.
        /// </summary>
        /// <param name="action">The action to call after a Bar is loaded.</param>
        public void Play(Action action)
        {
            Play(bar => action());
        }

        /// <summary>
        /// Loads the Bar instances sequentially and calls the given action.
        /// </summary>
        /// <param name="action">The action to call after a Bar is loaded. The Actoin
        /// receives the Bar instance as its first argument.</param>
        public void Play(Action<Bar> action)
        {
            bars.Clear();

            foreach (var bar in BarsToPlay)
            {
                bars = bars.Add(bar);
                ReloadBarSeries();
                action(bar);
            }

            BarsToPlay = null;
        }
    }
}
