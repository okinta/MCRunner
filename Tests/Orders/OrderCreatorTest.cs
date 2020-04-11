using FluentAssertions;
using OrderCreator = MCRunner.Orders.OrderCreator;
using PowerLanguage;
using Xunit;
using MCRunner.Orders;

namespace Tests.MCRunnerTests.Orders
{
    public class OrderCreatorTest
    {
        [Fact]
        public void TestLimit()
        {
            var orderCreator = new OrderCreator();

            using var monitoredOrderCreator = orderCreator.Monitor();
            var order = orderCreator.Limit(new SOrderParameters(EOrderAction.Buy));
            order.Send(15);
            monitoredOrderCreator
                .Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Order == order)
                .WithArgs<OrderInfo>(info => info.Price == 15)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestMarketNextBar()
        {
            var orderCreator = new OrderCreator();

            using var monitoredOrderCreator = orderCreator.Monitor();
            var order = orderCreator.MarketNextBar(new SOrderParameters(EOrderAction.Sell));
            order.Send();
            monitoredOrderCreator
                .Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Order == order)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Sell)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestMarketThisBar()
        {
            var orderCreator = new OrderCreator();

            using var monitoredOrderCreator = orderCreator.Monitor();
            var order = orderCreator.MarketThisBar(
                new SOrderParameters(Contracts.CreateUserSpecified(10), EOrderAction.Buy));
            order.Send(99);
            monitoredOrderCreator
                .Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Order == order)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 99);
        }

        [Fact]
        public void TestStop()
        {
            var orderCreator = new OrderCreator();

            using var monitoredOrderCreator = orderCreator.Monitor();
            var order = orderCreator.Stop(new SOrderParameters(EOrderAction.Sell));
            order.Send(15);
            monitoredOrderCreator
                .Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Order == order)
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 15)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Sell)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }

        [Fact]
        public void TestStopLimit()
        {
            var orderCreator = new OrderCreator();

            using var monitoredOrderCreator = orderCreator.Monitor();
            var order = orderCreator.StopLimit(new SOrderParameters(EOrderAction.Buy));
            order.Send(15, 14);
            monitoredOrderCreator
                .Should().Raise("OrderSent")
                .WithArgs<OrderInfo>(info => info.Order == order)
                .WithArgs<OrderInfo>(info => info.ConditionPrice == 15)
                .WithArgs<OrderInfo>(info => info.Price == 14)
                .WithArgs<OrderInfo>(info => info.OrderAction == EOrderAction.Buy)
                .WithArgs<OrderInfo>(info => info.Size == 100);
        }
    }
}
