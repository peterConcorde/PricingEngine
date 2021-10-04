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
            this.productService = productService;
        }


        public decimal ComputePrice(IEnumerable<string> shoppingCart)
        {
            var itemGroups = shoppingCart.GroupBy(s => s).Select(g => (SkuId : g.Key, Quantity : g.Count()));
            var details = productService.GetProductDetails(itemGroups.Select(i => i.SkuId));


            return itemGroups.Select(g => details[g.SkuId] * g.Quantity).Sum();
        }
    }
}
