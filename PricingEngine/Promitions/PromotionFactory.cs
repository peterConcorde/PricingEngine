using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine.Promitions
{
    public static class PromotionFactory
    {
        public static IPromotion CreateMultiBuyPromotion(string skuid, int quantity, decimal pricePerSet)
        {
            return new MultiBuyPromotion(skuid, quantity, pricePerSet);
        }

        public static IPromotion CreateBundleOf2Promotion(string item1, string item2, decimal cost)
        {
            return new BundlePromotionOf2(item1, item2, cost);
        }
    }
}
