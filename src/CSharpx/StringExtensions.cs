//Use project level define(s) when referencing with Paket.
//#define CSX_STRING_EXT_INTERNAL // Uncomment this to set StringExtensions accessibility to internal.

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace CSharpx
{
#if !CSX_STRING_EXT_INTERNAL
    public
#endif
    static class StringExtensions
    {
        /// <summary>
        /// Determines if a string is composed only by letter characters.
        /// </summary>
        public static bool IsAlpha(this string @string)
        {
            foreach (var @char in @string.ToCharArray()) {
                if (!char.IsLetter(@char) || char.IsWhiteSpace(@char)) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if a string is composed only by alphanumeric characters.
        /// </summary>
        public static bool IsAlphanumeric(this string @string)
        {
            foreach (var @char in @string.ToCharArray()) {
                if (!char.IsLetterOrDigit(@char) || char.IsWhiteSpace(@char)) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Determines if a string is contains any kind of white spaces.
        /// </summary>
        public static bool IsWhiteSpace(this string @string)
        {
            foreach (var @char in @string.ToCharArray()) {
                if (char.IsWhiteSpace(@char)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Replicates a string for a given number of times using a seperator.
        /// </summary>
        public static string Replicate(this string @string, uint count, string separator = " ")
        {
            if (separator == null) throw new ArgumentNullException(nameof(separator));

            var builder = new StringBuilder();
            for (var i = 0; i < count; i++) {
                builder.Append(@string);
                builder.Append(separator);
            }
            return builder.ToString(0, builder.Length - separator.Length);
        }

        /// <summary>
        /// Applies a given function to nth-word of string.
        /// </summary>
        public static string ApplyAt(this string @string, int index, Func<string, string> modifier)
        {
            if (index < 0) throw new ArgumentException(nameof(index));

            var words = @string.Split().ToArray();
            words[index] = modifier(words[index]);
            return string.Join(" ", words);
        }

        /// <summary>
        /// Selects a random index of a word that optionally satisfies a function.
        /// </summary>
        public static int ChoiceOfIndex(this string @string, Func<string, bool> validator = null)
        {
            Func<string, bool> _nullValidator = _ => true;
            var _validator = validator ?? _nullValidator;

            var words = @string.Split();
            var index = new Random().Next(0,  words.Length - 1);
            if (_validator(words[index])) {
                return index;
            }
            return ChoiceOfIndex(@string, validator);
        }

        /// <summary>
        /// Mangles a word with a non alphanumeric character as prefix or suffix.
        /// </summary>
        public static string Mangle(this string word)
        {
            if (word.IsWhiteSpace()) throw new ArgumentException(nameof(word));

            var nonAlphanumeric =
                new string[] {"!", "\"", "£", "$", "%", "&", "/", "(", ")", "="}.Choice();
            var prefix = new Random().Next(0, 1);
            if (prefix == 1) {
                return $"{word}{nonAlphanumeric}";
            }
            return $"{nonAlphanumeric}{word}";
        }

        /// <summary>
        /// Takes a value and a string and `intersperses' that value between its words.
        /// </summary>
        public static string Intersperse(this string @string, params object[] values)
        {
            if (values.Length == 0) {
                return @string;
            }
            return string.Join(" ", impl());
            IEnumerable<string> impl() {
                var words = @string.Split();
                var count = words.Length;
                var last = count - 1;
                for (var i = 0; i < count; i++) {
                    yield return words.ElementAt(i);
                    if (i < values.Length) {
                        var element = values[i];
                        if (element.GetType() == typeof(string)) {
                            yield return (string)element;
                        } else {
                            yield return element.ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sanitizes a string removing non alphanumeric characters and optionally normalizing
        /// white spaces.
        /// </summary>
        public static string Sanitize(this string @string, bool normalizeWhiteSpace = true)
        {
            return impl().Aggregate<char, string>(string.Empty, (s, c) => $"{s}{c}");
            IEnumerable<char> impl() {
                foreach (var @char in @string) {
                    if (Char.IsLetterOrDigit(@char)) {
                        yield return @char;
                    }
                    else if (Char.IsWhiteSpace(@char)) {
                        if (normalizeWhiteSpace) {
                            yield return ' ';
                        } else {
                            yield return @char;
                        }
                    }
                }
            }
        }
    }
}