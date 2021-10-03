using PricingEngine;
using System;
using Xunit;

namespace PricingEngineTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var a = new Class1();
            Assert.Equal(125, a.TestMe());
        }
    }
}
