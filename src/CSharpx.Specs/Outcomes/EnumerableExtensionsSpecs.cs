using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx;

public class EnumerableExtensionsSpecs
{
    #region Choose
    [Theory]
    [InlineData(
        new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
        new int[] {0, 2, 4, 6, 8})]
    public void Should_choose_elements_to_create_a_new_sequence(
        IEnumerable<int> value, IEnumerable<int> expected)
    {
        var outcome = value.Choose(x => x % 2 == 0
                                        ? Maybe.Just(x)
                                        : Maybe.Nothing<int>());
        
        outcome.Should().BeEquivalentTo(expected);
    }
    #endregion

    #region Intersperse
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
    #endregion

    #region FlattenOnce
    [Fact]
    public void Should_flatten_sequence_by_one_level()
    {
        var sequence = new List<IEnumerable<int>>()
            {
                new int[] {0, 1, 2},
                new int[] {3, 4, 5},
                new int[] {6, 7, 8}
            };

        sequence.FlattenOnce().Should().BeEquivalentTo(new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8});
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
    #endregion
}