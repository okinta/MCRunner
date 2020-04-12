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
    internal enum AutoTraderState
    {
        Running,
        NotRunning
    }

    internal struct AutoTraderInfo
    {
        public Action<OrderInfo> OnOrderCanceled;
        public Action<OrderInfo> OnOrderTriggered;
        public AutoTraderState State;
        public IAutoTrader AutoTrader;
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

        private IReadOnlyDictionary<string, AutoTraderInfo> AutoTraders { get; }

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

            var autoTraders = new Dictionary<string, AutoTraderInfo>();
            foreach (var strategy in strategies)
            {
                foreach (var symbol in symbols)
                {
                    if (!AutoTraders.ContainsKey(symbol))
                    {
                        autoTraders[symbol] = CreateAutoTrader(strategy, symbol, output);
                    }
                }
            }

            AutoTraders = autoTraders;

            if (AutoTraders.Count == 0)
            {
                throw new ArgumentException(
                    "There must be at least one symbol to trade", "symbols");
            }
        }

        /// <summary>
        /// Subscribes to the given chart and starts trading.
        /// </summary>
        /// <param name="symbol">The symbol the chart is for.</param>
        /// <param name="bars">The chart to subscribe to.</param>
        public void Start(string symbol, ImmutableList<IMonitoredInstrument> bars)
        {
            var trader = AutoTraders[symbol];
            if (trader.State == AutoTraderState.Running)
            {
                throw new InvalidOperationException(string.Format(
                    "AutoTradingManager for {0} is already running", symbol));
            }

            trader.AutoTrader.OrderTriggered += trader.OnOrderTriggered;
            trader.AutoTrader.OrderCanceled += trader.OnOrderCanceled;
            trader.AutoTrader.Start(bars);
            trader.State = AutoTraderState.Running;
        }

        /// <summary>
        /// Stops trading a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to stop trading.</param>
        public void Stop(string symbol)
        {
            var trader = AutoTraders[symbol];
            if (trader.State == AutoTraderState.NotRunning)
            {
                throw new InvalidOperationException(string.Format(
                    "AutoTradingManager for {0} isn't running", symbol));
            }

            trader.AutoTrader.OrderTriggered -= trader.OnOrderTriggered;
            trader.AutoTrader.OrderCanceled -= trader.OnOrderCanceled;
            trader.AutoTrader.Stop();
            trader.State = AutoTraderState.NotRunning;
        }

        /// <summary>
        /// Stops trading all symbols.
        /// </summary>
        public void StopAll()
        {
            foreach (var symbol in AutoTraders.Keys)
            {
                Stop(symbol);
            }
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
                OnOrderCanceled = (order) => OrderCanceled(autoTrader, order),
                OnOrderTriggered = (order) => OrderTriggered(autoTrader, order),
                State = AutoTraderState.NotRunning
            };
        }
    }
}
