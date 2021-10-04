using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PricingEngine
{
    public interface IProductService
    {

        IEnumerable<string> GetProducts();

        IDictionary<string, decimal> GetProductDetails(IEnumerable<string> prodcuts);


    }
}
