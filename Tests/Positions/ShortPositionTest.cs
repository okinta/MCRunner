using FluentAssertions;
using MCRunner.Orders;
using MCRunner.Positions;
using PowerLanguage;
using Xunit;

namespace Tests.MCRunnerTests.Positions
{
    public class ShortPositionTest
    {
        [Fact]
        public void TestSizeNoPositions()
        {
            var position = new ShortPosition();
            Assert.Equal(0, position.Size);
        }

        [Fact]
        public void TestCantClosePositionBeforeOpening()
        {
            var position = new ShortPosition();
            var order = GetOrderInfo(new SOrderParameters(EOrderAction.BuyToCover));
            Assert.Throws<InvalidOrderException>(() => position.ValidateOrder(order));
        }

        [Fact]
        public void TestSize()
        {
            var position = new ShortPosition();
            position.UpdatePosition(
                GetOrderInfo(new SOrderParameters(EOrderAction.SellShort)));
            Assert.Equal(100, position.Size);
        }

        [Fact]
        public void TestClosePosition()
        {
            var position = new ShortPosition();
            position.UpdatePosition(
                GetOrderInfo(new SOrderParameters(EOrderAction.SellShort)));
            position.UpdatePosition(
                GetOrderInfo(new SOrderParameters(EOrderAction.BuyToCover)));

            Assert.Equal(0, position.Size);
        }

        private OrderInfo GetOrderInfo(SOrderParameters orderParams)
        {
            var order = new MarketOrder(orderParams);

            using var monitoredOrder = order.Monitor();
            order.Send();
            monitoredOrder.Should().Raise("OrderSent");
            return (OrderInfo)monitoredOrder.OccurredEvents[0].Parameters[0];
        }
    }
}
