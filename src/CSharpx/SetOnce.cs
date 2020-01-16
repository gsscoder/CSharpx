//#define CSX_SETONCE_INTERNAL // Uncomment or define at build time to set SetOnce<T> accessibility to internal.
//#define CSX_SETONCE_ONLY_UNSAFE // Uncomment or define at build time to remove thread-safe implementation.

using System;

namespace CSharpx
{
    /// <summary>
    /// Wraps a value that can be set only once.
    /// </summary>
    #if !CSX_SETONCE_INTERNAL
    public
    #endif
    struct SetOnce<T>
    {
        private bool _set;
        private T _value;

        /// <summary>
        /// Inner wrapped value.
        /// </summary>
        public T Value
        {
            get
            {
                if (_set) {
                    return _value;
                }
                else {
                    throw new InvalidOperationException("Value not set");
                }
            }
            set
            {
                if (_set) {
                    throw new InvalidOperationException("Value can be set only once");
                }
                else {
                    _value = value;
                    _set = true;
                }
            }
        }

        public bool HasValue
        {
            get { return _set; }
        }

        public static implicit operator T(SetOnce<T> instance) => instance.Value;
    }

#if !CSX_SETONCE_ONLY_UNSAFE
    /// <summary>
    /// Wraps a value that can be set only once. Thread-safe implementation.
    /// </summary>
    #if !CSX_SETONCE_INTERNAL
    public
    #endif
    class SafeSetOnce<T>
    {
        private bool _set;
        private T _value;

        /// <summary>
        /// Inner wrapped value.
        /// </summary>
        public T Value
        {
            get
            {
                lock (this)
                {
                    if (_set) {
                        return _value;
                    }
                    else {
                        throw new InvalidOperationException("Value not set");
                    }
                } 
            }
            set
            {
                lock (this)
                {
                    if (_set) {
                        throw new InvalidOperationException("Value can be set only once");
                    }
                    else {
                        _value = value;
                        _set = true;
                    }
                }
            }
        }

        public static implicit operator T(SafeSetOnce<T> instance) => instance.Value;

        public bool HasValue
        {
            get { return _set; }
        }
    }
#endif
}