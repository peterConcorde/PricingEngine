using System.Collections.Generic;

namespace PricingEngine
{
    /// <summary>
    /// Used to determine the value of a set of items
    /// </summary>
    public interface IPricingEngine
    {
        decimal ComputePrice(IEnumerable<CartItem> shoppingCart);
    }
}