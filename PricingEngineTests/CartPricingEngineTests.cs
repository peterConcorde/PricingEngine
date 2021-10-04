using PricingEngine;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;


namespace PricingEngineTests
{
    public class CartPricingEngineTests
    {

        #region data
        private IDictionary<string, decimal> Prices { get; } = new Dictionary<string, decimal>
        {
            { "A" , 50 },
            { "B" , 30 },
            { "C" , 20 },
            { "D" , 15 }
        };

        #endregion

        private readonly Mock<IProductService> mockProdcutService;

        public CartPricingEngineTests()
        {
            mockProdcutService = new Mock<IProductService>();
        }


        private IPricingEngine CreateEngine() => new CartPricingEngine(mockProdcutService.Object);


        [Fact]
        public void CanCreate()
        {
            Assert.NotNull(CreateEngine());
        }

        [Fact]
        public void ProdcutServiceNullCheck()
        {
            Assert.Throws<ArgumentNullException>(() => new CartPricingEngine(null));
        }

        [Fact]
        public void ComputeCartNull()
        {
            Assert.Throws<ArgumentNullException>(() => CreateEngine().ComputePrice(null));
        }

        [Fact]
        public void ComputeCartVauleWithNoDeals()
        {
            mockProdcutService.Setup(m => m.GetProductDetails(It.IsAny<IEnumerable<string>>())).Returns(Prices);

            var cart = new List<CartItem>
            {
                CartItem.Create("A",1),
                CartItem.Create("A",1),
                CartItem.Create("C",1),
                CartItem.Create("D",1),
                CartItem.Create("C",1),
                CartItem.Create("A",1),
                CartItem.Create("A",1),
                CartItem.Create("B",1)
            };

            decimal expected = 285;

            var result = CreateEngine().ComputePrice(cart);
            Assert.Equal(expected, result);          
        }

        [Fact]
        public void ComputeCartVauleWithNoDealsComplexCart()
        {
            mockProdcutService.Setup(m => m.GetProductDetails(It.IsAny<IEnumerable<string>>())).Returns(Prices);

            var cart = new List<CartItem>
            {
                CartItem.Create("A",3),
                CartItem.Create("C",1),
                CartItem.Create("D",4),
                CartItem.Create("C",1),
                CartItem.Create("A",1),
                CartItem.Create("B",1),
                CartItem.Create("B",2)
            };

            decimal expected = 390;

            var result = CreateEngine().ComputePrice(cart);
            Assert.Equal(expected, result);
        }


    }
}
