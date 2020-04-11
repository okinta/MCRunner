using Bar = MCRunner.Instruments.Bar;
using MCRunner.Orders;
using MCRunner.Positions;
using PowerLanguage;
using System.Collections.Immutable;
using System;

namespace MCRunner.Strategy
{
    /// <summary>
    /// Manages a strategy and its positions.
    /// </summary>
    public class StrategyManager
    {
        /// <summary>
        /// Triggered after an order has been received and validated.
        /// </summary>
        public event Action<OrderInfo> OrderValidated;

        /// <summary>
        /// Triggered when an order has been canceled.
        /// </summary>
        public event Action<OrderInfo> OrderCanceled;

        private ImmutableHashSet<OrderInfo> previousSubmittedOrders =
            ImmutableHashSet<OrderInfo>.Empty;
        private ImmutableHashSet<OrderInfo> untriggeredOrders =
            ImmutableHashSet<OrderInfo>.Empty;
        private ImmutableHashSet<OrderInfo> validatedOrders =
            ImmutableHashSet<OrderInfo>.Empty;
        private readonly IPosition longPosition = new LongPosition();
        private readonly IPosition shortPosition = new ShortPosition();
        private readonly StrategyPerformance strategyInfo = new StrategyPerformance();

        /// <summary>
        /// Gets the current position size.
        /// </summary>
        private int Size
        {
            get
            {
                return (int)(longPosition.Size - shortPosition.Size);
            }
        }

        /// <summary>
        /// Retrieves the strategy performance information.
        /// </summary>
        public IStrategyPerformance StrategyInfo
        {
            get
            {
                return strategyInfo;
            }
        }

        /// <summary>
        /// Instantiates a new instance.
        /// </summary>
        /// <param name="orderCreator">The IOrderManaged to use to subscribe to
        /// orders.</param>
        public StrategyManager(IOrderManaged orderCreator)
        {
            orderCreator.OrderSent += OnOrderSent;
        }

        /// <summary>
        /// Triggers any orders in the queue. Any orders that were submitted previously
        /// but not re-submitted since the last call of this method will be canceled.
        /// </summary>
        /// <param name="bar">The latest Bar instance. The price data from this Bar is
        /// used to see if any orders have been triggered.</param>
        public void TriggerOrders(Bar bar)
        {
            foreach (var order in untriggeredOrders)
            {
                if (order.Order is IOrderPriced)
                {
                    var price = order.ConditionPrice;
                    if (price <= 0)
                    {
                        price = order.Price;
                    }

                    switch (order.OrderAction)
                    {
                        case EOrderAction.Buy:
                        case EOrderAction.BuyToCover:
                            if (price >= bar.Low)
                            {
                                OnOrderTriggered(order);
                            }
                            break;

                        case EOrderAction.Sell:
                        case EOrderAction.SellShort:
                            if (price <= bar.High)
                            {
                                OnOrderTriggered(order);
                            }
                            break;

                        default:
                            throw new ArgumentException("Unknown OrderAction");
                    }
                }
                else
                {
                    throw new ArgumentException("Unknown order type");
                }
            }

            // Any orders that are in previousSubmittedOrders but not in untriggeredOrders
            // were not resubmitted, and so should be canceled
            foreach (var order in previousSubmittedOrders)
            {
                if (!untriggeredOrders.Contains(order))
                {
                    OrderCanceled.SafeTrigger(order);
                }
            }

            previousSubmittedOrders = untriggeredOrders;
            untriggeredOrders = untriggeredOrders.Clear();
        }

        /// <summary>
        /// Called when a new order is sent. Validates the order and registers for when
        /// it's triggered.
        /// </summary>
        /// <param name="order">The information about the order that was sent.</param>
        private void OnOrderSent(OrderInfo order)
        {
            ValidateOrder(order);

            if (!validatedOrders.Contains(order))
            {
                validatedOrders = validatedOrders.Add(order);
                OrderValidated.SafeTrigger(order);
            }

            CheckIfOrderTriggered(order);
        }

        /// <summary>
        /// Called when a new order is triggered. Updates the strategy.
        /// </summary>
        /// <param name="order">The order that was triggered.</param>
        private void OnOrderTriggered(OrderInfo order)
        {
            switch (order.OrderAction)
            {
                case EOrderAction.Buy:
                case EOrderAction.Sell:
                    longPosition.UpdatePosition(order);
                    break;

                case EOrderAction.SellShort:
                case EOrderAction.BuyToCover:
                    shortPosition.UpdatePosition(order);
                    break;

                default:
                    throw new ArgumentException("Invalid OrderAction");
            }

            strategyInfo.MarketPosition = Size;

            // No need to track orders after they're filled
            validatedOrders = validatedOrders.Remove(order);
            untriggeredOrders = untriggeredOrders.Remove(order);
            previousSubmittedOrders = previousSubmittedOrders.Remove(order);
        }

        /// <summary>
        /// Validates an order to ensure it is sane to send to the market.
        /// </summary>
        /// <param name="order">The order to validate.</param>
        private void ValidateOrder(OrderInfo order)
        {
            switch (order.OrderAction)
            {
                case EOrderAction.Buy:
                case EOrderAction.Sell:
                    longPosition.ValidateOrder(order);
                    break;

                case EOrderAction.SellShort:
                case EOrderAction.BuyToCover:
                    shortPosition.ValidateOrder(order);
                    break;

                default:
                    throw new InvalidOrderException("Unknown OrderAction");
            }
        }

        /// <summary>
        /// If the order has a price condition, determines whether it has been triggered.
        /// </summary>
        /// <param name="order">The order to check if it has been triggered.</param>
        private void CheckIfOrderTriggered(OrderInfo order)
        {
            if (order.Order is IOrderMarket)
            {
                OnOrderTriggered(order);
            }
            else
            {
                untriggeredOrders = untriggeredOrders.Add(order);
            }
        }
    }
}
