﻿// Copyright (c) 2011-2020 Roland Pheasant. All rights reserved.
// Roland Pheasant licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using DynamicData.Kernel;

namespace DynamicData.Cache.Internal
{
    internal sealed class ImmutableGroup<TObject, TKey, TGroupKey> : IGrouping<TObject, TKey, TGroupKey>, IEquatable<ImmutableGroup<TObject, TKey, TGroupKey>>
        where TKey : notnull
        where TGroupKey : notnull
    {
        private readonly ICache<TObject, TKey> _cache;

        internal ImmutableGroup(TGroupKey key, ICache<TObject, TKey> cache)
        {
            Key = key;
            _cache = new Cache<TObject, TKey>(cache.Count);
            cache.KeyValues.ForEach(kvp => _cache.AddOrUpdate(kvp.Value, kvp.Key));
        }

        public int Count => _cache.Count;

        public IEnumerable<TObject> Items => _cache.Items;

        public TGroupKey Key { get; }

        public IEnumerable<TKey> Keys => _cache.Keys;

        public IEnumerable<KeyValuePair<TKey, TObject>> KeyValues => _cache.KeyValues;

        public static bool operator ==(ImmutableGroup<TObject, TKey, TGroupKey> left, ImmutableGroup<TObject, TKey, TGroupKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ImmutableGroup<TObject, TKey, TGroupKey> left, ImmutableGroup<TObject, TKey, TGroupKey> right)
        {
            return !Equals(left, right);
        }

        public bool Equals(ImmutableGroup<TObject, TKey, TGroupKey>? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other is not null && EqualityComparer<TGroupKey?>.Default.Equals(Key, other.Key);
        }

        public override bool Equals(object? obj)
        {
            return obj is ImmutableGroup<TObject, TKey, TGroupKey> value && Equals(value);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TGroupKey>.Default.GetHashCode(Key);
        }

        public Optional<TObject> Lookup(TKey key)
        {
            return _cache.Lookup(key);
        }

        public override string ToString()
        {
            return $"Grouping for: {Key} ({Count} items)";
        }
    }
}