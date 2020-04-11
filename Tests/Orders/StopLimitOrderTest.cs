using FluentAssertions;
using MCRunner.Orders;
using PowerLanguage;
using System;
using Xunit;

namespace Tests.MCRunnerTests.Orders
{
    public class StopLimitOrderTest
    {
        [Fact]
        public void TestSend()
        {
            var order = new StopLimitOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(20, 19);
            monitoredOrder
                .Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 20)
                .WithArgs<OrderInfo>(info => info.Price == 19)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendWithUserDefinedSize()
        {
            var order = new StopLimitOrder(new SOrderParameters(
                Contracts.CreateUserSpecified(50), EOrderAction.Sell));

            using var monitoredOrder = order.Monitor();
            order.Send(15, 14);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 15)
                .WithArgs<OrderInfo>(info => info.Price == 14)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Sell)
                .WithArgs<OrderInfo>(info => info.Size == 50);
        }

        [Fact]
        public void TestSendIgnoreNumLots()
        {
            var order = new StopLimitOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(20, 19, 99);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 20)
                .WithArgs<OrderInfo>(info => info.Price == 19)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendCustomNumLots()
        {
            var order = new StopLimitOrder(
                new SOrderParameters(Contracts.CreateUserSpecified(100), EOrderAction.Buy));

            using var monitoredOrder = order.Monitor();
            order.Send(17, 18, 25);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 17)
                .WithArgs<OrderInfo>(info => info.Price == 18)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 25);
        }

        [Fact]
        public void TestSendNumLotsZeroDefault()
        {
            var order = new StopLimitOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(15, 15, 0);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendNumLotsDissallowNegative()
        {
            var order = new StopLimitOrder(new SOrderParameters());
            Assert.Throws<ArgumentOutOfRangeException>(() => order.Send(14, 14, -100));
        }
    }
}
