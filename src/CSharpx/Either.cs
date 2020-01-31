//#define CSX_EITHER_INTERNAL // Uncomment or define at build time to set accessibility to internal.
//#define CSX_REM_MAYBE_FUNC // Uncomment or define at build time to remove dependency to Maybe.cs.

using System;
using System.Collections.Generic;
using System.Linq;

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
        readonly EitherType _tag;

        protected Either(EitherType tag) => _tag = tag;

        public EitherType Tag => _tag;

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
        readonly TLeft _value;

        internal Left(TLeft value) : base(EitherType.Left) => _value = value;

        /// <summary>The wrapped value.</summary>
        public TLeft Value => _value;
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    sealed class Right<TLeft, TRight> : Either<TLeft, TRight>
    {
        readonly TRight _value;

        internal Right(TRight value) : base(EitherType.Right) => _value = value;

        /// <summary>The wrapped value.</summary>
        public TRight Value => _value;
    }
    #endregion

#if !CSX_EITHER_INTERNAL
    public
#endif
    static class Either
    {
        #region Value Case Constructors
        /// <summary>Builds the <c>Left</c> case of an <c>Either</c> value.</summary>
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value) =>
            new Left<TLeft, TRight>(value);

        /// <summary>Builds the <c>Right</c> case of an <c>Either</c> value.</summary>
        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value) =>
            new Right<TLeft, TRight>(value);
        #endregion

        #region Monad
        /// <summary>Inject a value into the <c>Either</c> type, returning Right case.</summary>
        public static Either<string, TRight> Return<TRight>(TRight value) =>
            Either.Right<string, TRight>(value);

        /// <summary>Monadic bind.</summary>
        public static Either<TLeft, TResult> Bind<TLeft, TRight, TResult>(
            Either<TLeft, TRight> either, Func<TRight, Either<TLeft, TResult>> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (either.MatchRight(out TRight right)) {
                return func(right);
            }
            return Either.Left<TLeft, TResult>(either.GetLeft());
        }
        #endregion

        #region Functor
        /// <summary>Transforms a <c>Either</c> right value by using a specified mapping function.</summary>
        public static Either<TLeft, TResult> Map<TLeft, TRight, TResult>(Either<TLeft, TRight> either,
            Func<TRight, TResult> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            if (either.MatchRight(out TRight right)) {
                return Either.Right<TLeft, TResult>(func(right));
            }
            return Either.Left<TLeft, TResult>(either.GetLeft());
        }
        #endregion

        #region Bifunctor
        /// <summary>Maps both parts of a Either type. Applies the first function if <c>Either</c>
        /// is <c>Left</c>. Otherwise applies the second function.</summary>
        public static Either<TLeft1, TRight1> Bimap<TLeft, TRight, TLeft1, TRight1>(Either<TLeft, TRight> either,
            Func<TLeft, TLeft1> mapLeft, Func<TRight, TRight1> mapRight)
        {
            if (mapLeft == null) throw new ArgumentNullException(nameof(mapLeft));
            if (mapRight == null) throw new ArgumentNullException(nameof(mapRight));

            if (either.MatchRight(out TRight right)) {
                return Either.Right<TLeft1, TRight1>(mapRight(right));
            }
            return Either.Left<TLeft1, TRight1>(mapLeft(either.GetLeft()));
        }
        #endregion

        /// <summary>Fail with a message. Not part of mathematical definition of a monad.</summary>
        public static Either<string, TRight> Fail<TRight>(string message) => throw new Exception(message);

        /// <summary>Returns a <c>Left</c> or a defualt value.</summary>
        public static TLeft GetLeftOrDefault<TLeft, TRight>(Either<TLeft, TRight> either, TLeft @default) =>
            either.MatchLeft(out TLeft value) ? value : @default;

        /// <summary>Returns a <c>Right</c> or a defualt value.</summary>
        public static TRight GetRightOrDefault<TLeft, TRight>(Either<TLeft, TRight> either, TRight @default) =>
            either.MatchRight(out TRight value) ? value : @default;

        /// <summary>Wraps a function, encapsulates any exception thrown within to a <c>Either</c>.</summary>
        public static Either<Exception, TRight> Try<TRight>(Func<TRight> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            try {
                return new Right<Exception, TRight>(func());
            }
            catch (Exception ex) {
                return new Left<Exception, TRight>(ex);
            }
        }

        /// <summary>Attempts to cast an object. Stores the cast value in <c>Right</c> if successful, otherwise
        /// stores the exception in <c>Left</c>.</summary>
        public static Either<Exception, TRight> Cast<TRight>(object obj) => Either.Try(() => (TRight)obj);

