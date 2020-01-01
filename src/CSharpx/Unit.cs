//#define CSX_UNIT_INTERNAL // Uncomment or define at build time to set Unity accessibility to internal.

using System;

namespace CSharpx
{
#if !CSX_UNIT_INTERNAL
    public
#endif
    struct Unit : IEquatable<Unit>, IComparable<Unit>
    {
        private static readonly Unit @default = new Unit();

        public override int GetHashCode()
        {
            return 0;
        }

        public bool Equals(Unit other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Unit;
        }

        public int CompareTo(Unit obj)
        {
            return 0;
        }

        public override string ToString()
        {
            return "()";
        }

        public static bool operator ==(Unit first, Unit second)
        {
            return true;
        }

        public static bool operator !=(Unit first, Unit second)
        {
            return false;
        }

        public static Unit Default { get { return @default; } }
    }
}