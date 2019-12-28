using Xunit;
using FluentAssertions;

namespace CSharpx.Tests.Unit
{
    public class HelpersTests
    {
        [Fact]
        public void Should_detect_alphanumeric_characters()
        {
            "hello".IsAlphanumeric().Should().BeTrue();
            "0123456789".IsAlphanumeric().Should().BeTrue();
            "hello01234".IsAlphanumeric().Should().BeTrue();
            "hello.tests".IsAlphanumeric().Should().BeFalse();
            "hello tests".IsAlphanumeric().Should().BeFalse();
        }

        [Fact]
        public void Should_detect_whitespace_characters()
        {
            "hello01234".ContainsWhiteSpace().Should().BeFalse();
            "hello.tests".ContainsWhiteSpace().Should().BeFalse();
            "hello tests".ContainsWhiteSpace().Should().BeTrue();
            "hello\ntests".ContainsWhiteSpace().Should().BeTrue();
            "hello\ttests".ContainsWhiteSpace().Should().BeTrue();
        }

        [Fact]
        public void Should_sanitize_strings_normalizing_white_spaces()
        {
            "hello tests@".Sanitize().Should().Be("hello tests");
            "hello\ttests@".Sanitize().Should().Be("hello tests");
        }

        [Fact]
        public void Should_sanitize_strings_without_normalizing_white_spaces()
        {
            "hello\ntests@".Sanitize(normalizeWhiteSpace: false).Should().Be("hello\ntests");
            "hello\ttests@".Sanitize(normalizeWhiteSpace: false).Should().Be("hello\ttests");
        }

        [Fact]
        public void Should_sort_arrays()
        {
            (new int[] { 7, 3, 1, 2, 5, 4, 0, 6, 9, 8 }.Sort())
                .Should().BeEquivalentTo(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            (new string[] { "b", "e", "g", "a", "h", "j", "l", "c", "d", "f" }.Sort())
                .Should().BeEquivalentTo(new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "l", "j" });
        }
    }
}