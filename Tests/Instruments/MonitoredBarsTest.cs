using FluentAssertions;
using MCRunner.Instruments;
using Xunit;

namespace Tests.MCRunnerTests.Instruments
{
    public class MonitoredBarsTest
    {
        [Fact]
        public void TestAddTriggersUpdatedEvent()
        {
            var bars = new MonitoredBars();

            using var monitoredBars = bars.Monitor();

            bars.AddBar(new Bar() { });
            monitoredBars.Should().Raise("Updated");
        }
    }
}
