using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx;

public class MaybeSpecs
{
    [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
    public void Shoud_build_Just(int value)
    {
        if (value == default(int)) return;// Skip default values

        var outcome = Maybe.Just(value);

        outcome.Should().NotBeNull()
            .And.BeOfType<Just<int>>();
        outcome.FromJust().Should().Be(value);
    }

    [Fact]
    public void Shoud_build_Nothing()
    {
        var outcome = Maybe.Nothing<int>();

        outcome.Should().NotBeNull()
            .And.BeOfType<Nothing<int>>();
        outcome.Tag.Should().Be(MaybeType.Nothing);
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
    public void Shoud_return_proper_maybe_with_a_value_type(int value)
    {
        var outcome = Maybe.Return(value);

         outcome.Should().NotBeNull();

        if (value == default(int)) {
            outcome.Should().BeOfType<Nothing<int>>();
        }
        else {
            outcome.Should().BeOfType<Just<int>>();
            outcome.FromJust().Should().Be(value);
        }
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Shoud_return_proper_maybe_with_a_reference_type(string[] values)
    {
        values.ForEach(value =>
            {
                var outcome = Maybe.Return(value);

                outcome.Should().NotBeNull();

                if (value == null) {
                    outcome.Should().BeOfType<Nothing<string>>();
                }
                else {
                    outcome.Should().BeOfType<Just<string>>();
                    outcome.FromJust().Should().Be(value);
                }
            });
    }


    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void FromJust_should_unwrap_the_value_or_lazily_return_from_a_function(string[] values)
    {
        values.ForEach(value =>
            {
                Func<string> func = () => "foo";

                var sut = Maybe.Return(value);

                var outcome = sut.FromJust(func);

                if (value == null) outcome.Should().NotBeNull().And.Be(func());
                else outcome.Should().NotBeNull().And.Be(value);
            });
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Should_return_a_singleton_sequence_with_Just_and_an_empty_with_Nothing(string[] values)
    {
        values.ForEach(value =>
            {
                var sut = Maybe.Return(value);

                var outcome = sut.ToEnumerable();

                if (value == null) {
                    outcome.Should().NotBeNull().And.BeEmpty();
                }
                else {
                    outcome.Should().NotBeNullOrEmpty().And.HaveCount(1);
                    outcome.ElementAt(0).Should().Be(value);
                }
            });
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Should_return_Just_values_from_a_sequence(string[] values)
    {
        var maybes = from value in values select Maybe.Return(value);

        var outcome = maybes.Justs();

        outcome.Should().NotBeNullOrEmpty()
            .And.HaveCountLessOrEqualTo(values.Count())
            .And.ContainInOrder(from value in values where value != null select value);
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Should_count_Nothing_values_of_a_sequence(string[] values)
    {
        var maybes = from value in values select Maybe.Return(values);

        var outcome = maybes.Nothings();

        outcome.Should().BeLessOrEqualTo(values.Count());
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Should_throw_out_Just_values_from_a_sequence(string[] values)
    {
        Func<string, Maybe<int>> readInt = value => { 
            if (int.TryParse(value, out int result)) return Maybe.Just(result);
            return Maybe.Nothing<int>(); };

        var expected = from value in values where int.TryParse(value, out int _)
                       select int.Parse(value);

        var outcome = values.Map(readInt);

        outcome.Should().NotBeNullOrEmpty()
            .And.HaveCount(expected.Count())
            .And.ContainInOrder(expected);
    }
}