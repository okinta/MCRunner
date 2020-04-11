using PowerLanguage;
using System;

namespace MCRunner.Orders
{
    /// <summary>
    /// Describes a market order.
    /// </summary>
    public class MarketOrder : BaseOrder, IOrderMarket
    {
        /// <summary>
        /// Instantiates the order.
        /// </summary>
        /// <param name="orderParams">Parameters for the order.</param>
        /// <param name="openNext">Indicates whether the order must be placed at the open
        /// of next bar.</param>
        public MarketOrder(SOrderParameters orderParams, bool openNext = false)
            : base(orderParams, OrderCategory.Market, openNext)
        {
        }

        /// <summary>
        /// Sends the order to the market.
        /// </summary>
        public void Send()
        {
            TriggerOrderSent(new OrderInfo()
            {
                Order = this,
                OrderAction = OrderParams.Action,
                OrderExit = OrderParams.ExitTypeInfo,
                Size = OrderParams.Lots.GetSize()
            });
        }

        /// <summary>
        /// Sends the order to the market.
        /// </summary>
        /// <param name="numLots">Number of lots.</param>
        public void Send(int numLots)
        {
            if (numLots < 0)
            {
                throw new ArgumentOutOfRangeException("numLots must be larger than 0");
            }
            else if (numLots == 0)
            {
                Send();
            }
            else
            {
                TriggerOrderSent(new OrderInfo()
                {
                    Order = this,
                    OrderAction = OrderParams.Action,
                    OrderExit = OrderParams.ExitTypeInfo,
                    Size = OrderParams.Lots.GetSize(numLots)
                });
            }
        }

        public void Send(string new_name)
        {
            throw new NotImplementedException();
        }

        public void Send(string new_name, int numLots)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(string fromName)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(int numLots, string fromName)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(string new_name, string fromName)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(string new_name, int numLots, string fromName)
        {
            throw new NotImplementedException();
        }
    }
}
