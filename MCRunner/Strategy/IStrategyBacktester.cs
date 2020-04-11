using Bar = MCRunner.Instruments.Bar;
using MCRunner.Orders;
using PowerLanguage.Strategy;
using PowerLanguage;
using System;

namespace MCRunner.Strategy
{
    /// <summary>
    /// Interface to represent a strategy backtester.
    /// </summary>
    /// <typeparam name="T">The type of strategy to backtest.</typeparam>
    public interface IStrategyBacktester<T> : IOrderManaged where T : SignalObject
    {
        /// <summary>
        /// Gets the strategy instance this backtester is backtesting.
        /// </summary>
        public T Strategy { get; }

        /// <summary>
        /// Gets the strategy performance information of this backtest.
        /// </summary>
        public IStrategyPerformance StrategyInfo { get; }

        /// <summary>
        /// Backtests the strategy.
        /// </summary>
        public void Backtest();

        /// <summary>
        /// Backtests the strategy.
        /// </summary>
        /// <param name="action">The Action to call after a new Bar is processed.</param>
        public void Backtest(Action action);

        /// <summary>
        /// Backtests the strategy.
        /// </summary>
        /// <param name="action">The Action to call after a new Bar is processed. The
        /// first argument will be the Bar instance that was processed.</param>
        public void Backtest(Action<Bar> action);
    }
}
