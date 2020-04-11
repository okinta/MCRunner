using Bar = MCRunner.Instruments.Bar;
using FluentAssertions;
using MCRunner.Orders;
using MCRunner.Strategy;
using OrderCreator = MCRunner.Orders.OrderCreator;
using PowerLanguage;
using Xunit;

namespace Tests.MCRunnerTests.Strategy
{
    public class StrategyManagerTest
    {
        [Fact]
        public void TestNoPositions()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);
            Assert.Equal(0, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestUpdateMarketPosition()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            var order = orderCreator.MarketThisBar(
                new SOrderParameters(EOrderAction.Buy));
            order.Send();

            Assert.Equal(100, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestUpdateMarketPositionMultipleOrders()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            orderCreator.MarketThisBar(
                new SOrderParameters(EOrderAction.Buy)).Send();
            orderCreator.MarketThisBar(
                new SOrderParameters(EOrderAction.Buy)).Send();
            orderCreator.MarketThisBar(
                new SOrderParameters(
                    Contracts.Default, EOrderAction.Sell, OrderExit.FromAll)).Send();

            Assert.Equal(0, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestLimitOrderNotTriggeredImmediately()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            orderCreator.Limit(new SOrderParameters(EOrderAction.Buy)).Send(40);

            Assert.Equal(0, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestLimitBuyOrderNotTriggered()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            orderCreator.Limit(new SOrderParameters(EOrderAction.Buy)).Send(40);
            manager.TriggerOrders(new Bar() { Low = 50 });

            Assert.Equal(0, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestLimitBuyOrderTriggered()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            orderCreator.Limit(new SOrderParameters(EOrderAction.Buy)).Send(40);
            manager.TriggerOrders(new Bar() { Low = 39 });

            Assert.Equal(100, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestLimitBuyOrderTriggeredOnce()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            orderCreator.Limit(new SOrderParameters(EOrderAction.Buy)).Send(40);
            manager.TriggerOrders(new Bar() { Low = 39 });
            manager.TriggerOrders(new Bar() { Low = 39 });

            Assert.Equal(100, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestLimitBuyOrderCanceledWhenNotTriggered()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            orderCreator.Limit(new SOrderParameters(EOrderAction.Buy)).Send(40);
            manager.TriggerOrders(new Bar() { Low = 41 });
            manager.TriggerOrders(new Bar() { Low = 39 });

            Assert.Equal(0, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestLimitSellOrderNotTriggered()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            orderCreator.Limit(new SOrderParameters(EOrderAction.SellShort)).Send(40);
            manager.TriggerOrders(new Bar() { High = 39 });

            Assert.Equal(0, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestLimitSellOrderTriggered()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            orderCreator.Limit(new SOrderParameters(EOrderAction.SellShort)).Send(40);
            manager.TriggerOrders(new Bar() { High = 41 });

            Assert.Equal(-100, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestOrderValidatedTriggered()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            using var monitoredManager = manager.Monitor();
            orderCreator.Limit(new SOrderParameters()).Send(20);

            monitoredManager.Should()
                .Raise("OrderValidated")
                .WithArgs<OrderInfo>(info => info.Price == 20);
        }

        [Fact]
        public void TestOrderValidatedTriggeredOnlyOnce()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            using var monitoredManager = manager.Monitor();
            orderCreator.Limit(new SOrderParameters()).Send(20);
            orderCreator.Limit(new SOrderParameters()).Send(20);

            monitoredManager.Should()
                .Raise("OrderValidated")
                .WithArgs<OrderInfo>(info => info.Price == 20)
                .OnlyOnce();
        }

        [Fact]
        public void TestOrderValidatedTriggeredAgain()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            using var monitoredManager = manager.Monitor();
            orderCreator.Limit(new SOrderParameters()).Send(20);
            manager.TriggerOrders(new Bar() { Low = 19 });

            Assert.Equal(100, manager.StrategyInfo.MarketPosition);
            monitoredManager.Should()
                .Raise("OrderValidated")
                .WithArgs<OrderInfo>(info => info.Price == 20)
                .OnlyOnce()
                .Reset();

            // The same order is allowed to be sent again after being filled
            orderCreator.Limit(new SOrderParameters()).Send(20);
            monitoredManager.Should()
                .Raise("OrderValidated")
                .WithArgs<OrderInfo>(info => info.Price == 20)
                .OnlyOnce();
        }

        [Fact]
        public void TestOrderCanceledTriggered()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            using var monitoredManager = manager.Monitor();
            orderCreator.Limit(new SOrderParameters()).Send(20);
            manager.TriggerOrders(new Bar() { Low = 21 });
            manager.TriggerOrders(new Bar() { Low = 19 });

            monitoredManager.Should()
                .Raise("OrderValidated")
                .WithArgs<OrderInfo>(info => info.Price == 20)
                .OnlyOnce();
            monitoredManager.Should().Raise("OrderCanceled");
            Assert.Equal(0, manager.StrategyInfo.MarketPosition);
        }

        [Fact]
        public void TestOrderCanceledTriggeredAfterNotSendingOrder()
        {
            var orderCreator = new OrderCreator();
            var manager = new StrategyManager(orderCreator);

            using var monitoredManager = manager.Monitor();
            orderCreator.Limit(new SOrderParameters()).Send(20);
            manager.TriggerOrders(new Bar() { Low = 21 });
            orderCreator.Limit(new SOrderParameters()).Send(20);
            manager.TriggerOrders(new Bar() { Low = 22 });

            monitoredManager.Should()
                .Raise("OrderValidated")
                .WithArgs<OrderInfo>(info => info.Price == 20)
                .OnlyOnce();
            monitoredManager.Should().NotRaise("OrderCanceled");
            Assert.Equal(0, manager.StrategyInfo.MarketPosition);

            // If we don't send the order again, it should be canceled
            manager.TriggerOrders(new Bar() { Low = 23 });
            monitoredManager.Should().Raise("OrderCanceled");
            Assert.Equal(0, manager.StrategyInfo.MarketPosition);
        }
    }
}
