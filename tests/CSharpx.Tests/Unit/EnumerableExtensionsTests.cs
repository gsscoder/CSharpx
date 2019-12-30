using System.Collections.Generic;
using System.Linq;
using Xunit;
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

        [Theory]
        [InlineData(
            new string[] {"word1 word2 word3", "word4 word5 word6"},
            new string[] {"word1", "word2", "word3", "word4", "word5", "word6"})]
        public void Should_flatten_string_sequence(
            IEnumerable<string> value, IEnumerable<string> expected)
        {
            value.FlattenOnce().Should().BeEquivalentTo(expected);
        }
    }
}