namespace YuYan.MockTest
{
    using System;
    using Xunit;
    using NSubstitute;

    public class RepositoryTest
    {

        public interface ICalculator {
            int Add(int a, int b);
            string Mode { get; set; }
            event EventHandler PoweringUp;
        }

        [Fact]
        public void Prac_Test()
        {
            var mockCalculator = Substitute.For<ICalculator>();
            mockCalculator.Add(1, 2).Returns(3);

            
            var result = mockCalculator.Add(1,2);
            mockCalculator.Received().Add(1, 2);
            mockCalculator.DidNotReceive().Add(3, 2);
            Assert.Equal<int>(3, result);
        }

        [Fact]
        public void Prac2_Test()
        {
            var mockCalculator = Substitute.For<ICalculator>();
            mockCalculator.Mode.Returns("DEC");
            Assert.Equal<string>(mockCalculator.Mode, "DEC");

            mockCalculator.Mode = "HEX";
            Assert.Equal<string>(mockCalculator.Mode, "HEX"); 
        }

        [Fact]
        public void Prac3_Test()
        {
            // stick to substituting interfaces
            var mockCalculator = Substitute.For<ICalculator>();

            mockCalculator.Add(10, -5);
            mockCalculator.Received().Add(10, Arg.Any<int>());
            mockCalculator.Received().Add(10, Arg.Is<int>(x => x < 0));
        }
    }
}
