using System;

namespace PricingEngine
{
    /// <summary>
    /// An item in a shopping cart indentified by a Sku Id
    /// </summary>
    public class CartItem
    {
        public string SkuId { get; private set; }
        public int Quantity { get; private set; }

        public CartItem(string skuId, int quantity)
        {
            if (string.IsNullOrWhiteSpace(skuId))
            {
                throw new ArgumentException($"'{nameof(skuId)}' cannot be null or empty.", nameof(skuId));
            }

            if (quantity <= 0)
            {
                throw new ArgumentException($"'{nameof(quantity)}' must be greqater than zero.", nameof(quantity));
            }

            SkuId = skuId;
            Quantity = quantity;
        }

        public static CartItem Create(string skuId, int quantity)
        {
            return new CartItem(skuId, quantity);
        }
    }
}