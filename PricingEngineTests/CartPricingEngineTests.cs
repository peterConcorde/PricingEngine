using PricingEngine;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;


namespace PricingEngineTests
{
    public class CartPricingEngineTests
    {
        private IDictionary<string, decimal> Prices { get; } = new Dictionary<string, decimal>
        {
            { "A" , 50 },
            { "B" , 30 },
            { "C" , 20 },
            { "D" , 15 }
        };



        [Fact]
        public void ComputeCartVauleWithNoDeals()
        {
            Mock<IProductService> mockProdcutService = new();
            mockProdcutService.Setup(m => m.GetProductDetails(It.IsAny<IEnumerable<string>>())).Returns(Prices);
            IPricingEngine pe = new CartPricingEngine(mockProdcutService.Object);

            var cart = new List<string>
            {
                "A", "A","C","D","C","A","A", "B"
            };

            decimal expected = 285;

            var result = pe.ComputePrice(cart);

            Assert.Equal(expected, result);
            
        }
    }
}
