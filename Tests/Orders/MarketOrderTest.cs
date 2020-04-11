using FluentAssertions;
using MCRunner.Orders;
using PowerLanguage;
using System;
using Xunit;

namespace Tests.MCRunnerTests.Orders
{
    public class MarketOrderTest
    {
        [Fact]
        public void TestSend()
        {
            var order = new MarketOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send();
            monitoredOrder
                .Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendIgnoreNumLots()
        {
            var order = new MarketOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(20);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendCustomNumLots()
        {
            var order = new MarketOrder(
                new SOrderParameters(Contracts.CreateUserSpecified(100), EOrderAction.Buy));

            using var monitoredOrder = order.Monitor();
            order.Send(25);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 25);
        }

        [Fact]
        public void TestSendNumLotsZeroDefault()
        {
            var order = new MarketOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(0);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendNumLotsDissallowNegative()
        {
            var order = new MarketOrder(new SOrderParameters());
            Assert.Throws<ArgumentOutOfRangeException>(() => order.Send(-100));
        }
    }
}
