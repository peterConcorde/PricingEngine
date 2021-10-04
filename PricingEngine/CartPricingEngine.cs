using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    public class CartPricingEngine : IPricingEngine
    {
        private readonly IProductService productService;

        public CartPricingEngine(IProductService productService)
        {
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }


        public decimal ComputePrice(IEnumerable<CartItem> shoppingCart)
        {
            if (shoppingCart is null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }


            //Flatten the cart
            var itemGroups = shoppingCart.GroupBy(s => s.SkuId)
                                .Select(g => CartItem.Create(g.Key, g.Sum(x => x.Quantity)));


            // Apply any relevant promotions
            var promotions = productService.GetProductPromotions(itemGroups.Select(i => i.SkuId));

            (var price, var residualCart) = promotions.Aggregate((price : 0M, cart: itemGroups), 
                (acc,p) => {
                    (var promtionPrice, var residualCart) = p.ApplyPromotiom(acc.cart);
                    return (acc.price + promtionPrice, residualCart);
                });

            // Add the value of any remaining items
            var details = productService.GetProductDetails(itemGroups.Select(i => i.SkuId));
            price += residualCart.Select(g => details[g.SkuId] * g.Quantity).Sum();

            return price;
        }
    }
}
