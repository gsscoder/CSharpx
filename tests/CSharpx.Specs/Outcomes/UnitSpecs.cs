using Xunit;
using FluentAssertions;
using CSharpx;

public class UnitSpecs
{
    [Fact]
    public void Unit_values_should_be_equals()
    {
        var sut1 = new Unit();
        var sut2 = new Unit();

        var outcome = sut1.Equals(sut2);

        outcome.Should().BeTrue();
    }

    [Fact]
    public void Unit_values_should_compare_to_equality()
    {
        var sut1 = new Unit();
        var sut2 = new Unit();

        var outcome = sut1.CompareTo(sut2);
        
        outcome.Should().Be(0);
    }
}