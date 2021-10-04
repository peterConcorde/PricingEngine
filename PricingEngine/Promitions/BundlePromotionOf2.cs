using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingEngine.Promitions
{
    /// <summary>
    /// Applies discounts when two products are bought togther
    /// </summary>
    public class BundlePromotionOf2 : IPromotion
    {
        private readonly string item1;
        private readonly string item2;
        private readonly decimal cost;

        public BundlePromotionOf2(string item1, string item2, decimal cost)
        {
            if (string.IsNullOrWhiteSpace(item1))
            {
                throw new ArgumentException($"'{nameof(item1)}' cannot be null or empty.", nameof(item1));
            }

            if (string.IsNullOrWhiteSpace(item2))
            {
                throw new ArgumentException($"'{nameof(item2)}' cannot be null or empty.", nameof(item2));
            }

            if (item1 == item2)
            {
                throw new ArgumentException($"'{nameof(item1)}' cannot be the same as {nameof(item2)}.", nameof(item2));
            }

            if (cost < 0)
            {
                throw new ArgumentException(nameof(cost), $"'{nameof(cost)}' must be not be less than zero.");
            }

            this.item1 = item1;
            this.item2 = item2;
            this.cost = cost;
        }

        /// <summary>
        /// Apply the promotion to a collection of items
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public (decimal cost, IEnumerable<CartItem> residualCart) ApplyPromotiom(IEnumerable<CartItem> cart)
        {
            if (cart is null)
            {
                throw new ArgumentNullException(nameof(cart));
            }

            var item1Count = CountItems(cart, item1);
            var item2Count = CountItems(cart, item2);

            var promtionCount = item1Count < item2Count ? item1Count : item2Count;
            var promtionValue = promtionCount * cost;

            var residualCart = cart.Where(i => i.SkuId != item1 && i.SkuId != item2);

            residualCart = (item1Count - item2Count) switch
            {
                var a when a > 0 => residualCart.Append(CartItem.Create(item1, a)),
                var a when a < 0 => residualCart.Append(CartItem.Create(item2, -a)),
                _ => residualCart
            };

            return (promtionValue, residualCart);
        }

        /// <summary>
        /// Count the number of one kind of item
        /// </summary>
        /// <param name="items"></param>
        /// <param name="targetSkuId"></param>
        /// <returns></returns>
        private int CountItems(IEnumerable<CartItem> items, string targetSkuId)
        {
            return items.Where(i => i.SkuId == targetSkuId).Sum(i => i.Quantity);
        }
    }
}