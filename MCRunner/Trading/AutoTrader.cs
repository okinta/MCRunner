using MCRunner.Instruments;
using MCRunner.Orders;
using MCRunner.Strategy;
using OrderCreator = MCRunner.Orders.OrderCreator;
using PowerLanguage.Strategy;
using PowerLanguage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MCRunner.Trading
{
    /// <summary>
    /// Describes methods to automatically trade a strategy on a chart.
    /// </summary>
    /// <typeparam name="T">The type of strategy to trade.</typeparam>
    public class AutoTrader<T> : IAutoTrader where T : SignalObject
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
        public string StrategyName
        {
            get
            {
                return type.Name;
            }
        }

        /// <summary>
        /// The symbol being traded.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Information about the performance of the strategy being traded.
        /// </summary>
        public IStrategyPerformance StrategyInfo { get; private set; }

        private IEnumerable<IMonitoredInstrument> Bars { get; set; }
        private IOutput Output { get; }
        private OrderCreator OrderCreator { get; } = new OrderCreator();
        private readonly Type type = typeof(T);
        private StrategyManager Manager { get; }
        private StrategyRunner<T> Runner { get; set; }

        private IMonitoredInstrument PrimaryChart
        {
            get
            {
                return Bars.First();
            }
        }

        /// <summary>
        /// Instantiates the instance.
        /// </summary>
        /// <param name="symbol">The symbol to trade.</param>
        /// <param name="output">The IOuput instance to write to.</param>
        public AutoTrader(string symbol, IOutput output = null)
        {
            Manager = new StrategyManager(OrderCreator);

            if (output is null)
            {
                output = new ConsoleOutput();
            }

            Output = output;
            StrategyInfo = Manager.StrategyInfo;
            Symbol = symbol;
        }

        /// <summary>
        /// Subscribes to the given chart and starts trading.
        /// </summary>
        /// <param name="bars">The chart to subscribe to.</param>
        public void Start(IEnumerable<IMonitoredInstrument> bars)
        {
            if (Runner is object)
            {
                throw new InvalidOperationException("AutoTrader is already running.");
            }

            Manager.OrderValidated += OnOrderValidated;
            Manager.OrderCanceled += OnOrderCanceled;

            Bars = new List<IMonitoredInstrument>(bars);
            Runner = new StrategyRunner<T>(Bars, OrderCreator, Output, StrategyInfo);
            Runner.Create();
            Runner.StartCalc();

            // We only send orders based on the primary chart. The secondary charts are
            // used for reference within the strategy.
            PrimaryChart.Updated += OnBarsUpdated;
        }

        /// <summary>
        /// Stops trading.
        /// </summary>
        public void Stop()
        {
            if (Runner is null)
            {
                throw new InvalidOperationException("AutoTrader isn't running");
            }

            Manager.OrderValidated -= OnOrderValidated;
            Manager.OrderCanceled -= OnOrderCanceled;

            Bars.ForEach((bar) => bar.Updated -= OnBarsUpdated);
            Bars = null;
            Runner = null;
        }

        /// <summary>
        /// Call to update the market position at broker.
        /// </summary>
        /// <param name="positionChange">The change in position.</param>
        public void UpdateMarketPositionAtBroker(double positionChange)
        {
            Manager.UpdateMarketPositionAtBroker(positionChange);
        }

        /// <summary>
        /// Called when a new Bar is added to the chart. Processes the latest Bar.
        /// </summary>
        private void OnBarsUpdated(Instruments.Bar bar)
        {
            if (Runner is null)
            {
                throw new InvalidOperationException("AutoTrader isn't running");
            }

            Runner.CalcBar();
            Manager.TriggerOrders(bar);
        }

        private void OnOrderValidated(OrderInfo order)
        {
            OrderTriggered.SafeTrigger(order);
        }

        private void OnOrderCanceled(OrderInfo order)
        {
            OrderCanceled.SafeTrigger(order);
        }
    }
}
