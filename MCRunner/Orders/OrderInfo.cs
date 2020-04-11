using PowerLanguage;

namespace MCRunner.Orders
{
    /// <summary>
    /// Represents information about an order.
    /// </summary>
    public struct OrderInfo
    {
        /// <summary>
        /// The price at which the order should trigger.
        /// </summary>
        public double ConditionPrice;

        /// <summary>
        /// The price at which the order should fill at (e.g. for limit orders)
        /// </summary>
        public double Price;

        /// <summary>
        /// The size of the order.
        /// </summary>
        public double Size;

        /// <summary>
        /// The action of the order.
        /// </summary>
        public EOrderAction OrderAction;

        /// <summary>
        /// A reference to the IOrderObject instance that triggered the order.
        /// </summary>
        public IOrderObject Order;

        /// <summary>
        /// The type of exit of the order (e.g. for stop loss or profit taker orders).
        /// </summary>
        public OrderExit OrderExit;
    }
}
