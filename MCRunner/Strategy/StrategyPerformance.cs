using ATCenterProxy.interop;
using PowerLanguage;
using System;

namespace MCRunner.Strategy
{
    /// <summary>
    /// Allows accessing of strategy information.
    /// </summary>
    public class StrategyPerformance : IStrategyPerformance
    {
        /// <summary>
        /// Read-only property. Returns average entry price.
        /// </summary>
        public double AvgEntryPrice => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns potential profit, i.e. current Profit and Loss, aka
        /// PnL.
        /// </summary>
        public double OpenEquity => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns fixed profit.
        /// </summary>
        public double ClosedEquity => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns current Market Position for strategy on the chart.
        /// </summary>
        public int MarketPosition { get; set; }

        /// <summary>
        /// Read-only property. Returns Market Position requested from the broker.
        /// </summary>
        public int MarketPositionAtBroker
        {
            get
            {
                return (int)marketPositionAtBroker;
            }
        }

        /// <summary>
        /// Read-only property. Returns Market Position requested from the broker for the
        /// current strategy.
        /// </summary>
        public int MarketPositionAtBrokerForTheStrategy => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns average entry price for position that was requested
        /// from the broker.
        /// </summary>
        public double AvgEntryPriceAtBroker => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns average entry price for position requested from the
        /// broker for the current strategy.
        /// </summary>
        public double AvgEntryPriceAtBrokerForTheStrategy => throw new NotImplementedException();

        /// <summary>
        /// Read-only property. Returns signals consisting strategy.
        /// </summary>
        public IStrategy[] Signals => throw new NotImplementedException();

        private double marketPositionAtBroker = 0;

        /// <summary>
        /// Call to update the market position at broker.
        /// </summary>
        /// <param name="positionChange">The change in position.</param>
        public void UpdateMarketPositionAtBroker(double positionChange)
        {
            marketPositionAtBroker += positionChange;
        }

        /// <summary>
        /// Converts amount of money from one currency into another one according to
        /// currency rate on specified date.
        /// </summary>
        /// <param name="when">The date to specify.</param>
        /// <param name="from">Original currency of the amount of money.</param>
        /// <param name="to">Resulting currency of the amount of money.</param>
        /// <param name="value">The amount of money.</param>
        public double ConvertCurrency(DateTime when, MTPA_MCSymbolCurrency from, MTPA_MCSymbolCurrency to, double value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns "plot_num" value for the current bar that has been set previously by
        /// SetPlotValue.
        /// </summary>
        /// <param name="plot_num">Some identifier (key)</param>
        public double GetPlotValue(int plot_num)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets "plot_num" value for the current bar.
        /// </summary>
        /// <param name="plot_num">Some identifier (key)</param>
        /// <param name="value">Value</param>
        public void SetPlotValue(int plot_num, double value)
        {
            throw new NotImplementedException();
        }
    }
}
