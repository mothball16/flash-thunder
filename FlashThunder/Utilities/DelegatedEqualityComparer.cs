using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashThunder.Utilities
{
    /// <summary>
    /// Credit: https://stackoverflow.com/a/42456012/30180794
    /// For anonymous equality comparers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegatedEqualityComparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, int> _getHashCode;
        private readonly Func<T, T, bool> _equals;
        public DelegatedEqualityComparer(Func<T, int> getHashCode, Func<T, T, bool> equals)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(getHashCode));
            ArgumentException.ThrowIfNullOrEmpty(nameof(equals));
            _getHashCode = getHashCode;
            _equals = equals;
        }
        public override int GetHashCode(T obj) => _getHashCode(obj);
        public override bool Equals(T x, T y) => _equals(x, y);
    }
}
