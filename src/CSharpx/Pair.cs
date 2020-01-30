//#define CSX_PAIR_INTERNAL // Uncomment or define at build time to set accessibility to internal.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSharpx
{
    /// <summary>Provides static methods for creating pair objects.</summary>
    [Serializable]
#if !CSX_PAIR_INTERNAL
    public
#endif
    static class Pair
    {
        /// <summary>Creates a pair setting a value in first element and always <c>null</c> in the second.
        /// Dirty singleton, dirty 1-tuple.</summary>
        public static Pair<TFirst, object> Create<TFirst>(TFirst first) =>
            new Pair<TFirst, object>(first, null); 

        /// <summary>Create a pair, or 2-tuple.</summary>
        public static Pair<TFirst, TSecond> Create<TFirst, TSecond>(TFirst first, TSecond second) =>
            new Pair<TFirst, TSecond>(first, second); 
    }

    /// <summary>Represents a pair, or 2-tuple.</summary>
#if !CSX_PAIR_INTERNAL
    public
#endif
    struct Pair<TFirst, TSecond> : IStructuralEquatable, IStructuralComparable, IComparable
    {
        readonly TFirst _first;
        readonly TSecond _second;

        /// <summary>Initializes a new instance of the <c>Pair&lt;TFirst, TSecond&gt;</c> type.</summary>
        public Pair(TFirst first, TSecond second)
        {
            _first = first;
            _second = second;
        }

        /// <summary>Gets the value of the specified pair element.</summary>
        public object this[int index]
        {
            get {
                switch (index) {
                    case 0:  return _first;
                    case 1:  return _second;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>Gets the number of elements in the pair.</summary>
        public int Length => 2;

        /// <summary>Gets the value of the current <c>Pair&lt;TFirst, TSecond&gt;</c> object's first
        /// component.</summary>
        public TFirst First => _first;

        /// <summary>Gets the value of the current <c>Pair&lt;TFirst, TSecond&gt;</c> object's second
        /// component.</summary>
        public TSecond Second => _second;

        /// <summary>Returns a value that indicates whether the current  <c>Pair&lt;TFirst, TSecond&gt;</c>
        /// object is equal to a specified object.</summary>
        public override bool Equals(object obj) =>
            ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

        /// <summary>Returns a value that indicates whether the current <c>Pair&lt;TFirst, TSecond&gt;</c>
        /// object is equal to a specified object based on a specified comparison method.</summary>
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            if (other == null) return false;
            if (!(other is Pair<TFirst, TSecond> pair)) return false;

            return comparer.Equals(_first, pair._first) && comparer.Equals(_second, pair._second);
        }

        /// <summary>Compares the current <c>Pair&lt;TFirst, TSecond&gt;</c> object to a specified
        /// object and returns an integer that indicates whether the current object is before, after,
        /// or in the same position as the specified object in the sort order.</summary>
        int IComparable.CompareTo(object obj) =>
            ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

        /// <summary>Compares the current <c>Pair&lt;TFirst, TSecond&gt;</c> object to a specified
        /// object by using a specified comparer, and returns an integer that indicates whether the
        /// current object is before, after, or in the same position as the specified object in the
        /// sort order.</summary>
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (other == null) return 1;
            if (!(other is Pair<TFirst, TSecond> pair)) throw new ArgumentException(
                $"Argument must be of type {GetType()}", nameof(other));

            var c = comparer.Compare(_first, pair._first);
            if (c != 0) return c;
            return comparer.Compare(_second, pair._second);
        }

        /// <summary>Returns the hash code for the current <c>Pair&lt;TFirst, TSecond&gt;</c>
        /// object.</summary>
        public override int GetHashCode() =>
            ((_first.GetHashCode() << 5) + _first.GetHashCode()) ^ _second.GetHashCode();

        /// <summary>Calculates the hash code for the current <c>Pair&lt;TFirst, TSecond&gt;</c> object
        /// by using a specified computation method.</summary>
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer) =>
            ((comparer.GetHashCode(_first) << 5) + comparer.GetHashCode(_first)) ^ comparer.GetHashCode(_second);

        /// <summary>Returns a string that represents the value of this  <c>Pair&lt;TFirst, TSecond&gt;</c>
        /// instance.</summary>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("(");
            builder.Append(_first.ToString());
            builder.Append(", " );
            builder.Append(_second.ToString());
            builder.Append(")");
            return builder.ToString();
        }
    }
}