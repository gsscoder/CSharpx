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
        /// <summary>Computation case with a value.</summary>
        Just,
        /// <summary>Computation case without a value.</summary>
        Nothing
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
        public MaybeType Tag { get { return _tag; } }

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
        public static Maybe<T2> Bind<T1, T2>(Maybe<T1> maybe, Func<T1, Maybe<T2>> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            return maybe.MatchJust(out T1 value) ? func(value) : Nothing<T2>();
        }
        #endregion

        #region Functor
        /// <summary>Transforms a <c>Maybe</c> value by using a specified mapping function.</summary>
        public static Maybe<T2> Map<T1, T2>(Maybe<T1> maybe, Func<T1, T2> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            return maybe.MatchJust(out T1 value) ? Just(func(value)) : Nothing<T2>();
        }
        #endregion

        /// <summary>If both <c>Maybe</c> values contain a value, it merges them into a <c>Maybe</c>
        /// with a tupled value. </summary>
        public static Maybe<Tuple<T1, T2>> Merge<T1, T2>(Maybe<T1> first, Maybe<T2> second)
        {
            if (first.MatchJust(out T1 value1) && second.MatchJust(out T2 value2)) {
                return Just(Tuple.Create(value1, value2));
            }
            return Nothing<Tuple<T1, T2>>();
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
        public static void Match<T>(this Maybe<T> maybe, Action<T> ifJust, Action ifNothing)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
            if (ifNothing == null) throw new ArgumentNullException(nameof(ifNothing));

            if (maybe.MatchJust(out T value)) {
                ifJust(value);
                return;
            }
            ifNothing();
        }

        /// <summary>Provides pattern matching using <c>System.Action</c> delegates over a <c>Maybe</c>
        /// with tupled wrapped value.</summary>
        public static void Match<T1, T2>(this Maybe<Tuple<T1, T2>> maybe,
            Action<T1, T2> ifJust, Action ifNothing)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
            if (ifNothing == null) throw new ArgumentNullException(nameof(ifNothing));
            if (ifJust == null) throw new ArgumentNullException(nameof(ifJust));

            if (maybe.MatchJust(out T1 value1, out T2 value2)) {
                ifJust(value1, value2);
                return;
            }
            ifNothing();
        }

        /// <summary>Matches a value returning <c>true</c> and the tupled value itself via two output
        /// parameters.</summary>
        public static bool MatchJust<T1, T2>(this Maybe<Tuple<T1, T2>> maybeTuple,
            out T1 value1, out T2 value2)
        {
            if (maybeTuple == null) throw new ArgumentNullException(nameof(maybeTuple));

            if (maybeTuple.MatchJust(out Tuple<T1, T2> value)) {
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
        public static Maybe<T2> Bind<T1, T2>(this Maybe<T1> maybe, Func<T1, Maybe<T2>> func) =>
            Maybe.Bind(maybe, func);

        /// <summary>Transforms a maybe value by using a specified mapping function.</summary>
        public static Maybe<T2> Map<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> func) =>
            Maybe.Map(maybe, func);

        /// <summary>Unwraps a value applying a function o returns another value on fail.</summary>
        public static T2 Return<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> func, T2 @default)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
 
            return maybe.MatchJust(out T1 value) ? func(value) : @default;
        }
        #endregion

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
        public static void Do<T1, T2>(this Maybe<Tuple<T1, T2>> maybe, Action<T1, T2> action)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (maybe.MatchJust(out T1 value1, out T2 value2)) action(value1, value2);
        }
        #endregion

        /// <summary>Returns <c>true</c> if it is in form <c>Just</c>.</summary>
        public static bool IsJust<T>(this Maybe<T> maybe)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            return maybe.Tag == MaybeType.Just;
        }

        /// <summary>Returns <c>true</c> if it is in form of <c>Nothing</c>.</summary>
        public static bool IsNothing<T>(this Maybe<T> maybe)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            return maybe.Tag == MaybeType.Nothing;
        }

        /// <summary>Extracts the element out of <c>Just</c> and returns a default value (or <c>@default</c>
        /// when given) if it is in form of<c>Nothing</c>.</summary>
        public static T FromJust<T>(this Maybe<T> maybe, T @default = default(T))
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            return maybe.MatchJust(out T value) ? value : @default;
        }

        /// <summary>Lazy version of <c>FromJust</c>. Extracts the element out of <c>Just</c> and returns
        /// a default value returned by <c>@default</c> function if it is in form of<c>Nothing</c>.</summary>
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

        /// <summary>Extracts the element out of <c>Just</c> or <c>returns</c> a null if it is form of
        /// <c>Nothing</c>.</summary>
        public static T FromJustOrNull<T>(this Maybe<T> maybe) where T : class
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));

            return maybe.MatchJust(out T value) ? value : null;
        }

        /// <summary>If contains a values executes a mapping function over it, otherwise returns
        /// <c>@default</c>.</summary>
        public static T2 MapValueOrDefault<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> func, T2 @default)
        {
            if (maybe == null) throw new ArgumentNullException(nameof(maybe));
            if (func == null) throw new ArgumentNullException(nameof(func));

            return maybe.MatchJust(out T1 value) ? func(value) : @default;
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

        /// <summary>This is a version of map which can throw out elements. In particular, the functional
        /// argument returns something of type <c>Maybe&lt;T2&gt;</c>. If this is Nothing, no element is
        /// added on to the result sequence. If it is <c>Just&lt;T2&gt;</c>, then <c>T2</c> is included
        /// in the result sequence.</summary>
        public static IEnumerable<T2> Map<T1, T2>(this IEnumerable<T1> source, Func<T1, Maybe<T2>> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return _(); IEnumerable<T2> _()
            {
                foreach (var element in source) {
                    if (func(element).MatchJust(out T2 value)) yield return value;
                }
            }
        }
        #endregion
    }
}