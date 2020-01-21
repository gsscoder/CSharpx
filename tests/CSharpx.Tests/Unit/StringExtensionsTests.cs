using System;
using System.Linq;
using Xunit;
using FluentAssertions;

namespace CSharpx.Tests.Unit
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("hello", true)]
        [InlineData("0123456789", false)]
        [InlineData("hello01234", false)]
        [InlineData("hello.tests", false)]
        [InlineData("hello tests", false)]
        public void Should_detect_letter_characters(string value, bool expected)
        {
            value.IsAlpha().Should().Be(expected);
        }

        [Theory]
        [InlineData("hello", true)]
        [InlineData("0123456789", true)]
        [InlineData("hello01234", true)]
        [InlineData("hello.tests", false)]
        [InlineData("hello tests", false)]
        public void Should_detect_alphanumeric_characters(string value, bool expected)
        {
            value.IsAlphanumeric().Should().Be(expected);
        }

        [Theory]
        [InlineData("hello01234", false)]
        [InlineData("hello.tests", false)]
        [InlineData("hello tests", true)]
        [InlineData("hello\ntests", true)]
        [InlineData("hello\ttests", true)]
        public void Should_detect_whitespace_characters(string value, bool expected)
        {
            value.IsWhiteSpace().Should().Be(expected);
        }

        [Theory]
        [InlineData("hello tests@", "hello tests")]
        [InlineData("hello\ttests@", "hello tests")]
        [InlineData("hello-tests@", "hellotests")]
        public void Should_sanitize_strings_normalizing_white_spaces(string value, string expected)
        {
            value.Sanitize().Should().Be(expected);
        }

        [Theory]
        [InlineData("hello\ntests@", "hello\ntests")]
        [InlineData("hello\ttests@", "hello\ttests")]
        public void Should_sanitize_strings_without_normalizing_white_spaces(string value, string expected)
        {
            value.Sanitize(normalizeWhiteSpace: false).Should().Be(expected);
        }

        [Theory]
        [InlineData("hello this is a test", new object[] {'!', "!!", 10}, "hello ! this !! is 10 a test")]
        public void Should_intersperse_values(string value, object[] values, string expected)
        {
            value.Intersperse(values).Should().Be(expected);
        }

        [Theory]
        [InlineData("hello", 0, "", "")]
        [InlineData("hello", 1, "", "hello")]
        [InlineData("hello", 5, " ", "hello hello hello hello hello")]
        public void Should_replicate(string value, int count, string separator, string expected)
        {
            value.Replicate(count, separator).Should().Be(expected);
        }

        [Theory]
        [InlineData("hello", 0, 0)]
        [InlineData("hello", 1, 1)]
        [InlineData("hello", 3, 2)]
        [InlineData("hello tests", 3, 3)]
        public void Should_mangle(string value, int times, int maxLength)
        {
            int mangleSize = (int)times * (int)maxLength;

            var expected = value.Mangle(times, maxLength);

            expected.Length.Should().Be(value.Length + mangleSize);

            var expectedCount = (from @char in expected.ToCharArray()
                                 where !char.IsLetterOrDigit(@char) && !char.IsWhiteSpace(@char)
                                 select @char).Count();

            expectedCount.Should().Be(mangleSize);
        }

        [Fact]
        public void Mangle_beyond_string_length_throws_ArgumentException()
        {
            Action action = () => "hello from magle extension test".Mangle(100, 3);

            action.Should().ThrowExactly<ArgumentException>()
                .WithMessage("times");
        }
    }
}