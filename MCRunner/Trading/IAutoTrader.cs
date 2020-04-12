using MCRunner.Instruments;
using MCRunner.Orders;
using PowerLanguage;
using System.Collections.Generic;
using System;

namespace MCRunner.Trading
{
    /// <summary>
    /// Interface to represent an automatic trader.
    /// </summary>
    public interface IAutoTrader
    {
        /// <summary>
        /// Triggered when an order is ready to be sent to the market.
        /// </summary>
        public event Action<OrderInfo> OrderTriggered;

        /// <summary>
        /// Triggered when an order should be canceled.
        /// </summary>
        public event Action<OrderInfo> OrderCanceled;

        /// <summary>
        /// The name of the strategy being traded.
        /// </summary>
        public string StrategyName { get; }

        /// <summary>
        /// The symbol being traded.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Information about the performance of the strategy being traded.
        /// </summary>
        public IStrategyPerformance StrategyInfo { get; }

        /// <summary>
        /// Subscribes to the given chart and starts trading.
        /// </summary>
        /// <param name="bars">The chart to subscribe to.</param>
        public void Start(IEnumerable<IMonitoredInstrument> bars);

        /// <summary>
        /// Stops trading.
        /// </summary>
        public void Stop();

        /// <summary>
        /// Call to update the market position at broker.
        /// </summary>
        /// <param name="positionChange">The change in position.</param>
        public void UpdateMarketPositionAtBroker(double positionChange);
    }
}
