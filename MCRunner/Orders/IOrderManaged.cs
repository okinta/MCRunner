using System;

namespace MCRunner.Orders
{
    /// <summary>
    /// Represents an order that can be managed.
    /// </summary>
    public interface IOrderManaged
    {
        /// <summary>
        /// Triggered when an order is sent to the market.
        /// </summary>
        public event Action<OrderInfo> OrderSent;
    }
}
