using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    public interface IPricingEngine
    {
        decimal ComputePrice(IEnumerable<string> shoppingCart);
    }
}
