using Xunit;
using FluentAssertions;

namespace CSharpx.Tests.Unit
{
    public class UnitTests
    {
        [Fact]
        public void Unit_values_are_always_identical()
        {
            var unit1 = new CSharpx.Unit();
            var unit2 = new CSharpx.Unit();

            unit1.Should().Be(unit2);
            (unit1 == unit2).Should().BeTrue();
        }
    }
}