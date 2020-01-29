using System;
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
    [Property(Arbitrary = new[] { typeof(ArbitraryListOfIntegers) })]
    public void Trying_to_get_the_head_element_of_a_sequence_should_return_Just(
        FSharpList<int> values)
    {
        var outcome = values.TryHead();

        outcome.Should().BeEquivalentTo(Maybe.Just(values.ElementAt(0)));
    }

    [Fact]
    public void Trying_to_get_the_head_element_of_an_empty_sequence_should_return_Nothing()
    {
        var outcome = Enumerable.Empty<int>().TryHead();

        outcome.Should().BeEquivalentTo(Maybe.Nothing<int>());
    }
    #endregion

    #region TryLast
    [Property(Arbitrary = new[] { typeof(ArbitraryListOfIntegers) })]
    public void Trying_to_get_the_last_element_of_a_sequence_should_return_Just(
        FSharpList<int> values)
    {
        var outcome = values.TryLast();

        outcome.Should().BeEquivalentTo(Maybe.Just(values.Last()));
    }

    [Fact]
    public void Trying_to_get_the_last_element_of_an_empty_sequence_should_return_Nothing()
    {
        var outcome = Enumerable.Empty<int>().TryLast();

        outcome.Should().BeEquivalentTo(Maybe.Nothing<int>());
    }
    #endregion

    #region ToMaybe
    [Fact]
    public void An_empty_sequence_should_be_converted_to_Nothing()
    {
        var outcome = Enumerable.Empty<int>().ToMaybe();

        outcome.Should().BeEquivalentTo(Maybe.Nothing<IEnumerable<int>>());
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfIntegers) })]
    public void An_not_empty_sequence_should_be_converted_to_Just(FSharpList<int> values)
    {
        var outcome = values.ToMaybe();

        outcome.Should().BeEquivalentTo(Maybe.Just(values));
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
    public void Should_intersperse_a_value_in_a_sequence(int value)
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
    public void Should_flatten_a_sequence_by_one_level()
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
    #endregion

    #region Tail
    [Property(Arbitrary = new[] { typeof(ArbitraryListOfIntegers) })]
    public void Should_return_the_tail_of_a_sequence(FSharpList<int> values)
    {
        var outcome = values.Tail();

        outcome.Should().HaveCount(values.Count() - 1)
            .And.BeEquivalentTo(values.Skip(1));
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfIntegers) })]
    public void Should_return_the_tail_of_a_sequence_using_TailOrEmpty(FSharpList<int> values)
    {
        var outcome = values.TailOrEmpty();

        outcome.Should().HaveCount(values.Count() - 1)
            .And.BeEquivalentTo(values.Skip(1));
    }

    [Fact]
    public void Trying_to_get_the_tail_of_an_empty_sequence_throws_ArgumentException()
    {
        Action action = () => { foreach (var _ in Enumerable.Empty<int>().Tail()) { } };

        action.Should().ThrowExactly<ArgumentException>()
            .WithMessage("The input sequence has an insufficient number of elements.");
    }

    [Fact]
    public void Trying_to_get_the_tail_of_an_empty_sequence_returns_an_empty_sequence_using_TailOrEmpty()
    {
        var outcome = Enumerable.Empty<int>().TailOrEmpty();

        outcome.Should().HaveCount(0)
            .And.BeEquivalentTo(Enumerable.Empty<int>());
    }
    #endregion

    [Fact]
    public void Should_memoize_a_sequence()
    {
        Action action = () => NullYielder(true).Memoize();

        action.Should().Throw<Exception>();

        IEnumerable<object> NullYielder(bool raise)
        {
            if (raise) throw new Exception();

            yield return null;
        }
    }

    #region ChunkBySize
    [Fact]
    public void Should_partition_a_sequence_by_chunk_size_into_arrays_without_remainder()
    {
        var values = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        var outcome = values.ChunkBySize(3);

        outcome.Should().NotBeNullOrEmpty()
            .And.HaveCount(3)
            .And.SatisfyRespectively(
                item => item.Should().BeEquivalentTo(new int[] { 0, 1, 2 }),
                item => item.Should().BeEquivalentTo(new int[] { 3, 4, 5 }),
                item => item.Should().BeEquivalentTo(new int[] { 6, 7, 8 }));
    }

    [Fact]
    public void Should_partition_a_sequence_by_chunk_size_into_arrays_with_remainder()
    {
        var values = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        var outcome = values.ChunkBySize(3);

        outcome.Should().NotBeNullOrEmpty()
            .And.HaveCount(4)
            .And.SatisfyRespectively(
                item => item.Should().BeEquivalentTo(new int[] { 0, 1, 2 }),
                item => item.Should().BeEquivalentTo(new int[] { 3, 4, 5 }),
                item => item.Should().BeEquivalentTo(new int[] { 6, 7, 8 }),
                item => item.Should().BeEquivalentTo(new int[] { 9, 10 }));
    }

    [Fact]
    public void Should_partition_a_sequence_in_a_single_chunk_if_chunk_size_is_equal_than_elements_count()
    {
        var values = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        var outcome = values.ChunkBySize(10);

        outcome.Should().NotBeNullOrEmpty()
            .And.HaveCount(1)
            .And.SatisfyRespectively(
                item => item.Should().BeEquivalentTo(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
    }

    [Fact]
    public void Should_partition_a_sequence_in_a_single_chunk_if_chunk_size_is_greater_than_elements_count()
    {
        var values = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        var outcome = values.ChunkBySize(11);

        outcome.Should().NotBeNullOrEmpty()
            .And.HaveCount(1)
            .And.SatisfyRespectively(
                item => item.Should().BeEquivalentTo(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfIntegers) })]
    public void Trying_to_partition_a_sequence_by_zero_chunks_throws_ArgumentException(
        FSharpList<int> values)
    {
        Action action = () => { foreach (var _ in values.ChunkBySize(0)) {}; };

        action.Should().ThrowExactly<ArgumentException>()
            .WithMessage("The input must be positive.");
    }
    
    [Property(Arbitrary = new[] { typeof(ArbitraryListOfIntegers) })]
    public void Trying_to_partition_a_sequence_by_a_negative_number_of_chunks_throws_ArgumentException(
        FSharpList<int> values)
    {
        Action action = () => { foreach (var _ in values.ChunkBySize(-1)) {}; };

        action.Should().ThrowExactly<ArgumentException>()
            .WithMessage("The input must be positive.");
    }
    #endregion
}