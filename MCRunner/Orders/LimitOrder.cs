using PowerLanguage;
using System;

namespace MCRunner.Orders
{
    /// <summary>
    /// Describes a limit order.
    /// </summary>
    public class LimitOrder : BaseOrder, IOrderPriced
    {
        /// <summary>
        /// Instantiates the order.
        /// </summary>
        /// <param name="orderParams">Parameters for the order.</param>
        /// <param name="openNext">Indicates whether the order must be placed at the open
        /// of next bar.</param>
        public LimitOrder(SOrderParameters orderParams, bool openNext = false)
            : base(orderParams, OrderCategory.Limit, openNext)
        {
        }

        /// <summary>
        /// Sends the order to the market.
        /// </summary>
        /// <param name="price">The limit price.</param>
        public void Send(double price)
        {
            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException("price must be larger than 0");
            }

            TriggerOrderSent(new OrderInfo()
            {
                Order = this,
                OrderAction = OrderParams.Action,
                OrderExit = OrderParams.ExitTypeInfo,
                Price = price,
                Size = OrderParams.Lots.GetSize()
            });
        }

        /// <summary>
        /// Sends the order to the market.
        /// </summary>
        /// <param name="price">The limit price.</param>
        /// <param name="numLots">Number of Lots.</param>
        public void Send(double price, int numLots)
        {
            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException("price must be larger than 0");
            }

            if (numLots < 0)
            {
                throw new ArgumentOutOfRangeException("numLots must be larger than 0");
            }
            else if (numLots == 0)
            {
                Send(price);
            }
            else
            {
                TriggerOrderSent(new OrderInfo()
                {
                    Order = this,
                    OrderAction = OrderParams.Action,
                    OrderExit = OrderParams.ExitTypeInfo,
                    Price = price,
                    Size = OrderParams.Lots.GetSize(numLots)
                });
            }
        }

        public void Send(string new_name, double price)
        {
            throw new NotImplementedException();
        }

        public void Send(string new_name, double price, int numLots)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(double price, string fromName)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(double price, int numLots, string fromName)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(string new_name, double price, string fromName)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(string new_name, double price, int numLots, string fromName)
        {
            throw new NotImplementedException();
        }
    }
}
