﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Promitions
{
    public class BundlePromotionOf2 : IPromotion
    {
        private readonly string item1;
        private readonly string item2;
        private readonly decimal cost;

        public BundlePromotionOf2(string item1, string item2, decimal cost)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.cost = cost;
        }

        private int CountItems(IEnumerable<CartItem> items, string targetSkuId)
        {
            return items.Where(i => i.SkuId == targetSkuId).Sum(i => i.Quantity);
        }

        public (decimal cost, IEnumerable<CartItem> residualCart) ApplyPromotiom(IEnumerable<CartItem> cart)
        {
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
    }
}