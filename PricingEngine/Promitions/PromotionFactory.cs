namespace PricingEngine.Promitions
{
    /// <summary>
    /// Create promotions
    /// </summary>
    public static class PromotionFactory
    {
        /// <summary>
        /// Create a promotion that applies a discount if an item is purchased multiple times
        /// </summary>
        /// <param name="skuid"></param>
        /// <param name="quantity"></param>
        /// <param name="pricePerSet"></param>
        /// <returns></returns>
        public static IPromotion CreateMultiBuyPromotion(string skuid, int quantity, decimal pricePerSet)
        {
            return new MultiBuyPromotion(skuid, quantity, pricePerSet);
        }

        /// <summary>
        /// Create a promotion that applies a discount if two differnt items are purchased
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public static IPromotion CreateBundleOf2Promotion(string item1, string item2, decimal cost)
        {
            return new BundlePromotionOf2(item1, item2, cost);
        }
    }
}