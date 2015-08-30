//Use project level define(s) when referencing with Paket.
//#define CSX_EITHER_INTERNAL // Uncomment this to set visibility to internal.
//#define CSX_REM_MAYBE_FUNC // Uncomment this to remove dependency to Maybe.cs.

using System;

namespace CSharpx
{
#if !CSX_EITHER_INTERNAL
    public
#endif
    enum EitherType { Left, Right }

#if !CSX_EITHER_INTERNAL
    public
#endif
    abstract class Either<T1, T2>
    {
        private readonly EitherType tag;

        protected Either(EitherType tag)
        {
            this.tag = tag;
        }

        public EitherType Tag
        {
            get { return this.tag; }
        }
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    sealed class Left<T1, T2> : Either<T1, T2>
    {
        private readonly T1 value;

        internal Left(T1 value)
            : base(EitherType.Left)
        {
            this.value = value;
        }

        public T1 Value
        {
            get { return value; }
        }
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    sealed class Right<T1, T2> : Either<T1, T2>
    {
        private readonly T2 value;

        internal Right(T2 value)
            : base(EitherType.Right)
        {
            this.value = value;
        }

        public T2 Value
        {
            get { return value; }
        }
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    static class Either
    {
        #region Value Case Constructors
        public static Either<T1, T2> Left<T1, T2>(T1 value)
        {
            return new Left<T1, T2>(value);
        }

        public static Either<T1, T2> Right<T1, T2>(T2 value)
        {
            return new Right<T1, T2>(value);
        }
        #endregion

        /// <summary>
        /// Inject a value into the Either type.
        /// </summary>
        public static Func<T1, Either<T1, T2>> ReturnM<T1, T2>()
        {
            return value => new Left<T1, T2>(value);
        }

        /// <summary>
        /// Wraps a function, encapsulates any exception thrown within to a Either.
        /// </summary>
        public static T1 Get<T1, T2>(Either<T1, T2> either)
        {
            if (either.Tag == EitherType.Left)
            {
                return ((Left<T1, T2>)either).Value;
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
                return new Left<T2, Exception>(func(value));
            }
            catch (Exception ex)
            {
                return new Right<T2, Exception>(ex);
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

        /// <summary>
        /// Sequential application.
        /// </summary>
        public static Either<T3, T2> Ap<T1, T2, T3>(Either<T1, T2> value, Either<Func<T1, T3>, T2> func)
        {
            if (func.Tag == EitherType.Left && value.Tag == EitherType.Left)
            {
                var f = (Left<Func<T1, T3>, T2>)func;
                var x = (Left<T1, T2>)value;
                return new Left<T3, T2>(f.Value(x.Value));
            }
            if (func.Tag == EitherType.Right)
            {
                var e = (Right<Func<T1, T3>, T2>)func;
                return new Right<T3, T2>(e.Value);
            }
            var g = (Right<T1, T2>)value;
            return new Right<T3, T2>(g.Value);
        }

        /// <summary>
        /// Transforms a Either's first value by using a specified mapping function.
        /// </summary>
        public static Either<T2, T3> Map<T1, T2, T3>(Func<T1, T2> func, Either<T1, T3> either)
        {
            if (either.Tag == EitherType.Left)
            {
                var x = (Left<T1, T3>)either;
                return new Left<T2, T3>(func(x.Value));
            }
            var y = (Right<T1, T3>)either;
            return new Right<T2, T3>(y.Value);
        }

        /// <summary>
        /// Monadic bind.
        /// </summary>
        public static Either<T2, T3> Bind<T1, T2, T3>(Func<T1, Either<T2, T3>> func, Either<T1, T3> either)
        {
            if (either.Tag == EitherType.Left)
            {
                var x = (Left<T1, T3>)either;
                return func(x.Value);
            }
            var y = (Right<T1, T3>)either;
            return new Right<T2, T3>(y.Value);
        }

        /// <summary>
        /// Maps both parts of a Either type.
        /// Applies the first function if Either is 1Of2.
        /// Otherwise applies the second function.
        /// </summary>
        public static Either<T2, T4> Bimap<T1, T2, T3, T4>(Func<T1, T2> func1, Func<T3, T4> func2, Either<T1, T3> either)
        {
            if (either.Tag == EitherType.Left)
            {
                var x = (Left<T1, T3>)either;
                return new Left<T2, T4>(func1(x.Value));
            }
            var y = (Right<T1, T3>)either;
            return new Right<T2, T4>(func2(y.Value));
        }

        /// <summary>
        /// Maps both parts of a Either.
        /// Applies the first function if Either is 1Of2.
        /// Otherwise applies the second function
        /// </summary>
        public static T2 Choice<T1, T2, T3>(Func<T1, T2> func1, Func<T3, T2> func2, Either<T1, T3> either)
        {
            if (either.Tag == EitherType.Left)
            {
                var x = (Left<T1, T3>)either;
                return func1(x.Value);
            }
            var y = (Right<T1, T3>)either;
            return func2(y.Value);
        }

#if !CSX_REM_MAYBE_FUNC
        public static Either<T1, T2> OfMaybe<T1, T2>(Maybe<T1> maybe, T2 second)
        {
            if (maybe.Tag == MaybeType.Just)
            {
                return new Left<T1, T2>(((Just<T1>)maybe).Value);
            }
            return new Right<T1, T2>(second);
        }
#endif
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    static class EitherExtensions
    {
        public static void Match<T1, T2>(this Either<T1, T2> either, Action<T1> ifFirst, Action<T2> ifSecond)
        {
            if (either.Tag == EitherType.Left)
            {
                ifFirst(((Left<T1, T2>)either).Value);
                return;
            }
            ifSecond(((Right<T1, T2>)either).Value);
        }
    }
}
