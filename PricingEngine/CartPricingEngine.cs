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


            var details = productService.GetProductDetails(itemGroups.Select(i => i.SkuId));


            return itemGroups.Select(g => details[g.SkuId] * g.Quantity).Sum();
        }
    }
}
