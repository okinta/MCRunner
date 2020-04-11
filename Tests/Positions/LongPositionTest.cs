using FluentAssertions;
using MCRunner.Orders;
using MCRunner.Positions;
using PowerLanguage;
using Xunit;

namespace Tests.MCRunnerTests.Positions
{
    public class LongPositionTest
    {
        [Fact]
        public void TestSizeNoPositions()
        {
            var position = new LongPosition();
            Assert.Equal(0, position.Size);
        }

        [Fact]
        public void TestCantClosePositionBeforeOpening()
        {
            var position = new LongPosition();
            var order = GetOrderInfo(new SOrderParameters(EOrderAction.Sell));
            Assert.Throws<InvalidOrderException>(() => position.ValidateOrder(order));
        }

        [Fact]
        public void TestSize()
        {
            var position = new LongPosition();
            position.UpdatePosition(GetOrderInfo(new SOrderParameters(EOrderAction.Buy)));
            Assert.Equal(100, position.Size);
        }

        [Fact]
        public void TestClosePosition()
        {
            var position = new LongPosition();
            position.UpdatePosition(GetOrderInfo(new SOrderParameters(EOrderAction.Buy)));
            position.UpdatePosition(GetOrderInfo(new SOrderParameters(EOrderAction.Sell)));

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
