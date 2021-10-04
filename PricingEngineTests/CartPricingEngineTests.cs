using PricingEngine;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using PricingEngine.Promitions;

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

        private IEnumerable<IPromotion> ActivePromotions { get; } = new List<IPromotion>
        {
            PromotionFactory.CreateMultiBuyPromotion("A", 3, 130),
            PromotionFactory.CreateMultiBuyPromotion("B", 2, 45),
            PromotionFactory.CreateBundleOf2Promotion("C", "D", 30)
        };

        #endregion

        #region Scenarios 

        [Theory]
        [InlineData(1,2,3,0, 100)]
        [InlineData(5, 5, 1, 0, 370)]
        [InlineData(3, 5, 1, 1, 280)]
        public void ApplyPromotionSet(int numberOfA, int numberOfB, int numberOfC, int numberOfD, decimal expected)
        {
            var cart = new List<CartItem>
            {
                CartItem.Create("A",numberOfA),
                CartItem.Create("B",numberOfB),
                CartItem.Create("C",numberOfC),
                CartItem.Create("D",numberOfD),  
            };

            mockProdcutService.Setup(m => m.GetProductDetails(It.IsAny<IEnumerable<string>>())).Returns(Prices);
            mockProdcutService.Setup(m => m.GetProductPromotions(It.IsAny<IEnumerable<string>>())).Returns(ActivePromotions);

            var result = CreateEngine().ComputePrice(cart);
            Assert.Equal(expected, result);
        }


        #endregion

        private readonly Mock<IProductService> mockProdcutService;

        public CartPricingEngineTests()
        {
            mockProdcutService = new Mock<IProductService>();
        }

        #region Test without promtions

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

        #endregion


    }
}
