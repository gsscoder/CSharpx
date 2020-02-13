//#define CSX_MAYBE_INTERNAL // Uncomment or define at build time to set accessibility to internal.
//#define CSX_REM_EITHER_FUNC // Uncomment or define at build time to remove dependency to Either.cs.

using System;
using System.Text;
using System.Collections.Generic;

namespace CSharpx
{
    #region Maybe Type
    /// <summary>Discriminator for <c>Maybe</c>.</summary>
#if !CSX_MAYBE_INTERNAL
    public
#endif
    enum MaybeType
    {
        /// <summary>Computation case without a value.</summary>
        Nothing,
        /// <summary>Computation case with a value.</summary>
        Just
    }

    /// <summary>The <c>Maybe</c> type models an optional value. A value of type <c>Maybe</c> either
    /// contains a value (represented as <c>Just</c> a), or it is empty (represented as
    /// <c>Nothing</c>).</summary>
#if !CSX_MAYBE_INTERNAL
    public
#endif
    abstract class Maybe<T>
    {
        readonly MaybeType _tag;

        protected Maybe(MaybeType tag) => _tag = tag;

        /// <summary>Type discriminator. </summary>
        public MaybeType Tag => _tag;

        #region Basic Match Methods
        /// <summary>Matches a value returning <c>true</c> and value itself via an output
        /// parameter.</summary>
        public bool MatchJust(out T value)
        {
            value = Tag == MaybeType.Just ? ((Just<T>)this).Value : default;
            return Tag == MaybeType.Just;
        }

        /// <summary>Matches an empty value returning <c>true</c>.</summary>
        public bool MatchNothing() => Tag == MaybeType.Nothing;
        #endregion
    }
    #endregion

    /// <summary>Models a <c>Maybe</c> when in empty state.</summary>
#if !CSX_MAYBE_INTERNAL
    public
#endif
    sealed class Nothing<T> : Maybe<T>
    {
        internal Nothing() : base(MaybeType.Nothing) { }

        /// <summary>Returns a string that represents the value of this <c>Maybe</c>
        /// instance in form of <c>Nothing</c>.</summary>
        public override string ToString() => "<Nothing>";
    }

    /// <summary>Models a <c>Maybe</c> when contains a value.</summary>
#if !CSX_MAYBE_INTERNAL
    public
#endif
    sealed class Just<T> : Maybe<T>
    {
        readonly T _value;

        internal Just(T value) : base(MaybeType.Just) => _value = value;

        /// <summary>The wrapped value.</summary>
        public T Value => _value;

        /// <summary>Returns a string that represents the value of this <c>Maybe</c>
        /// instance in form of <c>Just</c>.</summary>
        public override string ToString()
        {
            var builder = new StringBuilder("Just(");
            builder.Append(_value);
            builder.Append(")");
            return builder.ToString();
        }
    }

    /// <summary>Provides static methods for manipulating <c>Maybe</c>.</summary>
#if !CSX_MAYBE_INTERNAL
    public
#endif
    static class Maybe
    {
        #region Value Case Constructors
        /// <summary>Builds the empty case of <c>Maybe</c>.</summary>
        public static Maybe<T> Nothing<T>() => new Nothing<T>();

        /// <summary>Builds the case when <c>Maybe</c> contains a value.</summary>
        public static Just<T> Just<T>(T value) => new Just<T>(value);
        #endregion

        #region Monad
        /// <summary>Injects a value into the monadic <c>Maybe</c> type.</summary>
        public static Maybe<T> Return<T>(T value) => Equals(value, default(T)) ? Nothing<T>() : Just(value);

        /// <summary>Sequentially compose two actions, passing any value produced by the first as
        /// an argument to the second.</summary>
        public static Maybe<U> Bind<T, U>(Maybe<T> maybe, Func<T, Maybe<U>> onJust)
        {
            if (onJust == null) throw new ArgumentNullException(nameof(onJust));

            return maybe.MatchJust(out T value) ? onJust(value) : Nothing<U>();
        }
        #endregion

        #region Functor
        /// <summary>Transforms a <c>Maybe</c> value by using a specified mapping function.</summary>
        public static Maybe<U> Map<T, U>(Maybe<T> maybe, Func<T, U> onJust)
        {
            if (onJust == null) throw new ArgumentNullException(nameof(onJust));

            return maybe.MatchJust(out T value) ? Just(onJust(value)) : Nothing<U>();
        }
        #endregion

