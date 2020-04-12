using MCRunner.Orders;
using PowerLanguage;
using System.Collections.Immutable;
using System.Linq;
using System;

namespace MCRunner.Positions
{
    /// <summary>
    /// Describes a short position.
    /// </summary>
    public class ShortPosition : IPosition
    {
        /// <summary>
        /// Retrieves the size of the short position.
        /// </summary>
        public double Size
        {
            get
            {
                return positions.Sum(position => position.Size);
            }
        }

        private ImmutableList<PositionInfo> positions = ImmutableList<PositionInfo>.Empty;

        /// <summary>
        /// Validates the given order. Raises InvalidOrderException if invalid.
        /// </summary>
        /// <param name="order">The order to validate.</param>
        public void ValidateOrder(OrderInfo order)
        {
            switch (order.OrderAction)
            {
                case EOrderAction.SellShort:
                    break;

                case EOrderAction.BuyToCover:
                    if (positions.Count == 0)
                    {
                        throw new InvalidOrderException(
                            "Can't close position before opening one!");
                    }

                    if (order.OrderExit.ExitType != OrderExit.EExitType.All)
                    {
                        throw new InvalidOrderException(
                            "Only OrderExit.EExitType.All is currently supported");
                    }
                    break;

                default:
                    throw new InvalidOrderException("Short orders only!");
            }
        }

        /// <summary>
        /// Updates the position with the given order.
        /// </summary>
        /// <param name="order">The order that was filled.</param>
        public void UpdatePosition(OrderInfo order)
        {
            if (order.OrderAction == EOrderAction.SellShort)
            {
                OpenPosition(order);
            }
            else if (order.OrderAction == EOrderAction.BuyToCover)
            {
                ClosePosition(order);
            }
            else
            {
                throw new ArgumentException(
                    string.Format("Unsupported OrderAction {0}", order.OrderAction));
            }
        }

        /// <summary>
        /// Opens a new position.
        /// </summary>
        /// <param name="order">Information about the order.</param>
        private void OpenPosition(OrderInfo order)
        {
            positions = positions.Add(new PositionInfo() { Size = order.Size });
        }

        /// <summary>
        /// Closes an existing position.
        /// </summary>
        /// <param name="order">Information about the order.</param>
        private void ClosePosition(OrderInfo order)
        {
            if (positions.Count == 0)
            {
                throw new InvalidOperationException(
                    "Can't close position before opening one!");
            }

            if (order.OrderExit.ExitType == OrderExit.EExitType.All)
            {
                positions = positions.Clear();
            }
            else
            {
                throw new NotImplementedException(
                    "Other OrderExit types not currently supported");
            }
        }
    }
}
