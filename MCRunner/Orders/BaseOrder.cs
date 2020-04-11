using PowerLanguage;
using System;

namespace MCRunner.Orders
{
    /// <summary>
    /// Describes a base order.
    /// </summary>
    public abstract class BaseOrder : IOrderObject, IOrderManaged
    {
        /// <summary>
        /// Triggered when an order is sent to the market.
        /// </summary>
        public event Action<OrderInfo> OrderSent;

        /// <summary>
        /// The parameters for the Order.
        /// </summary>
        public SOrderParameters OrderParams { get; private set; }

        /// <summary>
        /// Information about the order.
        /// </summary>
        public Order Info { get; private set; }

        /// <summary>
        /// The ID of the order.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Instantiates the order.
        /// </summary>
        /// <param name="orderParams">Parameters for the order.</param>
        /// <param name="orderCategory">Order category.</param>
        /// <param name="openNext">Indicates whether the order must be placed at the open
        /// of next bar.</param>
        public BaseOrder(
            SOrderParameters orderParams, OrderCategory orderCategory, bool openNext)
        {
            OrderParams = orderParams;
            Info = new Order(
                orderParams.Name,
                orderParams.Action,
                orderCategory,
                orderParams.Lots,
                openNext,
                orderParams.ExitTypeInfo);
            ID = GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current order.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object;
        /// otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            var equals = false;

            if (obj is object && obj.GetType() == GetType())
            {
                var order = (BaseOrder)obj;
                if (order.OrderParams.Name == OrderParams.Name
                    && order.OrderParams.Action == OrderParams.Action
                    && order.OrderParams.ExitTypeInfo == OrderParams.ExitTypeInfo
                    && order.OrderParams.Lots.Contract == OrderParams.Lots.Contract
                    && order.OrderParams.Lots.Type == OrderParams.Lots.Type
                    && order.Info.Category == Info.Category
                    && order.Info.OnClose == Info.OnClose)
                {
                    equals = true;
                }
            }

            return equals;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current order.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(OrderParams, Info.Category, Info.OnClose);
        }

        /// <summary>
        /// Triggers the OrderSent event.
        /// </summary>
        /// <param name="orderInfo">The information about the order.</param>
        protected virtual void TriggerOrderSent(OrderInfo orderInfo)
        {
            OrderSent.SafeTrigger(orderInfo);
        }
    }
}
