using MCRunner.Orders;
using System;

namespace MCRunner.Positions
{
    /// <summary>
    /// The exception that is thrown when an order is invalid.
    /// </summary>
    public class InvalidOrderException : SystemException
    {
        public InvalidOrderException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Interface to represent a position.
    /// </summary>
    public interface IPosition
    {
        /// <summary>
        /// Retrieves the size of the position.
        /// </summary>
        public double Size { get; }

        /// <summary>
        /// Validates the given order. Raises InvalidOrderException if invalid.
        /// </summary>
        /// <param name="order">The order to validate.</param>
        public void ValidateOrder(OrderInfo order);

        /// <summary>
        /// Updates the position with the given order.
        /// </summary>
        /// <param name="order">The order that was filled.</param>
        public void UpdatePosition(OrderInfo order);
    }
}
