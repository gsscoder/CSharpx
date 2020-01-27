//#define CSX_EITHER_INTERNAL // Uncomment or define at build time to set accessibility to internal.
//#define CSX_REM_MAYBE_FUNC // Uncomment or define at build time to remove dependency to Maybe.cs.

using System;

namespace CSharpx
{
    #region Either Type
    /// <summary>Discriminator for <c>Either</c>.</summary>
#if !CSX_EITHER_INTERNAL
    public
#endif
    enum EitherType
    {
        /// <summary>Failed computation case.</summary>
        Left,
        /// <summary>Sccessful computation case.</summary>
        Right
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    /// <summary>The <c>Either</c> type represents values with two possibilities: a value of type
    /// <c>Either</c> T1 T2 is either <c>Left</c> T1 or <c>Right</c> T2. The <c>Either</c> type is
    /// sometimes used to represent a value which is either correct or an error; by convention, the
    /// <c>Left</c> constructor is used to hold an error value and the <c>Right</c> constructor is
    /// used to hold a correct value (mnemonic: "right" also means "correct").</summary>
    abstract class Either<TLeft, TRight>
    {
        readonly EitherType tag;

        protected Either(EitherType tag)
        {
            this.tag = tag;
        }

        public EitherType Tag
        {
            get { return this.tag; }
        }

        #region Basic Match Methods
        /// <summary>Matches a <c>Left</c> value returning <c>true</c> and value itself via an output
        /// parameter.</summary>
        public bool MatchLeft(out TLeft value)
        {
            value = Tag == EitherType.Left ? ((Left<TLeft, TRight>)this).Value : default(TLeft);
            return Tag == EitherType.Left;
        }

        /// <summary>Matches a <c>Right</c> value returning <c>true</c> and value itself via an output
        /// parameter.</summary>
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
        readonly TLeft value;

        internal Left(TLeft value)
            : base(EitherType.Left)
        {
            this.value = value;
        }

        /// <summary>The wrapped value.</summary>
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
        readonly TRight value;

        internal Right(TRight value)
            : base(EitherType.Right)
        {
            this.value = value;
        }

        /// <summary>The wrapped value.</summary>
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
        /// <summary>Builds the <c>Left</c> case of an <c>Either</c> value.</summary>
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            return new Left<TLeft, TRight>(value);
        }

        /// <summary>Builds the <c>Right</c> case of an <c>Either</c> value.</summary>
        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            return new Right<TLeft, TRight>(value);
        }
        #endregion

        #region Monad
        /// <summary>Inject a value into the <c>Either</c> type, returning Right case.</summary>
        public static Either<string, TRight> Return<TRight>(TRight value)
        {
            return Either.Right<string, TRight>(value);
        }

        /// <summary>Monadic bind.</summary>
        public static Either<TLeft, TResult> Bind<TLeft, TRight, TResult>(Either<TLeft, TRight> either, Func<TRight, Either<TLeft, TResult>> func)
        {
            TRight right;
            if (either.MatchRight(out right)) {
                return func(right);
            }
            return Either.Left<TLeft, TResult>(either.GetLeft());
        }
        #endregion

        #region Functor
        /// <summary>Transforms a <c>Either</c> right value by using a specified mapping function.</summary>
        public static Either<TLeft, TResult> Map<TLeft, TRight, TResult>(Either<TLeft, TRight> either, Func<TRight, TResult> func)
        {
            TRight right;
            if (either.MatchRight(out right)) {
                return Either.Right<TLeft, TResult>(func(right));
            }
            return Either.Left<TLeft, TResult>(either.GetLeft());
        }
        #endregion

        #region Bifunctor
        /// <summary>Maps both parts of a Either type. Applies the first function if <c>Either</c>
        /// is <c>Left</c>. Otherwise applies the second function.</summary>
        public static Either<TLeft1, TRight1> Bimap<TLeft, TRight, TLeft1, TRight1>(Either<TLeft, TRight> either, Func<TLeft, TLeft1> mapLeft, Func<TRight, TRight1> mapRight)
        {
            TRight right;
            if (either.MatchRight(out right)) {
                return Either.Right<TLeft1, TRight1>(mapRight(right));
            }
            return Either.Left<TLeft1, TRight1>(mapLeft(either.GetLeft()));
        }
        #endregion

