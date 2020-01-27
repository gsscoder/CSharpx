using System.Collections.Generic;
using System.Linq;
using Microsoft.FSharp.Collections;
using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx;

public class EnumerableExtensionsSpecs
{
    #region TryHead
    [Fact]
    public void When_trying_to_get_head_element_should_return_none_in_case_of_empty_sequence()
    {
        var outcome = Enumerable.Empty<int>().TryHead();

        outcome.Should().BeEquivalentTo(Maybe.Nothing<int>());
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfIntegers) })]
    public void When_trying_to_get_head_element_should_return_just_in_case_of_not_empty_sequence(
        FSharpList<int> values)
    {
        var outcome = values.TryHead();

        outcome.Should().BeEquivalentTo(Maybe.Just(values.ElementAt(0)));
    }
    #endregion

    #region Choose
    [Theory]
    [InlineData(
        new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9},
        new int[] {0, 2, 4, 6, 8})]
    public void Should_choose_elements_to_create_a_new_sequence(
        IEnumerable<int> values, IEnumerable<int> expected)
    {
        var outcome = values.Choose(x => x % 2 == 0
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

        var outcome = sequence.Intersperse(value);

        outcome.Should().NotBeEmpty()
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

        var outcome = sequence.FlattenOnce();
        
        outcome.Should().BeEquivalentTo(new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8});
    }

    [Theory]
    [InlineData(
        new string[] {"word1 word2 word3", "word4 word5 word6"},
        new string[] {"word1", "word2", "word3", "word4", "word5", "word6"})]
    public void Should_flatten_string_sequence(
        IEnumerable<string> values, IEnumerable<string> expected)
    {
        var outcome = values.FlattenOnce();
        
        outcome.Should().BeEquivalentTo(expected);
    }
    #endregion
}