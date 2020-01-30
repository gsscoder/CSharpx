using System;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx;

public class PairSpecs
{
    static CryptoRandom _random = new CryptoRandom();

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Pairs_with_same_values_should_be_equals(string[] values)
    {
        values.ForEach(first => {
            var second = _random.Next();

            var sut1 = Pair.Create(first, second);
            var sut2 = Pair.Create(first, second);

            var outcome = sut1.Equals(sut2);

            outcome.Should().BeTrue();
        });
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Pairs_with_same_values_should_compare_to_equality(string[] values)
    {
        values.ForEach(first => {
            var second = _random.Next();

            var sut1 = Pair.Create(first, second);
            var sut2 = Pair.Create(first, second);

            var outcome = ((IComparable)sut1).CompareTo(sut2);

            outcome.Should().Be(0);
        });
    }
}