        #region LINQ Operators
        /// <summary>Map operation compatible with LINQ.</summary>
        public static Either<TLeft, TResult> Select<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> either,
            Func<TRight, TResult> selector)
        {
            return Either.Map(either, selector);
        }

        /// <summary>Map operation compatible with LINQ.</summary>
        public static Either<TLeft, TResult> SelectMany<TLeft, TRight, TResult>(this Either<TLeft, TRight> result,
            Func<TRight, Either<TLeft, TResult>> func)
        {
            return Either.Bind(result, func);
        }
        #endregion

        /// <summary>Fail with a message. Not part of mathematical definition of a monad.</summary>
        public static Either<string, TRight> Fail<TRight>(string message)
        {
            throw new Exception(message);
        }

        /// <summary>Returns a <c>Right</c> or fail with an exception.</summary>
        public static TRight GetOrFail<TLeft, TRight>(Either<TLeft, TRight> either)
        {
            TRight value;
            if (either.MatchRight(out value)) {
                return value;
            }
            throw new ArgumentException(nameof(either), string.Format("The either value was Left {0}.", either));
        }

        /// <summary>Returns a <c>Left</c> or a defualt value.</summary>
        public static TLeft GetLeftOrDefault<TLeft, TRight>(Either<TLeft, TRight> either, TLeft @default)
        {
            TLeft value;
            return either.MatchLeft(out value) ? value : @default;
        }

        /// <summary>Returns a <c>Right</c> or a defualt value.</summary>
        public static TRight GetRightOrDefault<TLeft, TRight>(Either<TLeft, TRight> either, TRight @default)
        {
            TRight value;
            return either.MatchRight(out value) ? value : @default;
        }

        /// <summary>Wraps a function, encapsulates any exception thrown within to a <c>Either</c>.</summary>
        public static Either<Exception, TRight> Try<TRight>(Func<TRight> func)
        {
            try {
                return new Right<Exception, TRight>(func());
            }
            catch (Exception ex) {
                return new Left<Exception, TRight>(ex);
            }
        }

        /// <summary>Attempts to cast an object. Stores the cast value in <c>Right</c> if successful, otherwise
        /// stores the exception in <c>Left</c>.</summary>
        public static Either<Exception, TRight> Cast<TRight>(object obj)
        {
            return Either.Try(() => (TRight)obj);
        }

#if !CSX_REM_MAYBE_FUNC
        /// <summary>Converts a <c>Just</c> value to a <c>Right</c> and a <c>Nothing</c> value to a
        /// <c>Left</c>.</summary>
        public static Either<TLeft, TRight> FromMaybe<TLeft, TRight>(Maybe<TRight> maybe, TLeft left)
        {
            if (maybe.Tag == MaybeType.Just) {
                return Either.Right<TLeft, TRight>(((Just<TRight>)maybe).Value);
            }
            return Either.Left<TLeft, TRight>(left);
        }
#endif

        static TLeft GetLeft<TLeft, TRight>(this Either<TLeft, TRight> either)
        {
            return ((Left<TLeft, TRight>)either).Value;
        }
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    static class EitherExtensions
    {
        #region Alternative Match Methods
        public static void Match<TLeft, TRight>(this Either<TLeft, TRight> either, Action<TLeft> ifLeft, Action<TRight> ifRight)
        {
            TLeft left;
            if (either.MatchLeft(out left)) {
                ifLeft(left);
                return;
            }
            ifRight(((Right<TLeft, TRight>)either).Value);
        }
        #endregion

        /// <summary>
        /// Equivalent to monadic <see cref="CSharpx.Either.Return{TLeft, TRight}"/> operation.
        /// Builds a <see cref="CSharpx.Right{TLeft, TRight}"/> value in case <paramref name="value"/> by default.
        /// </summary>
        public static Either<string, TRight> ToEither<TRight>(this TRight value)
        {
            return Either.Return<TRight>(value);
        }

        public static Either<TLeft, TResult> Bind<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> either,
            Func<TRight, Either<TLeft, TResult>> func)
        {
            return Either.Bind(either, func);
        }

        public static Either<TLeft, TResult> Map<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> either,
            Func<TRight, TResult> func)
        {
            return Either.Map(either, func);
        }

        public static Either<TLeft1, TRight1> Bimap<TLeft, TRight, TLeft1, TRight1>(
            this Either<TLeft, TRight> either,
            Func<TLeft, TLeft1> mapLeft,
            Func<TRight, TRight1> mapRight)
        {
            return Either.Bimap(either, mapLeft, mapRight);
        }

        public static bool IsLeft<TLeft, TRight>(this Either<TLeft, TRight> either)
        {
            return either.Tag == EitherType.Left;
        }

        public static bool IsRight<TLeft, TRight>(this Either<TLeft, TRight> either)
        {
            return either.Tag == EitherType.Right;
        }
    }
}