        /// <summary>If both <c>Maybe</c> values contain a value, it merges them into a <c>Maybe</c>
        /// with a tupled value. </summary>
        public static Maybe<(T, U)> Merge<T, U>(Maybe<T> first, Maybe<U> second)
        {
            if (first.MatchJust(out T value1) && second.MatchJust(out U value2)) {
                return Just((value1, value2));
            }
            return Nothing<(T, U)>();
        }

#if !CSX_REM_EITHER_FUNC
        /// <summary>Maps <c>Either</c> right value to <c>Just</c>, otherwise returns
        /// <c>Nothing</c>.</summary>
        public static Maybe<TRight> FromEither<TLeft, TRight>(Either<TLeft, TRight> either)
        {
            if (either.Tag == EitherType.Right) {
                return Just(((Right<TLeft, TRight>)either).Value);
            }
            return Nothing<TRight>();
        }
#endif
    }

    /// <summary>Provides convenience extension methods for <c>Maybe</c>.</summary>
#if !CSX_MAYBE_INTERNAL
    public
#endif
    static class MaybeExtensions
    {
        #region Alternative Match Methods
        /// <summary>Provides pattern matching using <c>System.Action</c> delegates.</summary>
        public static void Match<T>(this Maybe<T> maybe, Action<T> onJust, Action onNothing)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
            if (onNothing == null) throw new ArgumentNullException(nameof(onNothing));

            if (maybe.MatchJust(out T value)) {
                onJust(value);
                return;
            }
            onNothing();
        }

        /// <summary>Provides pattern matching using <c>System.Action</c> delegates over a <c>Maybe</c>
        /// with tupled wrapped value.</summary>
        public static void Match<T, U>(this Maybe<(T, U)> maybe,
            Action<T, U> onJust, Action onNothing)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
            if (onNothing == null) throw new ArgumentNullException(nameof(onNothing));
            if (onJust == null) throw new ArgumentNullException(nameof(onJust));

