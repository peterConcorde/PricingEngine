using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Promitions
{
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
            this.skuid = skuid;
            this.quantity = quantity;
            this.pricePerSet = pricePerSet;
        }


        public (decimal cost, IEnumerable<CartItem> residualCart) ApplyPromotiom(IEnumerable<CartItem> cart)
        {
            var itemCount = cart.Where(i => i.SkuId == this.skuid)
                                    .Sum(i => i.Quantity);

            var price = (itemCount / quantity) * pricePerSet;
            var remainder = itemCount % quantity;

            var residualCart =  cart.Where(i => i.SkuId != skuid);

            if (remainder > 0)
            {
                residualCart = residualCart.Append(CartItem.Create(skuid, remainder));
            }

            return (price, residualCart);
        }
    }
}
