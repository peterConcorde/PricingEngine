using PricingEngine.Promitions;
using System.Collections.Generic;

namespace PricingEngine
{
    /// <summary>
    /// Gather information about a available products and thier pricing
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Get a list of product Ids
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetProducts();

        /// <summary>
        /// Get the price for items in a set of products
        /// </summary>
        /// <param name="prodcuts"></param>
        /// <returns></returns>
        IDictionary<string, decimal> GetProductDetails(IEnumerable<string> prodcuts);

        /// <summary>
        /// Get promotions related to set of products
        /// </summary>
        /// <param name="prodcuts"></param>
        /// <returns></returns>
        IEnumerable<IPromotion> GetProductPromotions(IEnumerable<string> prodcuts);
    }
}