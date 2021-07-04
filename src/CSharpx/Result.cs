//requires: ExceptionExtensions.cs, Unit.cs, Maybe.cs
//#define CSX_TYPES_INTERNAL // Uncomment or define at build time to set accessibility to internal.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CSharpx
{

#if !CSX_TYPES_INTERNAL
    public
#endif
    enum ResultType
    {
        Success,
        Failure
    }

#if !CSX_TYPES_INTERNAL
    public
#endif   
    struct Error : IEquatable<Error>
    {
        readonly Lazy<ExceptionEqualityComparer> _comparer => new Lazy<ExceptionEqualityComparer>(
            () => new ExceptionEqualityComparer());
        Exception _exception;
        public string Message { get; private set; }
        public Maybe<Exception> Exception => _exception.ToMaybe();

        internal Error(string message, Exception exception)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            Message = message;
            _exception = exception;
        }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (other.GetType() != typeof(Error)) return false;
            var otherError = (Error)other;
            return otherError.Message.Equals(Message) &&
                   _comparer.Value.Equals(otherError._exception, _exception);
        }

        public bool Equals(Error other) =>
            other.Message.Equals(Message)  &&
                    _comparer.Value.Equals(other._exception, _exception);

        public static bool operator ==(Error left, Error right) => left.Equals(right);

        public static bool operator !=(Error left, Error right) => !left.Equals(right);

        public override int GetHashCode() =>
            _exception == null
                ? Message.GetHashCode()
                : Message.GetHashCode() ^ _exception.GetHashCode();

        public override string ToString() => Exception.IsJust()
            ? new StringBuilder(capacity: 256)
                .AppendLine($"{Message}:")
                .AppendLine(Exception.FromJust().ToStringEx())
                .ToString()
            : Message;

        sealed class ExceptionEqualityComparer : IEqualityComparer<Exception>
        {
            public bool Equals(Exception first, Exception second)
            {
                if (first == null && second == null) return true;
                if (first == null || second == null) return false;
                if (first.GetType() != second.GetType()) return false;
                if (first.Message != second.Message) return false;
                if (first.InnerException != null) return Equals(first.InnerException, second.InnerException);
                return true;
            }

            public int GetHashCode(Exception exception)
            {
                var hash = exception.Message.GetHashCode();
                if (exception.InnerException != null) hash ^= exception.InnerException.Message.GetHashCode();
                return hash;
            }
        }
    }

#if !CSX_TYPES_INTERNAL
    public
#endif    
    struct Result : IEquatable<Result>
    {
#if DEBUG
        internal
#endif
        readonly Error _error;

        internal Result(Error error)
        {
            Tag = ResultType.Failure;
            _error = error;
        }

        public ResultType Tag { get; private set; }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            var otherType = other.GetType();
            if (otherType != GetType()) return false;
            var otherTag = (ResultType)otherType.GetProperty(
                "Tag", BindingFlags.Public | BindingFlags.Instance).GetValue(other);
            if (otherTag != Tag) return false;
            if (otherTag == ResultType.Success && Tag == ResultType.Success) return true;
            var otherField = otherType.GetField("_error", BindingFlags.NonPublic | BindingFlags.Instance);
            return otherField.GetValue(other).Equals(_error);
        }

        public bool Equals(Result other) =>
            other.Tag != ResultType.Failure || _error.Equals(other._error);

        public static bool operator ==(Result left, Result right) => left.Equals(right);

        public static bool operator !=(Result left, Result right) => !left.Equals(right);

        public override int GetHashCode() =>
            Tag == ResultType.Success
                ? ToString().GetHashCode()
                : _error.GetHashCode();

        public override string ToString() =>
            Tag switch {
                ResultType.Success => "<Success>",
                _                  => _error.ToString()
            };

#region Value Case Constructors
        public static Result Failure(string error) => new Result(
            new Error(error, null));

        public static Result Failure(string error, Exception exception) => new Result(
            new Error(error, exception));

        public static Result Success => new Result();
#endregion

#region Basic Match Methods
        public bool MatchFailure(out Error error)
        {
            error = Tag == ResultType.Failure ? _error : default;
            return Tag == ResultType.Failure;
        }

        public bool MatchSuccess() => Tag == ResultType.Success;
#endregion
    }

#if !CSX_TYPES_INTERNAL
    public
#endif
    static class ResultExtensions
    {
        public static Unit Match(this Result result,
            Func<Unit> onSuccess, Func<Error, Unit> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            return result.MatchFailure(out Error error) switch {
                true => onFailure(error),
                _    => onSuccess() 
            };
        }

        public static Unit Match(this Result result,
            Func<Unit> onSuccess, Func<string, Unit> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            return result.MatchFailure(out Error error) switch {
                true => onFailure(error.Message),
                _    => onSuccess() 
            };
        }

        public static Unit Match(this Result result,
            Func<Unit> onSuccess, Func<Maybe<Exception>, Unit> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            return result.MatchFailure(out Error error) switch {
                true => onFailure(error.Exception),
                _    => onSuccess() 
            };
        }

        public static Unit Match(this Result result,
            Func<Unit> onSuccess, Func<Exception, Unit> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            return result.MatchFailure(out Error error) switch {
                true => onFailure(error.Exception.FromJust()),
                _    => onSuccess() 
            };
        }
    }
}