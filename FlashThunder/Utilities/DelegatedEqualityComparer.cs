﻿using System;
using System.Collections.Generic;

namespace FlashThunder.Utilities;

/// <summary>
/// Credit: https://stackoverflow.com/a/42456012/30180794
/// For equality comparer shortcut cause im not making a class for two lines of code
/// </summary>
/// <typeparam name="T"></typeparam>
internal class DelegatedEqualityComparer<T> : EqualityComparer<T>
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
