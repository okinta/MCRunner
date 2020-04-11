using PowerLanguage;
using System;

namespace MCRunner.Orders
{
    /// <summary>
    /// Describes a stop-limit order.
    /// </summary>
    public class StopLimitOrder : BaseOrder, IOrderStopLimit
    {
        /// <summary>
        /// Instantiates the order.
        /// </summary>
        /// <param name="orderParams">Parameters for the order.</param>
        /// <param name="openNext">Indicates whether the order must be placed at the open
        /// of next bar.</param>
        public StopLimitOrder(SOrderParameters orderParams, bool openNext = false)
            : base(orderParams, OrderCategory.StopLimit, openNext)
        {
        }

        /// <summary>
        /// Sends the order to the market.
        /// </summary>
        /// <param name="stopPrice">Order stop condition price.</param>
        /// <param name="limitPrice">Order Limit price.</param>
        public void Send(double stopPrice, double limitPrice)
        {
            if (stopPrice <= 0)
            {
                throw new ArgumentOutOfRangeException("stopPrice must be larger than 0");
            }

            if (limitPrice <= 0)
            {
                throw new ArgumentOutOfRangeException("limitPrice must be larger than 0");
            }

            TriggerOrderSent(new OrderInfo()
            {
                ConditionPrice = stopPrice,
                Order = this,
                OrderAction = OrderParams.Action,
                OrderExit = OrderParams.ExitTypeInfo,
                Price = limitPrice,
                Size = OrderParams.Lots.GetSize()
            });
        }

        /// <summary>
        /// Sends the order to the market.
        /// </summary>
        /// <param name="stopPrice">Order stop condition price.</param>
        /// <param name="limitPrice">Order Limit price.</param>
        /// <param name="numLots">Number of Lots.</param>
        public void Send(double stopPrice, double limitPrice, int numLots)
        {
            if (stopPrice <= 0)
            {
                throw new ArgumentOutOfRangeException("stopPrice must be larger than 0");
            }

            if (limitPrice <= 0)
            {
                throw new ArgumentOutOfRangeException("limitPrice must be larger than 0");
            }

            if (numLots < 0)
            {
                throw new ArgumentOutOfRangeException("numLots must be larger than 0");
            }
            else if (numLots == 0)
            {
                Send(stopPrice, limitPrice);
            }
            else
            {
                TriggerOrderSent(new OrderInfo()
                {
                    ConditionPrice = stopPrice,
                    Order = this,
                    OrderAction = OrderParams.Action,
                    OrderExit = OrderParams.ExitTypeInfo,
                    Price = limitPrice,
                    Size = OrderParams.Lots.GetSize(numLots)
                });
            }
        }

        public void Send(string new_name, double stopPrice, double limitPrice)
        {
            throw new NotImplementedException();
        }

        public void Send(string new_name, double stopPrice, double limitPrice, int numLots)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(double stopPrice, double limitPrice, string fromName)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(double stopPrice, double limitPrice, int numLots, string fromName)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(string new_name, double stopPrice, double limitPrice, string fromName)
        {
            throw new NotImplementedException();
        }

        public void SendFromEntry(string new_name, double stopPrice, double limitPrice, int numLots, string fromName)
        {
            throw new NotImplementedException();
        }
    }
}
