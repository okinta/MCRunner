using MCRunner.Instruments;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.MCRunnerTests.Instruments
{
    public class BarsTest
    {
        [Fact]
        public void TestCurrentBarEmpty()
        {
            var bars = new Bars();
            Assert.Throws<IndexOutOfRangeException>(() => bars.CurrentBar);
        }

        [Fact]
        public void TestHighValueEmpty()
        {
            var bars = new Bars();
            Assert.Throws<IndexOutOfRangeException>(() => bars.HighValue);
        }

        [Fact]
        public void TestCurrentBar()
        {
            Assert.Equal(4, SampleBars.CurrentBar);
        }

        [Fact]
        public void TestHigh()
        {
            Assert.Equal(11, SampleBars.HighValue);
            Assert.Equal(11, SampleBars.High.Value);
        }

        private Bars SampleBars
        {
            get
            {
                return LoadBars(new List<Bar>()
                {
                    new Bar() { Open=4, Close=6,  High=7, Low=3 },
                    new Bar() { Open=5, Close=7,  High=8, Low=4 },
                    new Bar() { Open=6, Close=8,  High=9, Low=5 },
                    new Bar() { Open=7, Close=9,  High=10, Low=6 },
                    new Bar() { Open=8, Close=10, High=11, Low=7 },
                });
            }
        }

        private Bars LoadBars(List<Bar> bars)
        {
            var loadableBars = new LoadableBars();

            foreach (var bar in bars)
            {
                loadableBars.AddBar(bar);
            }

            return loadableBars;
        }
    }
}
