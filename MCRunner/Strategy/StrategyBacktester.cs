using Bar = MCRunner.Instruments.Bar;
using MCRunner.Instruments;
using MCRunner.Orders;
using OrderCreator = MCRunner.Orders.OrderCreator;
using PowerLanguage.Strategy;
using PowerLanguage;
using System.Collections.Generic;
using System;

namespace MCRunner.Strategy
{
    public class StrategyBacktester<T> : IStrategyBacktester<T> where T : SignalObject
    {
        /// <summary>
        /// Gets the strategy instance this backtester is backtesting.
        /// </summary>
        public T Strategy { get; private set; }

        /// <summary>
        /// Gets the strategy performance information of this backtest.
        /// </summary>
        public IStrategyPerformance StrategyInfo { get; private set; }

        /// <summary>
        /// Triggered when an order is sent during the backtest.
        /// </summary>
        public event Action<OrderInfo> OrderSent;

        private readonly StrategyRunner<T> runner;
        private readonly PlayableBars playableBars;

        /// <summary>
        /// Instantiates a new instance.
        /// </summary>
        /// <param name="bars">The list of Bar instances to use during the backtest.</param>
        public StrategyBacktester(IEnumerable<Bar> bars)
        {
            var orderCreator = new OrderCreator();
            orderCreator.OrderSent += (order) => OrderSent.SafeTrigger(order);

            var manager = new StrategyManager(orderCreator);
            StrategyInfo = manager.StrategyInfo;

            playableBars = new PlayableBars(bars);

            runner = new StrategyRunner<T>(
                playableBars.ToSingleDataStream(),
                orderCreator: orderCreator,
                strategyInfo: StrategyInfo);
            Strategy = runner.Strategy;
        }

        /// <summary>
        /// Backtests the strategy.
        /// </summary>
        public void Backtest()
        {
            Backtest((bar) => { });
        }

        /// <summary>
        /// Backtests the strategy.
        /// </summary>
        /// <param name="action">The Action to call after a new Bar is processed.</param>
        public void Backtest(Action action)
        {
            Backtest((bar) => action());
        }

        /// <summary>
        /// Backtests the strategy.
        /// </summary>
        /// <param name="action">The Action to call after a new Bar is processed. The
        /// first argument will be the Bar instance that was processed.</param>
        public void Backtest(Action<Bar> action)
        {
            runner.Create();
            runner.StartCalc();
            playableBars.Play((bar) =>
            {
                runner.CalcBar();
                action(bar);
            });
        }
    }
}
