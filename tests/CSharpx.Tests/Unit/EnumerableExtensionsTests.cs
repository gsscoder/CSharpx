using System.Linq;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx.Tests.Fakes;

namespace CSharpx.Tests.Unit
{
    public class EnumerableExtensionsTests
    {
        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_intersperse(int value)
        {
            var sequence = new int[] {0, 1, 2, 3, 4};
            var result = sequence.Intersperse(value);

            result.Should().NotBeEmpty()
                .And.HaveCount(sequence.Count() * 2 - 1)
                .And.SatisfyRespectively(
                    item => item.Should().Be(0),
                    item => item.Should().Be(value),
                    item => item.Should().Be(1),
                    item => item.Should().Be(value),
                    item => item.Should().Be(2),
                    item => item.Should().Be(value),
                    item => item.Should().Be(3),
                    item => item.Should().Be(value),
                    item => item.Should().Be(4)
                );
        }
    }
}