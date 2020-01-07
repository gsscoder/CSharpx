//#define CSX_FSHARP_DISABLED // Uncomment or define at build time to remove all F# related types.
//#define CSX_FSHARP_RESEXT_INTERNAL // Uncomment or define at build time to set FSharpResultExtensions accessibility to internal.
#if !CSX_FSHARP_DISABLED
using System;
using Microsoft.FSharp.Core;

namespace CSharpx.FSharp
{
#if !CSX_FSHARP_INTERNAL
    public
#endif
    static class FSharpResultExtensions
    {
        /// <summary>
        /// Allows pattern matching on Results.
        /// </summary>
        public static void Match<T, TError>(this FSharpResult<T, TError> result,
            Action<T> onOk,
            Action<TError> onError)
        {
            if (onOk == null) throw new ArgumentNullException(nameof(onOk));
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            if (result.IsOk) {
                onOk(result.ResultValue);
                return;
            }
            onError(result.ErrorValue);
        }

        /// <summary>
        /// Allows pattern matching on Results.
        /// </summary>
        public static TResult Either<T, TError, TResult>(this FSharpResult<T, TError> result,
            Func<T, TResult> onOk,
            Func<TError, TResult> onError)
        {
            if (onOk == null) throw new ArgumentNullException(nameof(onOk));
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            return Trail.Either(onOk, onError, result);
        }

        /// <summary>
        /// Lifts a Func into a Result and applies it on the given result.
        /// </summary>
        public static FSharpResult<TResult, TError> Map<T, TError, TResult>(this FSharpResult<T, TError> result,
            Func<T, TResult> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));

            return Trail.Lift(func, result);
        }

        static class Trail
        {
            // Takes a result and maps it with okFunc if it is a success, otherwise it maps it with errorFunc.
            public static TResult Either<T, TError, TResult>(
                Func<T, TResult> okFunc,
                Func<TError, TResult> errorFunc,
                FSharpResult<T, TError> result)
            {
                if (result.IsOk) {
                    return okFunc(result.ResultValue);
                }
                return errorFunc(result.ErrorValue);
            }

            // If the wrapped function is a success and the given result is a success the function is applied on the value. 
            // Otherwise the exisiting error is returned.
            public static FSharpResult<T, TError> Apply<TValue, T, TError>(
                FSharpResult<Func<TValue, T>, TError> wrappedFunc,
                FSharpResult<TValue, TError> result
            )
            {
                if (wrappedFunc.IsOk && result.IsOk) {
                    return FSharpResult<T, TError>.NewOk(
                        wrappedFunc.ResultValue(result.ResultValue));
                }
                return FSharpResult<T, TError>.NewError(result.ErrorValue);
            }

            // Lifts a function into a result container and applies it on the given result.
            public static FSharpResult<T, TError> Lift<TValue, T, TError>(
                Func<TValue, T> func,
                FSharpResult<TValue, TError> result)
            {
                return Apply(FSharpResult<Func<TValue, T>, TError>.NewOk(func), result);
            }
        }
    }
}
#endif