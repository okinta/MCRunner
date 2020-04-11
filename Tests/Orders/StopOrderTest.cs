using FluentAssertions;
using MCRunner.Orders;
using PowerLanguage;
using System;
using Xunit;

namespace Tests.MCRunnerTests.Orders
{
    public class StopOrderTest
    {
        [Fact]
        public void TestSend()
        {
            var order = new StopOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(15);
            monitoredOrder
                .Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 15)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendIgnoreNumLots()
        {
            var order = new StopOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(15, 20);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 15)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendCustomNumLots()
        {
            var order = new StopOrder(
                new SOrderParameters(Contracts.CreateUserSpecified(100), EOrderAction.Buy));

            using var monitoredOrder = order.Monitor();
            order.Send(14, 25);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 14)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 25);
        }

        [Fact]
        public void TestSendNumLotsZeroDefault()
        {
            var order = new StopOrder(new SOrderParameters());

            using var monitoredOrder = order.Monitor();
            order.Send(13, 0);
            monitoredOrder.Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 13)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestSendNumLotsDissallowNegative()
        {
            var order = new StopOrder(new SOrderParameters());
            Assert.Throws<ArgumentOutOfRangeException>(() => order.Send(20, -100));
        }
    }
}
