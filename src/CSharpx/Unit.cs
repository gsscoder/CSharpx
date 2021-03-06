//#define CSX_TYPES_INTERNAL // Uncomment or define at build time to set accessibility to internal.

using System;

namespace CSharpx
{
    /// <summary>The <c>Unit</c> type is a type that indicates the absence of a specific value; the
    /// <c>Unit</c> type has only a single value, which acts as a placeholder when no other value
    /// exists or is needed.</summary>
#if !CSX_TYPES_INTERNAL
    public
#endif
    struct Unit : IComparable
    {
        private static readonly Unit @default = new Unit();

        /// <summary>Returns the hash code for this <c>Unit</c>.</summary>
        public override int GetHashCode() => 0;

        /// <summary>Determines whether this instance and a specified object, which must also be a
        /// <c>Unit</c> object, have the same value.</summary>
        public override bool Equals(object obj) => obj == null || obj is Unit;

        /// <summary>Compares always to equality.</summary>
        public int CompareTo(object obj) => 0;

        /// <summary>Converts this instance to a string representation.</summary>
        public override string ToString() => "()";

        /// <summary><c>Unit</c> singleton instance.</summary>
        public static Unit Default { get { return @default; } }

        /// <summary>Returns <c>Unit</c> after executing a delegate.</summary>
        public static Unit Do(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            action();
            return Default;
        }
    }
}