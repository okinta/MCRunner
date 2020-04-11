using MCRunner.Instruments;
using Xunit;

namespace Tests.MCRunnerTests.Instruments
{
    public class LoadableBarsTest
    {
        [Fact]
        public void TestAddBarLowValue()
        {
            var bars = new LoadableBars();
            bars.AddBar(new Bar() { Low = 4 });
            Assert.Equal(4, bars.LowValue);
        }
    }
}
