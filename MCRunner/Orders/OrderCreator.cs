using PowerLanguage;
using System;

namespace MCRunner.Orders
{
    /// <summary>
    /// Implements an interface to create various types of orders that can be submitted to
    /// a broker.
    /// </summary>
    public class OrderCreator : IOrderCreator, IOrderManaged
    {
        /// <summary>
        /// Triggered when an order is sent.
        /// </summary>
        public event Action<OrderInfo> OrderSent;

        /// <summary>
        /// Function for Limit-order creation.
        /// </summary>
        /// <param name="orderParams">Parameters for creating limit order.</param>
        /// <returns>An instance of the interface describing limit order.</returns>
        public IOrderPriced Limit(SOrderParameters orderParams)
        {
            var order = new LimitOrder(orderParams, true);
            order.OrderSent += OrderSent;
            return order;
        }

        /// <summary>
        /// Function for market order creation on the next bar.
        /// </summary>
        /// <param name="orderParams">Parameters for creating market order.</param>
        /// <returns>An instance of the interface describing market order.</returns>
        public IOrderMarket MarketNextBar(SOrderParameters orderParams)
        {
            var order = new MarketOrder(orderParams, true);
            order.OrderSent += OrderSent;
            return order;
        }

        /// <summary>
        /// Function for market order creation on the current bar.
        /// </summary>
        /// <param name="orderParams">Parameters for creating market order.</param>
        /// <returns>An instance of the interface describing market order.</returns>
        public IOrderMarket MarketThisBar(SOrderParameters orderParams)
        {
            var order = new MarketOrder(orderParams, false);
            order.OrderSent += OrderSent;
            return order;
        }

        /// <summary>
        /// Function for Stop-order creation.
        /// </summary>
        /// <param name="orderParams">Parameters for creating stop order.</param>
        /// <returns>An instance of the interface describing stop order.</returns>
        public IOrderPriced Stop(SOrderParameters orderParams)
        {
            var order = new StopOrder(orderParams, true);
            order.OrderSent += OrderSent;
            return order;
        }

        /// <summary>
        /// Function for StopLimit-order creation.
        /// </summary>
        /// <param name="orderParams">Parameters for creating stop-limit order.</param>
        /// <returns>An instance of the interface describing stop-limit order.</returns>
        public IOrderStopLimit StopLimit(SOrderParameters orderParams)
        {
            var order = new StopLimitOrder(orderParams, true);
            order.OrderSent += OnOrderSent;
            return order;
        }

        /// <summary>
        /// Called when an order has been sent. Triggers the OrderSent event.
        /// </summary>
        /// <param name="order">The order information that was sent.</param>
        private void OnOrderSent(OrderInfo order)
        {
            OrderSent.SafeTrigger(order);
        }
    }
}
