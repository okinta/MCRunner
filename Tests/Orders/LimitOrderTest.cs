using FluentAssertions;
using MCRunner.Orders;
using PowerLanguage;
using System;
using Xunit;

namespace Tests.MCRunnerTests.Orders
{
    public class LimitOrderTest
    {
        [Fact]
        public void TestSend()
        {
            var order = new LimitOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(20);
            monitoredOrder
                .Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Price == 20)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendWithUserDefinedSize()
        {
            var order = new LimitOrder(new SOrderParameters(
                Contracts.CreateUserSpecified(50), EOrderAction.Sell));

            using var monitoredOrder = order.Monitor();
            order.Send(15);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Price == 15)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Sell)
                .WithArgs<OrderInfo>(info => info.Size == 50);
        }

        [Fact]
        public void TestSendIgnoreNumLots()
        {
            var order = new LimitOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(20);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Price == 20)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendCustomNumLots()
        {
            var order = new LimitOrder(
                new SOrderParameters(Contracts.CreateUserSpecified(100), EOrderAction.Buy));

            using var monitoredOrder = order.Monitor();
            order.Send(17, 25);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Price == 17)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 25);
        }

        [Fact]
        public void TestSendNumLotsZeroDefault()
        {
            var order = new LimitOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(15, 0);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendNumLotsDissallowNegative()
        {
            var order = new LimitOrder(new SOrderParameters());
            Assert.Throws<ArgumentOutOfRangeException>(() => order.Send(14, -100));
        }
    }
}
