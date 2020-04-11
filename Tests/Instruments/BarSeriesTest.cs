using MCRunner.Instruments;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests.MCRunnerTests.Instruments
{
    public class BarSeriesTest
    {
        [Fact]
        public void TestInstantiateNotNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BarSeries<double>(null, "High"));
        }

        [Fact]
        public void TestBarFieldMustExist()
        {
            var bars = new List<Bar>();
            Assert.Throws<ArgumentException>(
                () => new BarSeries<double>(bars, "InvalidName"));
        }

        [Fact]
        public void TestBarFieldTypeMatch()
        {
            var bars = new List<Bar>();
            Assert.Throws<ArgumentException>(
                () => new BarSeries<DateTime>(bars, "High"));
        }

        [Fact]
        public void TestReferenceFuture()
        {
            var bars = new List<Bar>();
            var series = new BarSeries<double>(bars, "High");
            Assert.Throws<IndexOutOfRangeException>(() => series[-1]);
        }

        [Fact]
        public void TestEmptyValue()
        {
            var bars = new List<Bar>();
            var series = new BarSeries<double>(bars, "High");
            Assert.Throws<IndexOutOfRangeException>(() => series.Value);
        }

        [Fact]
        public void TestValue()
        {
            var bars = new List<Bar>() { new Bar() { Low = 5 } };
            var series = new BarSeries<double>(bars, "Low");
            Assert.Equal(5, series.Value);
        }

        [Fact]
        public void TestMultipleValue()
        {
            var bars = new List<Bar>();
            var series = new BarSeries<double>(bars, "Low");

            for (var i = 0; i < 10; ++i)
            {
                bars.Add(new Bar() { Low = i });
                Assert.Equal(i, series.Value);
            }
        }

        [Fact]
        public void TestReference()
        {
            var bars = new List<Bar>() { new Bar() { Close = 8 } };
            var series = new BarSeries<double>(bars, "Close");
            Assert.Equal(8, series[0]);
        }

        [Fact]
        public void TestMultipleReference()
        {
            var bars = new List<Bar>();
            var series = new BarSeries<double>(bars, "Low");

            for (var i = 0; i < 10; ++i)
            {
                bars.Add(new Bar() { Low = i });
                Assert.Equal(i, series[0]);
            }
        }

        [Fact]
        public void TestReferencePast()
        {
            var bars = new List<Bar>() {
                new Bar() { Low = 5 },
                new Bar() { Low = 7 },
                new Bar() { Low = 3 }
            };
            var series = new BarSeries<double>(bars, "Low");

            Assert.Equal(3, series[0]);
            Assert.Equal(7, series[1]);
            Assert.Equal(5, series[2]);
        }

        [Fact]
        public void TestFutureTooFarPast()
        {
            var bars = new List<Bar>() { new Bar() { Close = 8 } };
            var series = new BarSeries<double>(bars, "Close");
            Assert.Throws<IndexOutOfRangeException>(() => series[1]);
        }
    }
}
