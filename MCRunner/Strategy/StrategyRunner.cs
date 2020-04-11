using OrderCreator = MCRunner.Orders.OrderCreator;
using PowerLanguage.Strategy;
using PowerLanguage;
using System.Reflection;
using System;
using System.Collections.Immutable;

namespace MCRunner.Strategy
{
    /// <summary>
    /// Helper to run strategies
    /// </summary>
    public class StrategyRunner<T> where T : SignalObject
    {
        /// <summary>
        /// The strategy this runner runs.
        /// </summary>
        public T Strategy { get; private set; }

        private readonly Type type = typeof(T);

        /// <summary>
        /// Instantiates a new runner.
        /// </summary>
        /// <param name="barsData">Optional IList<IInstrument> instance to use. If not
        /// provided then the StrategyRunner will be loaded with an empty chart.</param>
        /// <param name="orderCreator">Optional IOrderCreator instance to use. If not
        /// provided a new OrderCreator instance will be created.</param>
        /// <param name="output">Optional IOutput instance to use. If not provided a new
        /// ConsoleOutput instance will be created.</param>
        /// <param name="strategyInfo">Optional IStrategyPerformance instance to use. If
        /// not provided a new StrategyPerformance instance will be created.</param>
        public StrategyRunner(
            ImmutableList<IInstrument> barsData = null,
            IOrderCreator orderCreator = null,
            IOutput output = null,
            IStrategyPerformance strategyInfo = null)
        {
            Strategy = (T)Activator.CreateInstance(type, "");

            if (barsData is null)
            {
                barsData = ImmutableList<IInstrument>.Empty;
            }

            if (orderCreator is null)
            {
                orderCreator = new OrderCreator();
            }

            if (output is null)
            {
                output = new ConsoleOutput();
            }

            if (strategyInfo is null)
            {
                strategyInfo = new StrategyPerformance();
            }

            SetProtectedProperty("BarsData", barsData);
            SetProtectedProperty("OrderCreator", orderCreator);
            SetProtectedProperty("Output", output);
            SetProtectedProperty("StrategyInfo", strategyInfo);
        }

        /// <summary>
        /// Calls the strategy's Create() method.
        /// </summary>
        public void Create()
        {
            CallProtectedMethod("Create");
        }

        /// <summary>
        /// Calls the strategy's StartCal() method.
        /// </summary>
        public void StartCalc()
        {
            CallProtectedMethod("StartCalc");
        }

        /// <summary>
        /// Calls the strategy's CalcBar() method.
        /// </summary>
        public void CalcBar()
        {
            CallProtectedMethod("CalcBar");
        }

        /// <summary>
        /// Calls a protected method of the strategy.
        /// </summary>
        /// <param name="name">The name of the method to call.</param>
        protected void CallProtectedMethod(string name)
        {
            MethodInfo method = Strategy.GetType().GetMethod(
                name, BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(Strategy, null);
        }

        /// <summary>
        /// Sets a protected property of the strategy.
        /// </summary>
        /// <param name="name">The name of the property to set.</param>
        /// <param name="value">The value of the property to set.</param>
        protected void SetProtectedProperty(string name, object value)
        {
            PropertyInfo property = Strategy.GetType().GetProperty(
                name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            property.SetValue(Strategy, value);
        }
    }
}