            if (maybe.MatchJust(out T value1, out U value2)) {
                onJust(value1, value2);
                return;
            }
            onNothing();
        }

        /// <summary>Matches a value returning <c>true</c> and the tupled value itself via two output
        /// parameters.</summary>
        public static bool MatchJust<T, U>(this Maybe<(T, U)> maybeTuple,
            out T value1, out U value2)
        {
            if (maybeTuple == null) throw new ArgumentNullException(nameof(maybeTuple));

            if (maybeTuple.MatchJust(out (T, U) value)) {
                value1 = value.Item1;
                value2 = value.Item2;
                return true;
            }
            value1 = default;
            value2 = default;
            return false;
        }
        #endregion

        #region Monad
        /// <summary>Equivalent to monadic <c>Return</c> operation. Builds a <c>Just</c> value in case
        /// <c>value</c> is different from its default.
        /// </summary>
        public static Maybe<T> ToMaybe<T>(this T value) => Maybe.Return(value);

        /// <summary>Invokes a function on this maybe value that itself yields a maybe.</summary>
        public static Maybe<U> Bind<T, U>(this Maybe<T> maybe, Func<T, Maybe<U>> onJust) =>
            Maybe.Bind(maybe, onJust);

        /// <summary>Transforms a maybe value by using a specified mapping function.</summary>
        public static Maybe<U> Map<T, U>(this Maybe<T> maybe, Func<T, U> onJust) =>
            Maybe.Map(maybe, onJust);

        /// <summary>Unwraps a value applying a function o returns another value on fail.</summary>
        public static U Return<T, U>(this Maybe<T> maybe, Func<T, U> onJust, U @default)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
 
            return maybe.MatchJust(out T value) ? onJust(value) : @default;
        }
        #endregion

        /// <summary>This is a version of map which can throw out the value. If contains a <c>Just</c>
        /// executes a mapping function over it, in case of <c>Nothing</c> returns <c>@default</c>.</summary>
        public static U Map<T, U>(this Maybe<T> maybe, Func<T, U> onJust, U @default = default(U))
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
            if (onJust == null) throw new ArgumentNullException(nameof(onJust));

            return maybe.MatchJust(out T value) ? onJust(value) : @default;
        }

        #region LINQ Operators
        /// <summary>Map operation compatible with LINQ.</summary>
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> maybe,
            Func<TSource, TResult> selector) => Maybe.Map(maybe, selector);

        /// <summary>Bind operation compatible with LINQ.</summary>
        public static Maybe<TResult> SelectMany<TSource, TValue, TResult>(this Maybe<TSource> maybe,
            Func<TSource, Maybe<TValue>> valueSelector, Func<TSource, TValue, TResult> resultSelector) =>
            maybe
                .Bind(sourceValue =>
                        valueSelector(sourceValue)
                            .Map(resultValue => resultSelector(sourceValue, resultValue)));

        /// <summary>Returns the same Maybe value if the predicate is true, otherwise
        /// <c>Nothing</c>.</summary>
        public static Maybe<TSource> Where<TSource>(this Maybe<TSource> maybe,
            Func<TSource, bool> predicate) 
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            if (maybe.MatchJust(out TSource value)) {
                if (predicate(value)) return maybe;
            }
            return Maybe.Nothing<TSource>();
        }
        #endregion

        #region Do Semantic
        /// <summary>If contains a value executes a <c>System.Action<c> delegate over it.</summary>
        public static void Do<T>(this Maybe<T> maybe, Action<T> action)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (maybe.MatchJust(out T value)) action(value);
        }

        /// <summary>If contans a value executes a <c>System.Action<c> delegate over it.</summary>
        public static void Do<T, U>(this Maybe<(T, U)> maybe, Action<T, U> action)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (maybe.MatchJust(out T value1, out U value2)) action(value1, value2);
        }
        #endregion

        /// <summary>Returns <c>true</c> if it is in form of <c>Nothing</c>.</summary>
        public static bool IsNothing<T>(this Maybe<T> maybe)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            return maybe.Tag == MaybeType.Nothing;
        }

        /// <summary>Returns <c>true</c> if it is in form of <c>Just</c>.</summary>
        public static bool IsJust<T>(this Maybe<T> maybe)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            return maybe.Tag == MaybeType.Just;
        }

        /// <summary>Extracts the element out of <c>Just</c> and returns a default value (or <c>@default</c>
        /// when given) if it is in form of <c>Nothing</c>.</summary>
        public static T FromJust<T>(this Maybe<T> maybe, T @default = default(T))
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            return maybe.MatchJust(out T value) ? value : @default;
        }

        /// <summary>Lazy version of <c>FromJust</c>. Extracts the element out of <c>Just</c> and returns
        /// a default value returned by <c>@default</c> function if it is in form of <c>Nothing</c>.</summary>
        public static T FromJust<T>(this Maybe<T> maybe, Func<T> @default)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            return maybe.MatchJust(out T value) ? value : @default();
        }

        /// <summary>Extracts the element out of <c>Just</c> or throws an exception if it is form of
        /// <c>Nothing</c>.</summary>
        public static T FromJustOrFail<T>(this Maybe<T> maybe, Exception exceptionToThrow = null)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            if (maybe.MatchJust(out T value)) {
                return value;
            }
            throw exceptionToThrow ?? new Exception("The value is empty.");
        }

        #region
        /// <summary>Returns an empty sequence when given <c>Nothing</c> or a singleton sequence in
        /// case of <c>Just</c>.</summary>
        public static IEnumerable<T> ToEnumerable<T>(this Maybe<T> maybe)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            return _(); IEnumerable<T> _()
            {
                if (maybe.MatchJust(out T value)) yield return value;
            }
        }

        /// <summary>Takes a sequence of <c>Maybe</c> and counts all the <c>Nothing</c> values.</summary>
        public static int Nothings<T>(this IEnumerable<Maybe<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var count = 0;
            foreach (var maybe in source) {
                if (maybe.Tag == MaybeType.Just) count++;
            }
            return count;
        }

        /// <summary>Takes a sequence of <c>Maybe</c> and returns a sequence of all the <c>Just</c>
        /// values.</summary>
        public static IEnumerable<T> Justs<T>(this IEnumerable<Maybe<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return _(); IEnumerable<T> _()
            {
                foreach (var maybe in source) {
                    if (maybe.Tag == MaybeType.Just) yield return maybe.FromJust();
                }
            }
        }

        /// <summary>This is a version of map which can throw out elements. In particular, the functional
        /// argument returns something of type <c>Maybe&lt;U&gt;</c>. If this is Nothing, no element is
        /// added on to the result sequence. If it is <c>Just&lt;U&gt;</c>, then <c>U</c> is included
        /// in the result sequence.</summary>
        public static IEnumerable<U> Map<T, U>(this IEnumerable<T> source, Func<T, Maybe<U>> onElement)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return _(); IEnumerable<U> _()
            {
                foreach (var element in source) {
                    if (onElement(element).MatchJust(out U value)) yield return value;
                }
            }
        }
        #endregion
    }
}