#if !CSX_REM_MAYBE_FUNC
        /// <summary>Converts a <c>Just</c> value to a <c>Right</c> and a <c>Nothing</c> value to a
        /// <c>Left</c>.</summary>
        public static Either<TLeft, TRight> FromMaybe<TLeft, TRight>(Maybe<TRight> maybe, TLeft left)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            if (maybe.Tag == MaybeType.Just) {
                return Either.Right<TLeft, TRight>(((Just<TRight>)maybe).Value);
            }
            return Either.Left<TLeft, TRight>(left);
        }
#endif

        static TLeft GetLeft<TLeft, TRight>(this Either<TLeft, TRight> either)
        { 
            if (either == null) throw new ArgumentNullException(nameof(either));

            return ((Left<TLeft, TRight>)either).Value;
        }
    }

#if !CSX_EITHER_INTERNAL
    public
#endif
    static class EitherExtensions
    {
        #region LINQ Operators
        /// <summary>Map operation compatible with LINQ.</summary>
        public static Either<TLeft, TResult> Select<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> either,
            Func<TRight, TResult> selector) => Either.Map(either, selector);

        /// <summary>Map operation compatible with LINQ.</summary>
        public static Either<TLeft, TResult> SelectMany<TLeft, TRight, TResult>(this Either<TLeft, TRight> result,
            Func<TRight, Either<TLeft, TResult>> func) => Either.Bind(result, func);
        #endregion

        #region Alternative Match Methods
        public static void Match<TLeft, TRight>(this Either<TLeft, TRight> either,
            Action<TLeft> ifLeft, Action<TRight> ifRight)
        {
            if (either == null) throw new ArgumentNullException(nameof(either));
            if (ifLeft == null) throw new ArgumentNullException(nameof(ifLeft));
            if (ifRight == null) throw new ArgumentNullException(nameof(ifRight));

            if (either.MatchLeft(out TLeft left)) {
                ifLeft(left);
                return;
            }
            ifRight(((Right<TLeft, TRight>)either).Value);
        }
        #endregion

        /// <summary>Equivalent to monadic <c>Return</c> operation. Builds a <c>Right</c> value
        /// by default.</summary>
        public static Either<string, TRight> ToEither<TRight>(this TRight value) => Either.Return<TRight>(value);

        /// <summary>Equivalent to monadic Bind.</summary>
        public static Either<TLeft, TResult> Bind<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> either,
            Func<TRight, Either<TLeft, TResult>> func) => Either.Bind(either, func);

        /// <summary>Equivalent to monadic Map.</summary>
        public static Either<TLeft, TResult> Map<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> either,
            Func<TRight, TResult> func) => Either.Map(either, func);

        /// <summary>Eviqualent to monadic Bimap.</summary>
        public static Either<TLeft1, TRight1> Bimap<TLeft, TRight, TLeft1, TRight1>(
            this Either<TLeft, TRight> either,
            Func<TLeft, TLeft1> mapLeft,
            Func<TRight, TRight1> mapRight) => Either.Bimap(either, mapLeft, mapRight);

        /// <summary>Returns <c>true</c> if it is in form of <c>Left</c>.</summary>
        public static bool IsLeft<TLeft, TRight>(this Either<TLeft, TRight> either)
        {
            if (either == null) throw new ArgumentNullException(nameof(either));

            return either.Tag == EitherType.Left;
        }

        /// <summary>Returns <c>true</c> if it is in form of <c>Right</c>.</summary>
        public static bool IsRight<TLeft, TRight>(this Either<TLeft, TRight> either)
        {
            if (either == null) throw new ArgumentNullException(nameof(either));

            return either.Tag == EitherType.Right;
        }

        /// <summary>Extracts the element out of <c>Left</c> and returns a default value (or <c>noneValue</c>
        /// when given) if it is in form of <c>Right</c>.</summary>
        public static TLeft FromLeft<TLeft, TRight>(this Either<TLeft, TRight> either,
            TLeft noneValue = default(TLeft))
        {
            if (either == null) throw new ArgumentNullException(nameof(either));

            return either.MatchLeft(out TLeft value) ? value : noneValue;
        }

        /// <summary>Extracts the element out of <c>Left</c> and throws an exception if it is form of
        /// <c>Right</c>.</summary>
        public static TLeft FromLeftOrFail<TLeft, TRight>(this Either<TLeft, TRight> either,
            Exception exceptionToThrow = null)
        {
            if (either == null) throw new ArgumentNullException(nameof(either));

            if (either.MatchLeft(out TLeft value)) {
                return value;
            }
            throw exceptionToThrow ?? new Exception("The value is empty.");
        }

        /// <summary>Extracts the element out of <c>Left</c> and returns a default (or <c>noneValue</c>
        /// when given) value if it is in form of<c>Right</c>.</summary>
        public static TRight FromRight<TLeft, TRight>(this Either<TLeft, TRight> either,
            TRight noneValue = default(TRight))
        {
            if (either == null) throw new ArgumentNullException(nameof(either));

            return either.MatchRight(out TRight value) ? value : noneValue;
        }

        /// <summary>Extracts the element out of <c>Left</c> and throws an exception if it is form of
        /// <c>Right</c>.</summary>
        public static TRight FromRightOrFail<TLeft, TRight>(this Either<TLeft, TRight> either,
            Exception exceptionToThrow = null)
        {
            if (either == null) throw new ArgumentNullException(nameof(either));

            if (either.MatchRight(out TRight value)) {
                return value;
            }
            throw exceptionToThrow ?? new Exception("The value is empty.");
        }

        #region IEnumerable
        /// <summary>Extracts from a sequence of <c>Either</c> all the <c>Left</c> elements. All the
        /// <c>Left</c> elements are extracted in order.</summary>
        public static IEnumerable<TLeft> Lefts<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return _(); IEnumerable<TLeft> _()
            {
                foreach (var either in source) {
                    if (either.Tag == EitherType.Left) yield return either.FromLeft();
                }
            }
        }

        /// <summary>Extracts from a sequence of <c>Either</c> all the <c>Right</c> elements. All the
        /// <c>Rights</c> elements are extracted in order.</summary>
        public static IEnumerable<TRight> Rights<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return _(); IEnumerable<TRight> _()
            {
                foreach (var either in source) {
                    if (either.Tag == EitherType.Right) yield return either.FromRight();
                }
            }
        }       

        /// <summary>Partitions a sequence of <c>Either</c> into two sequences. All the <c>Left</c>
        /// elements are extracted, in order, to the first component of the pair. Similarly the <c>Right</c>
        /// elements are extracted to the second component of the pair.</summary>
        public static Pair<IEnumerable<TLeft>, IEnumerable<TRight>> Partition<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var lefts = new List<TLeft>();
            var rights = new List<TRight>();

            foreach (var either in source) {
                if (either.Tag == EitherType.Left) lefts.Add(either.FromLeft());
                else rights.Add(either.FromRight());
            }
            return new Pair<IEnumerable<TLeft>, IEnumerable<TRight>>(lefts, rights);
        }
        #endregion
    }
}