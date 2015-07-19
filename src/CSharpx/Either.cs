using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpx
{
    public enum Either2Type { Either1Of2, Either2Of2 }

    public abstract class Either<T1, T2>
    {
        private readonly Either2Type tag;

        protected Either(Either2Type tag)
        {
            this.tag = tag;
        }

        public Either2Type Tag
        {
            get { return this.tag; }
        }
    }

    public sealed class Either1Of2<T1, T2> : Either<T1, T2>
    {
        private readonly T1 value;

        internal Either1Of2(T1 value)
            : base(Either2Type.Either1Of2)
        {
            this.value = value;
        }

        public T1 Value
        {
            get { return value; }
        }
    }

    public sealed class Either2Of2<T1, T2> : Either<T1, T2>
    {
        private readonly T2 value;

        internal Either2Of2(T2 value)
            : base(Either2Type.Either2Of2)
        {
            this.value = value;
        }

        public T2 Value
        {
            get { return value; }
        }
    }

    public enum Either3Type { Either1Of3, Either2Of3, Either3Of3 }

    public abstract class Either<T1, T2, T3>
    {
        private readonly Either3Type tag;

        protected Either(Either3Type tag)
        {
            this.tag = tag;
        }

        public Either3Type Tag
        {
            get { return this.tag; }
        }
    }

    public sealed class Either1Of3<T1, T2, T3> : Either<T1, T2, T3>
    {
        private readonly T1 value;

        internal Either1Of3(T1 value)
            : base(Either3Type.Either1Of3)
        {
            this.value = value;
        }

        public T1 Value
        {
            get { return value; }
        }
    }

    public sealed class Either2Of3<T1, T2, T3> : Either<T1, T2, T3>
    {
        private readonly T2 value;

        internal Either2Of3(T2 value)
            : base(Either3Type.Either2Of3)
        {
            this.value = value;
        }

        public T2 Value
        {
            get { return value; }
        }
    }

    public sealed class Either3Of3<T1, T2, T3> : Either<T1, T2, T3>
    {
        private readonly T3 value;

        internal Either3Of3(T3 value)
            : base(Either3Type.Either3Of3)
        {
            this.value = value;
        }

        public T3 Value
        {
            get { return value; }
        }
    }

    public enum Either4Type { Either1Of4, Either2Of4, Either3Of4, Either4Of4 }

    public abstract class Either<T1, T2, T3, T4>
    {
        private readonly Either4Type tag;

        protected Either(Either4Type tag)
        {
            this.tag = tag;
        }

        public Either4Type Tag
        {
            get { return this.tag; }
        }
    }

    public sealed class Either1Of4<T1, T2, T3, T4> : Either<T1, T2, T3, T4>
    {
        private readonly T1 value;

        internal Either1Of4(T1 value)
            : base(Either4Type.Either1Of4)
        {
            this.value = value;
        }

        public T1 Value
        {
            get { return value; }
        }
    }

    public sealed class Either2Of4<T1, T2, T3, T4> : Either<T1, T2, T3, T4>
    {
        private readonly T2 value;

        internal Either2Of4(T2 value)
            : base(Either4Type.Either2Of4)
        {
            this.value = value;
        }

        public T2 Value
        {
            get { return value; }
        }
    }

    public sealed class Either3Of4<T1, T2, T3, T4> : Either<T1, T2, T3, T4>
    {
        private readonly T3 value;

        internal Either3Of4(T3 value)
            : base(Either4Type.Either3Of4)
        {
            this.value = value;
        }

        public T3 Value
        {
            get { return value; }
        }
    }

    public sealed class Either4Of4<T1, T2, T3, T4> : Either<T1, T2, T3, T4>
    {
        private readonly T4 value;

        internal Either4Of4(T4 value)
            : base(Either4Type.Either4Of4)
        {
            this.value = value;
        }

        public T4 Value
        {
            get { return value; }
        }
    }

    public enum Either5Type { Either1Of5, Either2Of5, Either3Of5, Either4Of5, Either5Of5 }

    public abstract class Either<T1, T2, T3, T4, T5>
    {
        private readonly Either5Type tag;

        protected Either(Either5Type tag)
        {
            this.tag = tag;
        }

        public Either5Type Tag
        {
            get { return this.tag; }
        }
    }

    public sealed class Either1Of5<T1, T2, T3, T4, T5> : Either<T1, T2, T3, T4, T5>
    {
        private readonly T1 value;

        internal Either1Of5(T1 value)
            : base(Either5Type.Either1Of5)
        {
            this.value = value;
        }

        public T1 Value
        {
            get { return value; }
        }
    }

    public sealed class Either2Of5<T1, T2, T3, T4, T5> : Either<T1, T2, T3, T4, T5>
    {
        private readonly T2 value;

        internal Either2Of5(T2 value)
            : base(Either5Type.Either2Of5)
        {
            this.value = value;
        }

        public T2 Value
        {
            get { return value; }
        }
    }

    public sealed class Either3Of5<T1, T2, T3, T4, T5> : Either<T1, T2, T3, T4, T5>
    {
        private readonly T3 value;

        internal Either3Of5(T3 value)
            : base(Either5Type.Either3Of5)
        {
            this.value = value;
        }

        public T3 Value
        {
            get { return value; }
        }
    }

    public sealed class Either4Of5<T1, T2, T3, T4, T5> : Either<T1, T2, T3, T4, T5>
    {
        private readonly T4 value;

        internal Either4Of5(T4 value)
            : base(Either5Type.Either4Of5)
        {
            this.value = value;
        }

        public T4 Value
        {
            get { return value; }
        }
    }

    public sealed class Either5Of5<T1, T2, T3, T4, T5> : Either<T1, T2, T3, T4, T5>
    {
        private readonly T5 value;

        internal Either5Of5(T5 value)
            : base(Either5Type.Either5Of5)
        {
            this.value = value;
        }

        public T5 Value
        {
            get { return value; }
        }
    }

    public enum Either6Type { Either1Of6, Either2Of6, Either3Of6, Either4Of6, Either5Of6, Either6Of6 }

    public abstract class Either<T1, T2, T3, T4, T5, T6>
    {
        private readonly Either6Type tag;

        protected Either(Either6Type tag)
        {
            this.tag = tag;
        }

        public Either6Type Tag
        {
            get { return this.tag; }
        }
    }

    public sealed class Either1Of6<T1, T2, T3, T4, T5, T6> : Either<T1, T2, T3, T4, T5, T6>
    {
        private readonly T1 value;

        internal Either1Of6(T1 value)
            : base(Either6Type.Either1Of6)
        {
            this.value = value;
        }

        public T1 Value
        {
            get { return value; }
        }
    }

    public sealed class Either2Of6<T1, T2, T3, T4, T5, T6> : Either<T1, T2, T3, T4, T5, T6>
    {
        private readonly T2 value;

        internal Either2Of6(T2 value)
            : base(Either6Type.Either2Of6)
        {
            this.value = value;
        }

        public T2 Value
        {
            get { return value; }
        }
    }

    public sealed class Either3Of6<T1, T2, T3, T4, T5, T6> : Either<T1, T2, T3, T4, T5, T6>
    {
        private readonly T3 value;

        internal Either3Of6(T3 value)
            : base(Either6Type.Either3Of6)
        {
            this.value = value;
        }

        public T3 Value
        {
            get { return value; }
        }
    }

    public sealed class Either4Of6<T1, T2, T3, T4, T5, T6> : Either<T1, T2, T3, T4, T5, T6>
    {
        private readonly T4 value;

        internal Either4Of6(T4 value)
            : base(Either6Type.Either4Of6)
        {
            this.value = value;
        }

        public T4 Value
        {
            get { return value; }
        }
    }

    public sealed class Either5Of6<T1, T2, T3, T4, T5, T6> : Either<T1, T2, T3, T4, T5, T6>
    {
        private readonly T5 value;

        internal Either5Of6(T5 value)
            : base(Either6Type.Either5Of6)
        {
            this.value = value;
        }

        public T5 Value
        {
            get { return value; }
        }
    }

    public sealed class Either6Of6<T1, T2, T3, T4, T5, T6> : Either<T1, T2, T3, T4, T5, T6>
    {
        private readonly T6 value;

        internal Either6Of6(T6 value)
            : base(Either6Type.Either6Of6)
        {
            this.value = value;
        }

        public T6 Value
        {
            get { return value; }
        }
    }

    public enum Either7Type { Either1Of7, Either2Of7, Either3Of7, Either4Of7, Either5Of7, Either6Of7, Either7Of7 }

    public abstract class Either<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly Either7Type tag;

        protected Either(Either7Type tag)
        {
            this.tag = tag;
        }

        public Either7Type Tag
        {
            get { return this.tag; }
        }
    }

    public sealed class Either1Of7<T1, T2, T3, T4, T5, T6, T7> : Either<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly T1 value;

        internal Either1Of7(T1 value)
            : base(Either7Type.Either1Of7)
        {
            this.value = value;
        }

        public T1 Value
        {
            get { return value; }
        }
    }

    public sealed class Either2Of7<T1, T2, T3, T4, T5, T6, T7> : Either<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly T2 value;

        internal Either2Of7(T2 value)
            : base(Either7Type.Either2Of7)
        {
            this.value = value;
        }

        public T2 Value
        {
            get { return value; }
        }
    }

    public sealed class Either3Of7<T1, T2, T3, T4, T5, T6, T7> : Either<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly T3 value;

        internal Either3Of7(T3 value)
            : base(Either7Type.Either3Of7)
        {
            this.value = value;
        }

        public T3 Value
        {
            get { return value; }
        }
    }

    public sealed class Either4Of7<T1, T2, T3, T4, T5, T6, T7> : Either<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly T4 value;

        internal Either4Of7(T4 value)
            : base(Either7Type.Either4Of7)
        {
            this.value = value;
        }

        public T4 Value
        {
            get { return value; }
        }
    }

    public sealed class Either5Of7<T1, T2, T3, T4, T5, T6, T7> : Either<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly T5 value;

        internal Either5Of7(T5 value)
            : base(Either7Type.Either5Of7)
        {
            this.value = value;
        }

        public T5 Value
        {
            get { return value; }
        }
    }

    public sealed class Either6Of7<T1, T2, T3, T4, T5, T6, T7> : Either<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly T6 value;

        internal Either6Of7(T6 value)
            : base(Either7Type.Either6Of7)
        {
            this.value = value;
        }

        public T6 Value
        {
            get { return value; }
        }
    }

    public sealed class Either7Of7<T1, T2, T3, T4, T5, T6, T7> : Either<T1, T2, T3, T4, T5, T6, T7>
    {
        private readonly T7 value;

        internal Either7Of7(T7 value)
            : base(Either7Type.Either7Of7)
        {
            this.value = value;
        }

        public T7 Value
        {
            get { return value; }
        }
    }

    public static class Either
    {
        #region Constructor methods
        public static Either<T1, T2> New1Of2<T1, T2>(T1 value)
        {
            return new Either1Of2<T1, T2>(value);
        }

        public static Either<T1, T2> New2Of2<T1, T2>(T2 value)
        {
            return new Either2Of2<T1, T2>(value);
        }

        public static Either<T1, T2, T3> New1Of3<T1, T2, T3>(T1 value)
        {
            return new Either1Of3<T1, T2, T3>(value);
        }

        public static Either<T1, T2, T3> New2Of3<T1, T2, T3>(T2 value)
        {
            return new Either2Of3<T1, T2, T3>(value);
        }

        public static Either<T1, T2, T3> New3Of3<T1, T2, T3>(T3 value)
        {
            return new Either3Of3<T1, T2, T3>(value);
        }

        public static Either<T1, T2, T3, T4> New1Of4<T1, T2, T3, T4>(T1 value)
        {
            return new Either1Of4<T1, T2, T3, T4>(value);
        }

        public static Either<T1, T2, T3, T4> New2Of4<T1, T2, T3, T4>(T2 value)
        {
            return new Either2Of4<T1, T2, T3, T4>(value);
        }

        public static Either<T1, T2, T3, T4> New3Of4<T1, T2, T3, T4>(T3 value)
        {
            return new Either3Of4<T1, T2, T3, T4>(value);
        }

        public static Either<T1, T2, T3, T4> New4Of4<T1, T2, T3, T4>(T4 value)
        {
            return new Either4Of4<T1, T2, T3, T4>(value);
        }

        public static Either<T1, T2, T3, T4, T5> New1Of5<T1, T2, T3, T4, T5>(T1 value)
        {
            return new Either1Of5<T1, T2, T3, T4, T5>(value);
        }

        public static Either<T1, T2, T3, T4, T5> New2Of5<T1, T2, T3, T4, T5>(T2 value)
        {
            return new Either2Of5<T1, T2, T3, T4, T5>(value);
        }

        public static Either<T1, T2, T3, T4, T5> New3Of5<T1, T2, T3, T4, T5>(T3 value)
        {
            return new Either3Of5<T1, T2, T3, T4, T5>(value);
        }

        public static Either<T1, T2, T3, T4, T5> New4Of5<T1, T2, T3, T4, T5>(T4 value)
        {
            return new Either4Of5<T1, T2, T3, T4, T5>(value);
        }

        public static Either<T1, T2, T3, T4, T5> New5Of5<T1, T2, T3, T4, T5>(T5 value)
        {
            return new Either5Of5<T1, T2, T3, T4, T5>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6> New1Of6<T1, T2, T3, T4, T5, T6>(T1 value)
        {
            return new Either1Of6<T1, T2, T3, T4, T5, T6>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6> New2Of6<T1, T2, T3, T4, T5, T6>(T2 value)
        {
            return new Either2Of6<T1, T2, T3, T4, T5, T6>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6> New3Of6<T1, T2, T3, T4, T5, T6>(T3 value)
        {
            return new Either3Of6<T1, T2, T3, T4, T5, T6>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6> New4Of6<T1, T2, T3, T4, T5, T6>(T4 value)
        {
            return new Either4Of6<T1, T2, T3, T4, T5, T6>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6> New5Of6<T1, T2, T3, T4, T5, T6>(T5 value)
        {
            return new Either5Of6<T1, T2, T3, T4, T5, T6>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6> New6Of6<T1, T2, T3, T4, T5, T6>(T6 value)
        {
            return new Either6Of6<T1, T2, T3, T4, T5, T6>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6, T7> New1Of7<T1, T2, T3, T4, T5, T6, T7>(T1 value)
        {
            return new Either1Of7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6, T7> New2Of7<T1, T2, T3, T4, T5, T6, T7>(T2 value)
        {
            return new Either2Of7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6, T7> New3Of7<T1, T2, T3, T4, T5, T6, T7>(T3 value)
        {
            return new Either3Of7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6, T7> New4Of7<T1, T2, T3, T4, T5, T6, T7>(T4 value)
        {
            return new Either4Of7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6, T7> New5Of7<T1, T2, T3, T4, T5, T6, T7>(T5 value)
        {
            return new Either5Of7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6, T7> New6Of7<T1, T2, T3, T4, T5, T6, T7>(T6 value)
        {
            return new Either6Of7<T1, T2, T3, T4, T5, T6, T7>(value);
        }

        public static Either<T1, T2, T3, T4, T5, T6, T7> New7Of7<T1, T2, T3, T4, T5, T6, T7>(T7 value)
        {
            return new Either7Of7<T1, T2, T3, T4, T5, T6, T7>(value);
        }
        #endregion

        /// <summary>
        /// Inject a value into the Either type.
        /// </summary>
        public static Func<T1, Either<T1, T2>> ReturnM<T1, T2>()
        {
            return value => new Either1Of2<T1, T2>(value);
        }

        /// <summary>
        /// Wraps a function, encapsulates any exception thrown within to a Either.
        /// </summary>
        public static T1 Get<T1, T2>(Either<T1, T2> either)
        {
            if (either.Tag == Either2Type.Either1Of2)
            {
                return ((Either1Of2<T1, T2>)either).Value;
            }
            throw new ArgumentException("either", string.Format("The either value was Either2Of2 {0}", either));
        }

        /// <summary>
        /// Wraps a function, encapsulates any exception thrown within to a Either.
        /// </summary>
        public static Either<T2, Exception> Protect<T1, T2>(Func<T1, T2> func, T1 value)
        {
            try
            {
                return new Either1Of2<T2, Exception>(func(value));
            }
            catch (Exception ex)
            {
                return new Either2Of2<T2, Exception>(ex);
            }
        }

        /// <summary>
        /// Attempts to cast an object.
        /// Stores the cast value in 1Of2 if successful, otherwise stores the exception in 2Of2
        /// </summary>
        public static Either<T1, Exception> Cast<T1>(object obj)
        {
            return Protect(v => (T1)obj, obj);
        }
    }

    public static class EitherExtensions
    {
        public static void Match<T1, T2>(this Either<T1, T2> either, Action<T1> ifFirst, Action<T2> ifSecond)
        {
            if (either.Tag == Either2Type.Either1Of2)
            {
                ifFirst(((Either1Of2<T1, T2>)either).Value);
                return;
            }
            ifSecond(((Either2Of2<T1, T2>)either).Value);
        }

        public static void Match<T1, T2, T3>(this Either<T1, T2, T3> either, Action<T1> ifFirst, Action<T2> ifSecond,
            Action<T3> ifThird)
        {
            if (either.Tag == Either3Type.Either1Of3)
            {
                ifFirst(((Either1Of3<T1, T2, T3>)either).Value);
                return;
            }
            if (either.Tag == Either3Type.Either2Of3)
            {
                ifSecond(((Either2Of3<T1, T2, T3>)either).Value);
                return;
            }
            ifThird(((Either3Of3<T1, T2, T3>)either).Value);
        }

        public static void Match<T1, T2, T3, T4>(this Either<T1, T2, T3, T4> either, Action<T1> ifFirst, Action<T2> ifSecond,
            Action<T3> ifThird, Action<T4> ifFourth)
        {
            if (either.Tag == Either4Type.Either1Of4)
            {
                ifFirst(((Either1Of4<T1, T2, T3, T4>)either).Value);
                return;
            }
            if (either.Tag == Either4Type.Either2Of4)
            {
                ifSecond(((Either2Of4<T1, T2, T3, T4>)either).Value);
                return;
            }
            if (either.Tag == Either4Type.Either3Of4)
            {
                ifThird(((Either3Of4<T1, T2, T3, T4>)either).Value);
                return;
            }
            ifFourth(((Either4Of4<T1, T2, T3, T4>)either).Value);
        }

        public static void Match<T1, T2, T3, T4, T5>(this Either<T1, T2, T3, T4, T5> either, Action<T1> ifFirst, Action<T2> ifSecond,
            Action<T3> ifThird, Action<T4> ifFourth, Action<T5> ifFifth)
        {
            if (either.Tag == Either5Type.Either1Of5)
            {
                ifFirst(((Either1Of5<T1, T2, T3, T4, T5>)either).Value);
                return;
            }
            if (either.Tag == Either5Type.Either2Of5)
            {
                ifSecond(((Either2Of5<T1, T2, T3, T4, T5>)either).Value);
                return;
            }
            if (either.Tag == Either5Type.Either3Of5)
            {
                ifThird(((Either3Of5<T1, T2, T3, T4, T5>)either).Value);
                return;
            }
            if (either.Tag == Either5Type.Either4Of5)
            {
                ifFourth(((Either4Of5<T1, T2, T3, T4, T5>)either).Value);
                return;
            }
            ifFifth(((Either5Of5<T1, T2, T3, T4, T5>)either).Value);
        }

        public static void Match<T1, T2, T3, T4, T5, T6>(this Either<T1, T2, T3, T4, T5, T6> either, Action<T1> ifFirst, Action<T2> ifSecond,
            Action<T3> ifThird, Action<T4> ifFourth, Action<T5> ifFifth, Action<T6> ifSixth)
        {
            if (either.Tag == Either6Type.Either1Of6)
            {
                ifFirst(((Either1Of6<T1, T2, T3, T4, T5, T6>)either).Value);
                return;
            }
            if (either.Tag == Either6Type.Either2Of6)
            {
                ifSecond(((Either2Of6<T1, T2, T3, T4, T5, T6>)either).Value);
                return;
            }
            if (either.Tag == Either6Type.Either3Of6)
            {
                ifThird(((Either3Of6<T1, T2, T3, T4, T5, T6>)either).Value);
                return;
            }
            if (either.Tag == Either6Type.Either4Of6)
            {
                ifFourth(((Either4Of6<T1, T2, T3, T4, T5, T6>)either).Value);
                return;
            }
            if (either.Tag == Either6Type.Either5Of6)
            {
                ifFifth(((Either5Of6<T1, T2, T3, T4, T5, T6>)either).Value);
                return;
            }
            ifSixth(((Either6Of6<T1, T2, T3, T4, T5, T6>)either).Value);
        }

        public static void Match<T1, T2, T3, T4, T5, T6, T7>(this Either<T1, T2, T3, T4, T5, T6, T7> either, Action<T1> ifFirst, Action<T2> ifSecond,
            Action<T3> ifThird, Action<T4> ifFourth, Action<T5> ifFifth, Action<T6> ifSixth,
            Action<T7> ifSeventh)
        {
            if (either.Tag == Either7Type.Either1Of7)
            {
                ifFirst(((Either1Of7<T1, T2, T3, T4, T5, T6, T7>)either).Value);
                return;
            }
            if (either.Tag == Either7Type.Either2Of7)
            {
                ifSecond(((Either2Of7<T1, T2, T3, T4, T5, T6, T7>)either).Value);
                return;
            }
            if (either.Tag == Either7Type.Either3Of7)
            {
                ifThird(((Either3Of7<T1, T2, T3, T4, T5, T6, T7>)either).Value);
                return;
            }
            if (either.Tag == Either7Type.Either4Of7)
            {
                ifFourth(((Either4Of7<T1, T2, T3, T4, T5, T6, T7>)either).Value);
                return;
            }
            if (either.Tag == Either7Type.Either5Of7)
            {
                ifFifth(((Either5Of7<T1, T2, T3, T4, T5, T6, T7>)either).Value);
                return;
            }
            if (either.Tag == Either7Type.Either6Of7)
            {
                ifSixth(((Either6Of7<T1, T2, T3, T4, T5, T6, T7>)either).Value);
                return;
            }
            ifSeventh(((Either7Of7<T1, T2, T3, T4, T5, T6, T7>)either).Value);
        }
    }

}
