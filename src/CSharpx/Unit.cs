//#define CSX_UNIT_INTERNAL // Uncomment or define at build time to set accessibility to internal.

using System;

namespace CSharpx
{
    /// <summary>The <c>Unit</c> type is a type that indicates the absence of a specific value; the
    /// <c>Unit</c> type has only a single value, which acts as a placeholder when no other value
    /// exists or is needed.</summary>
#if !CSX_UNIT_INTERNAL
    public
#endif
    struct Unit : IComparable<Unit>
    {
        private static readonly Unit @default = new Unit();

        /// <summary>Returns the hash code for this <c>Unit</c>.</summary>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>Determines whether this instance and a specified object, which must also be a
        /// <c>Unit</c> object, have the same value.</summary>
        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        /// <summary>Compares this instance with a specified object and indicates whether this instance
        /// precedes, follows, or appears in the same position in the sort order as the specified
        /// object.</summary>
        public int CompareTo(Unit obj)
        {
            return 0;
        }

        /// <summary>Converts this instance to a string representation.</summary>
        public override string ToString()
        {
            return "()";
        }

        /// <summary>Equality operator.</summary>
        public static bool operator ==(Unit first, Unit second)
        {
            return true;
        }

        /// <summary>Inequality operator.</summary>
        public static bool operator !=(Unit first, Unit second)
        {
            return false;
        }

        /// <summary>Singleton value.</summary>
        public static Unit Default { get { return @default; } }
    }
}