using System;
using Xunit;

namespace TestsDuo2
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Basic test that should always pass
            Assert.True(true);
        }

        [Fact]
        public void StringManipulationTest()
        {
            // Testing a simple string manipulation
            string test = "Hello Duo";
            Assert.Contains("Duo", test);
            Assert.Equal(9, test.Length);
        }

        [Fact]
        public void MathOperationsTest()
        {
            // Test basic math operations
            Assert.Equal(4, 2 + 2);
            Assert.Equal(0, 2 - 2);
            Assert.Equal(4, 2 * 2);
            Assert.Equal(1, 2 / 2);
        }
    }
}