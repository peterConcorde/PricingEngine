using System.Collections.Generic;

namespace PricingEngine.Promitions
{
    /// <summary>
    /// Defines a promtion that can be applied to a collection of items
    /// </summary>
    public interface IPromotion
    {
        (decimal cost, IEnumerable<CartItem> residualCart) ApplyPromotiom(IEnumerable<CartItem> cart);
    }
}