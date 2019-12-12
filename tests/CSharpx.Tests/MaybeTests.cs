using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;

namespace CSharpx.Tests
{
    public class MaybeTests
    {
        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Constructing_a_monadic_number_allows_the_same_to_be_returned_unchanged(int value)
        {
            var maybeInt = Maybe.Return(value);
            switch (maybeInt.Tag)
            {
                case MaybeType.Just:
                    ((Just<int>)maybeInt).Value.Should().Be(value);
                    break;
                default:
                    default(int).Should().Be(value);
                    maybeInt.Should().BeOfType<Nothing<int>>();
                    break;
            };
        }
    }
}