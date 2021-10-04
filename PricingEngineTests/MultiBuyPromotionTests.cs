using PricingEngine;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using PricingEngine.Promitions;
using System.Linq;

namespace PricingEngineTests
{
    public class MultiBuyPromotionTests
    {


        [Fact]
        public void CanCreate()
        {
            var p = new MultiBuyPromotion("test", 4, 10.46M);
            Assert.NotNull(p);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void FirstArgInvalidCheck(string value)
        {
            Assert.Throws<ArgumentException>(() => new MultiBuyPromotion(value, 4, 10.46M));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-37)]
        public void QuntityMustBePosative(int value)
        {
            Assert.Throws<ArgumentException>(() => new MultiBuyPromotion("test", value, 48M));
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(-37)]
        public void PrinceMustNotBeNegative(decimal value)
        {
            Assert.Throws<ArgumentException>(() => new MultiBuyPromotion("test", 4, value));
        }

        [Theory]
        [InlineData("A", 1, 45, 180)]
        [InlineData("A", 2, 80, 160)]
        [InlineData("A", 3, 80, 80)]
        [InlineData("A", 4, 80, 80)]
        [InlineData("A", 5, 80, 0)]
        [InlineData("C", 2, 26.50, 53)]
        [InlineData("E", 2, 26.50, 0)]
        public void CanCalulatePromotionWithValue(string skuId, int size, decimal price, decimal expected)
        {
            var cart = new List<CartItem>
            {
                CartItem.Create("A",3),
                CartItem.Create("C",1),
                CartItem.Create("D",4),
                CartItem.Create("C",4),
                CartItem.Create("A",1),
                CartItem.Create("B",1),
                CartItem.Create("B",2)
            };


            var p = new MultiBuyPromotion(skuId, size, price);
            (var result, _) = p.ApplyPromotiom(cart);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("A", 1, 45, 12)]
        [InlineData("A", 2, 80, 12)]
        [InlineData("A", 3, 80, 13)]
        [InlineData("A", 4, 80, 12)]
        [InlineData("A", 5, 80, 16)]
        [InlineData("C", 2, 26.50, 12)]
        [InlineData("E", 2, 26.50, 16)]
        public void CanReduceCartTotalSize(string skuId, int size, decimal price, int expected)
        {
            var cart = new List<CartItem>
            {
                CartItem.Create("A",3),
                CartItem.Create("C",1),
                CartItem.Create("D",4),
                CartItem.Create("C",4),
                CartItem.Create("A",1),
                CartItem.Create("B",1),
                CartItem.Create("B",2)
            };


            var p = new MultiBuyPromotion(skuId, size, price);
            (_, var newCart) = p.ApplyPromotiom(cart);

            var result = newCart.Sum(i => i.Quantity);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("A", 1, 45, 0)]
        [InlineData("A", 2, 80, 0)]
        [InlineData("A", 3, 80, 1)]
        [InlineData("A", 4, 80, 0)]
        [InlineData("A", 5, 80, 4)]
        [InlineData("C", 2, 26.50, 1)]
        [InlineData("E", 2, 26.50, 0)]
        public void CanReduceCartTargetSize(string skuId, int size, decimal price, int expected)
        {
            var cart = new List<CartItem>
            {
                CartItem.Create("A",3),
                CartItem.Create("C",1),
                CartItem.Create("D",4),
                CartItem.Create("C",4),
                CartItem.Create("A",1),
                CartItem.Create("B",1),
                CartItem.Create("B",2)
            };


            var p = new MultiBuyPromotion(skuId, size, price);
            (_, var newCart) = p.ApplyPromotiom(cart);

            var result = newCart.Where(i => i.SkuId == skuId).Sum(i => i.Quantity);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void NullCheckForCart()
        {
            var p = new MultiBuyPromotion("test1", 4, 25);
            Assert.Throws<ArgumentNullException>(() => p.ApplyPromotiom(null));
        }

    }
}
