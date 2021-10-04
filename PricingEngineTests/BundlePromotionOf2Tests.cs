﻿using PricingEngine;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using PricingEngine.Promitions;
using System.Linq;

namespace PricingEngineTests
{
    public class BundlePromotionOf2Tests
    {


        [Fact]
        public void CanCreate()
        {
            var p = new BundlePromotionOf2("test", "test", 10.46M);
            Assert.NotNull(p);
        }

  

        [Theory]
        [InlineData("A", "B", 26.59, 79.77)]
        [InlineData("A", "C", 93.2, 372.8)]
        [InlineData("A", "D", 40, 160)]
        [InlineData("A", "E", 80, 0)]
        [InlineData("C", "D", 80, 320)]
        [InlineData("E", "E", 26.50, 0)]
        public void CanCalulatePromotionWithValue(string Sku1, string sku2, decimal price, decimal expected)
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


            var p = new BundlePromotionOf2(Sku1, sku2, price);
            (var result, _) = p.ApplyPromotiom(cart);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("A", "B", 26.59, 10)]
        [InlineData("A", "C", 93.2, 8)]
        [InlineData("A", "D", 40, 8)]
        [InlineData("A", "E", 80, 16)]
        [InlineData("C", "D", 80, 8)]
        [InlineData("E", "E", 26.50, 16)]
        public void CanReduceCartTotalSize(string sku1, string sku2, decimal price, int expected)
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


            var p = new BundlePromotionOf2(sku1, sku2, price);
            (_, var newCart) = p.ApplyPromotiom(cart);

            var result = newCart.Sum(i => i.Quantity);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("A", "B", 26.59, 1)]
        [InlineData("A", "C", 93.2, 0)]
        [InlineData("A", "D", 40, 0)]
        [InlineData("A", "E", 80, 4)]
        [InlineData("C", "D", 80, 1)]
        [InlineData("E", "E", 26.50, 0)]
        public void CanReduceCartFirstTargetSize(string sku1, string sku2, decimal price, int expected)
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


            var p = new BundlePromotionOf2(sku1, sku2, price);
            (_, var newCart) = p.ApplyPromotiom(cart);

            var result = newCart.Where(i => i.SkuId == sku1).Sum(i => i.Quantity);

            Assert.Equal(expected, result);
        }

    }
}