using Bar = MCRunner.Instruments.Bar;
using MCRunner.Instruments;
using MCRunner.Strategy;
using Xunit;

namespace Tests.MCRunnerTests.Strategy
{
    public class StrategyRunnerTest
    {
        [Fact]
        public void TestCreate()
        {
            var runner = new StrategyRunner<MyStrategy>();
            Assert.False(runner.Strategy.CreateCalled);
            runner.Create();
            Assert.True(runner.Strategy.CreateCalled);
        }

        [Fact]
        public void TestStartCalc()
        {
            var runner = new StrategyRunner<MyStrategy>();
            Assert.False(runner.Strategy.StartCalcCalled);
            runner.StartCalc();
            Assert.True(runner.Strategy.StartCalcCalled);
        }

        [Fact]
        public void TestCalcBar()
        {
            var runner = new StrategyRunner<MyStrategy>();
            Assert.False(runner.Strategy.CalcBarCalled);
            runner.CalcBar();
            Assert.True(runner.Strategy.CalcBarCalled);
        }

        [Fact]
        public void TestStrategyReferencesBars()
        {
            var bars = new LoadableBars();
            var runner = new StrategyRunner<MyStrategy>(bars.ToSingleDataStream());

            bars.AddBar(new Bar() { High = 10 });
            Assert.Equal(10, runner.Strategy.CurrentHigh);

            bars.AddBar(new Bar() { High = 7 });
            Assert.Equal(7, runner.Strategy.CurrentHigh);

            bars.AddBar(new Bar() { High = 20 });
            Assert.Equal(20, runner.Strategy.CurrentHigh);
        }
    }
}
