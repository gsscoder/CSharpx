//Use project level define(s) when referencing with Paket.
//#define CSX_EITHER_INTERNAL // Uncomment this to set visibility to internal.
//#define CSX_REM_MAYBE_FUNC // Uncomment this to remove dependency to Maybe.cs.

using System;

namespace CSharpx
{
    #region Either Type
#if !CSX_EITHER_INTERNAL
    public
#endif
    enum EitherType { Left, Right }

#if !CSX_EITHER_INTERNAL
    public
#endif
    abstract class Either<TLeft, TRight>
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

        #region Basic Match Methods
        public bool MatchLeft(out TLeft value)
        {
            value = Tag == EitherType.Left ? ((Left<TLeft, TRight>)this).Value : default(TLeft);
            return Tag == EitherType.Left;
        }

        public bool MatchRight(out TRight value)
        {
            value = Tag == EitherType.Right ? ((Right<TLeft, TRight>)this).Value : default(TRight);
            return Tag == EitherType.Right;
        }
        #endregion
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    sealed class Left<TLeft, TRight> : Either<TLeft, TRight>
    {
        private readonly TLeft value;

        internal Left(TLeft value)
            : base(EitherType.Left)
        {
            this.value = value;
        }

        public TLeft Value
        {
            get { return value; }
        }
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    sealed class Right<TLeft, TRight> : Either<TLeft, TRight>
    {
        private readonly TRight value;

        internal Right(TRight value)
            : base(EitherType.Right)
        {
            this.value = value;
        }

        public TRight Value
        {
            get { return value; }
        }
    }
    #endregion

#if !CSX_EITHER_INTERNAL
    public
#endif
    static class Either
    {
        #region Value Case Constructors
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            return new Left<TLeft, TRight>(value);
        }

        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            return new Right<TLeft, TRight>(value);
        }
        #endregion

        /// <summary>
        /// Inject a value into the Either type.
        /// </summary>
        public static Either<TLeft, TRight> Return<TLeft, TRight>(TRight value)
        {
            return Either.Right<TLeft, TRight>(value);
        }

        /// <summary>
        /// Wraps a function, encapsulates any exception thrown within to a Either.
        /// </summary>
        public static TLeft Get<TLeft, TRight>(Either<TLeft, TRight> either)
        {
            if (either.Tag == EitherType.Left)
            {
                return ((Left<TLeft, TRight>)either).Value;
            }
            throw new ArgumentException("either", string.Format("The either value was Either2Of2 {0}", either));
        }

        /// <summary>
        /// Wraps a function, encapsulates any exception thrown within to a Either.
        /// </summary>
        public static Either<TRight, Exception> Protect<TLeft, TRight>(Func<TLeft, TRight> func, TLeft value)
        {
            try
            {
                return new Left<TRight, Exception>(func(value));
            }
            catch (Exception ex)
            {
                return new Right<TRight, Exception>(ex);
            }
        }

        /// <summary>
        /// Attempts to cast an object.
        /// Stores the cast value in 1Of2 if successful, otherwise stores the exception in 2Of2
        /// </summary>
        public static Either<TLeft, Exception> Cast<TLeft>(object obj)
        {
            return Protect(v => (TLeft)obj, obj);
        }

        /// <summary>
        /// Sequential application.
        /// </summary>
        public static Either<T3, TRight> Ap<TLeft, TRight, T3>(Either<TLeft, TRight> value, Either<Func<TLeft, T3>, TRight> func)
        {
            if (func.Tag == EitherType.Left && value.Tag == EitherType.Left)
            {
                var f = (Left<Func<TLeft, T3>, TRight>)func;
                var x = (Left<TLeft, TRight>)value;
                return new Left<T3, TRight>(f.Value(x.Value));
            }
            if (func.Tag == EitherType.Right)
            {
                var e = (Right<Func<TLeft, T3>, TRight>)func;
                return new Right<T3, TRight>(e.Value);
            }
            var g = (Right<TLeft, TRight>)value;
            return new Right<T3, TRight>(g.Value);
        }

        /// <summary>
        /// Transforms a Either's first value by using a specified mapping function.
        /// </summary>
        public static Either<TRight, T3> Map<TLeft, TRight, T3>(Func<TLeft, TRight> func, Either<TLeft, T3> either)
        {
            if (either.Tag == EitherType.Left)
            {
                var x = (Left<TLeft, T3>)either;
                return new Left<TRight, T3>(func(x.Value));
            }
            var y = (Right<TLeft, T3>)either;
            return new Right<TRight, T3>(y.Value);
        }

        /// <summary>
        /// Monadic bind.
        /// </summary>
        public static Either<TRight, T3> Bind<TLeft, TRight, T3>(Func<TLeft, Either<TRight, T3>> func, Either<TLeft, T3> either)
        {
            if (either.Tag == EitherType.Left)
            {
                var x = (Left<TLeft, T3>)either;
                return func(x.Value);
            }
            var y = (Right<TLeft, T3>)either;
            return new Right<TRight, T3>(y.Value);
        }

        /// <summary>
        /// Maps both parts of a Either type.
        /// Applies the first function if Either is 1Of2.
        /// Otherwise applies the second function.
        /// </summary>
        public static Either<TRight, T4> Bimap<TLeft, TRight, T3, T4>(Func<TLeft, TRight> func1, Func<T3, T4> func2, Either<TLeft, T3> either)
        {
            if (either.Tag == EitherType.Left)
            {
                var x = (Left<TLeft, T3>)either;
                return new Left<TRight, T4>(func1(x.Value));
            }
            var y = (Right<TLeft, T3>)either;
            return new Right<TRight, T4>(func2(y.Value));
        }

        /// <summary>
        /// Maps both parts of a Either.
        /// Applies the first function if Either is 1Of2.
        /// Otherwise applies the second function
        /// </summary>
        public static TRight Choice<TLeft, TRight, T3>(Func<TLeft, TRight> func1, Func<T3, TRight> func2, Either<TLeft, T3> either)
        {
            if (either.Tag == EitherType.Left)
            {
                var x = (Left<TLeft, T3>)either;
                return func1(x.Value);
            }
            var y = (Right<TLeft, T3>)either;
            return func2(y.Value);
        }

#if !CSX_REM_MAYBE_FUNC
        public static Either<TLeft, TRight> OfMaybe<TLeft, TRight>(Maybe<TLeft> maybe, TRight second)
        {
            if (maybe.Tag == MaybeType.Just)
            {
                return new Left<TLeft, TRight>(((Just<TLeft>)maybe).Value);
            }
            return new Right<TLeft, TRight>(second);
        }
#endif
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    static class EitherExtensions
    {
        public static void Match<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TLeft> ifFirst, Action<TRight> ifSecond)
        {
            if (either.Tag == EitherType.Left)
            {
                ifFirst(((Left<TLeft, TRight>)either).Value);
                return;
            }
            ifSecond(((Right<TLeft, TRight>)either).Value);
        }
    }
}
