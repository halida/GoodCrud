using System;
using Xunit;

namespace GoodCrud.Contract.Tests
{
    public class UnitTestHello
    {
        [Fact]
        public void Test_Hello()
        {
            Assert.True(true, "It works");
        }
        [Fact]
        public void Test_False()
        {
            // Assert.False(true, "It works");
        }
    }
}
