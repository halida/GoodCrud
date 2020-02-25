using System;
using Xunit;
using GoodCrud.Domain.Libraries;
using System.Collections.Generic;
using System.Linq;


namespace GoodCrud.Domain.Tests.Libraries
{
    public class CommonTests
    {
        [Fact]
        public void Test_GetStringList()
        {
            Func<string, List<string>> test = (raw) => Common.GetStringList(raw);

            // empty strings
            Assert.False(test(null).Any());
            Assert.False(test("").Any());
            Assert.False(test("  ").Any());

            Assert.True(Enumerable.SequenceEqual(test("a,b"), new List<string> { "a", "b" }));
            // trim
            Assert.True(Enumerable.SequenceEqual(test("a , b,, ,"), new List<string> { "a", "b" }));
            // sort
            Assert.True(Enumerable.SequenceEqual(test("cccc, a , b"), new List<string> { "a", "b", "cccc" }));
            // not sort
            Assert.True(Enumerable.SequenceEqual(
                Common.GetStringList("cccc, a , b", false),
                new List<string> { "cccc", "a", "b" }));
        }

    }
}