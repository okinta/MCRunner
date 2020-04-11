using MCRunner.Instruments;
using MCRunner.Orders;
using PowerLanguage.Strategy;
using PowerLanguage;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System;

namespace MCRunner.Trading
{
    internal struct AutoTraderInfo
    {
        public IAutoTrader AutoTrader;
        public Action<OrderInfo> OnOrderTriggered;
        public Action<OrderInfo> OnOrderCanceled;
    }

    /// <summary>
    /// Manages a collection of automatic traders for a collection of strategies and
    /// symbols.
    /// </summary>
    public class AutoTradingManager
    {
        /// <summary>
        /// Triggered when an order is ready to be sent to the market.
        /// </summary>
        public event Action<IAutoTrader, OrderInfo> OrderTriggered;

        /// <summary>
        /// Triggered when an order should be canceled.
        /// </summary>
        public event Action<IAutoTrader, OrderInfo> OrderCanceled;

        private bool isTrading = false;
        private IReadOnlyList<AutoTraderInfo> AutoTraders { get; }

        /// <summary>
        /// Instantiates the instance.
        /// </summary>
        /// <param name="strategies">The list of strategies to trade.</param>
        /// <param name="symbols">The list of symbols to trade.</param>
        /// <param name="output">Optional IOutput instance to use. If not provided a new
        /// ConsoleOutput instance will be created.</param>
        public AutoTradingManager(
            IEnumerable<Type> strategies, IEnumerable<string> symbols,
            IOutput output = null)
        {
            strategies.ForEach((strategy) => ValidateStrategy(strategy));

            if (output is null)
            {
                output = new ConsoleOutput();
            }

            var autoTraders = new List<AutoTraderInfo>();
            foreach (var strategy in strategies)
            {
                foreach (var symbol in symbols)
                {
                    autoTraders.Add(CreateAutoTrader(strategy, symbol, output));
                }
            }

            AutoTraders = autoTraders;
        }

        /// <summary>
        /// Subscribes to the given chart and starts trading.
        /// </summary>
        /// <param name="bars">The chart to subscribe to.</param>
        public void Start(ImmutableList<IMonitoredInstrument> bars)
        {
            if (isTrading)
            {
                throw new InvalidOperationException(
                    "AutoTradingManager is already running");
            }

            foreach (var trader in AutoTraders)
            {
                trader.AutoTrader.OrderTriggered += trader.OnOrderTriggered;
                trader.AutoTrader.OrderCanceled += trader.OnOrderCanceled;
                trader.AutoTrader.Start(bars);
            }

            isTrading = true;
        }

        /// <summary>
        /// Stops trading.
        /// </summary>
        public void Stop()
        {
            if (!isTrading)
            {
                throw new InvalidOperationException("AutoTradingManager isn't running");
            }

            foreach (var trader in AutoTraders)
            {
                trader.AutoTrader.OrderTriggered -= trader.OnOrderTriggered;
                trader.AutoTrader.OrderCanceled -= trader.OnOrderCanceled;
                trader.AutoTrader.Stop();
            }

            isTrading = true;
        }

        /// <summary>
        /// Validates the given type to ensure it is in fact a strategy. Throws an
        /// exception if it's not a valid strategy.
        /// </summary>
        /// <param name="strategy">The type to validate.</param>
        private void ValidateStrategy(Type strategy)
        {
            var info = strategy.GetTypeInfo();

            if (!info.IsSubclassOf(typeof(SignalObject)))
            {
                throw new ArgumentException(
                    string.Format("{0} does not inherit from SignalObject", strategy));
            }

            if (info.IsAbstract)
            {
                throw new ArgumentException(
                    string.Format("{0} must not be abstract", strategy));
            }
        }

        /// <summary>
        /// Creates a new auto trader.
        /// </summary>
        /// <param name="strategy">The strategy to trade.</param>
        /// <param name="symbol">The symbol to trade.</param>
        /// <param name="output">The IOutput instance to use.</param>
        /// <returns>The newly configured auto trader.</returns>
        private AutoTraderInfo CreateAutoTrader(
            Type strategy, string symbol, IOutput output)
        {
            var autoTrader = (IAutoTrader)Activator.CreateInstance(
                typeof(AutoTrader<>).MakeGenericType(strategy), symbol, output);

            return new AutoTraderInfo()
            {
                AutoTrader = autoTrader,
                OnOrderTriggered = (order) => OrderTriggered(autoTrader, order),
                OnOrderCanceled = (order) => OrderCanceled(autoTrader, order)
            };
        }
    }
}
