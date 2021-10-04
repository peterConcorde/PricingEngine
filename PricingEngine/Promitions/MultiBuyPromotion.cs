using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingEngine.Promitions
{
    /// <summary>
    /// Applies a discount when an item is included multiple times
    /// </summary>
    public class MultiBuyPromotion : IPromotion
    {
        private readonly string skuid;
        private readonly int quantity;
        private readonly decimal pricePerSet;

        /// <summary>
        /// Create a multi buy promotion
        /// </summary>
        /// <param name="skuid"> The target product</param>
        /// <param name="quantity">Number of items per deal </param>
        /// <param name="pricePerSet"> Price for the deal</param>
        public MultiBuyPromotion(string skuid, int quantity, decimal pricePerSet)
        {
            if (string.IsNullOrWhiteSpace(skuid))
            {
                throw new ArgumentException($"'{nameof(skuid)}' cannot be null or empty.", nameof(skuid));
            }

            if (quantity <= 0)
            {
                throw new ArgumentException($"'{nameof(quantity)}' must be greater than zero.", nameof(quantity));
            }

            if (pricePerSet <= 0)
            {
                throw new ArgumentException($"'{nameof(pricePerSet)}' must not be less than zero.", nameof(pricePerSet));
            }

            this.skuid = skuid;
            this.quantity = quantity;
            this.pricePerSet = pricePerSet;
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

            var itemCount = cart.Where(i => i.SkuId == this.skuid)
                                    .Sum(i => i.Quantity);

            var price = (itemCount / quantity) * pricePerSet;
            var remainder = itemCount % quantity;

            var residualCart = cart.Where(i => i.SkuId != skuid);

            if (remainder > 0)
            {
                residualCart = residualCart.Append(CartItem.Create(skuid, remainder));
            }

            return (price, residualCart);
        }
    }
}