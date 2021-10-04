using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    public class CartItem
    {
        public string SkuId { get; private set; }
        public int Quantity { get; private set; }

        public CartItem(string skuId, int quantity)
        {
            SkuId = skuId;
            Quantity = quantity;
        }

        public static CartItem Create(string skuId, int quantity)
        {
            return new CartItem(skuId, quantity);
        }
    }
}
