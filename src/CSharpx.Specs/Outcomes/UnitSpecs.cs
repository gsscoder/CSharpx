using Xunit;
using FluentAssertions;
using CSharpx;

public class UnitSpecs
{
    [Fact]
    public void Unit_values_should_be_equal()
    {
        var unit1 = new Unit();
        var unit2 = new Unit();

        unit1.Should().Be(unit2);
        (unit1 == unit2).Should().BeTrue();
    }

    [Fact]
    public void Unit_values_should_match_to_equality()
    {
        var unit1 = new Unit();
        var unit2 = new Unit();

        unit1.CompareTo(unit2).Should().Be(0);
    }
}