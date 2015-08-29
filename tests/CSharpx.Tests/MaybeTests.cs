using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace CSharpx.Tests
{
    public class MaybeTests
    {
        [Fact]
        public void Constructing_a_monadic_integer_value_allows_the_same_to_be_extracted_unchanged()
        {
            Prop.ForAll<int>(
                x =>
                {
                    var maybeInt = Maybe.Return(x);
                    switch (maybeInt.Tag)
                    {
                        case MaybeType.Just:
                            ((Just<int>)maybeInt).Value.ShouldBeEquivalentTo(x);
                            break;
                        default:
                            default(int).ShouldBeEquivalentTo(x);
                            maybeInt.Should().BeOfType<Nothing<int>>();
                            break;
                    }
                }).QuickCheckThrowOnFailure();
        }
    }
}
