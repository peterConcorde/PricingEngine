using System.Collections.Generic;

namespace PricingEngine.Promitions
{
    public interface IPromotion
    {
        (decimal cost, IEnumerable<CartItem> residualCart) ApplyPromotiom(IEnumerable<CartItem> cart);
    }
}