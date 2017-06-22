using System;
using System.Collections.Generic;

namespace OOP_ForExam.Comparers
{
    class ValueComparer<K, V> : IComparer<KeyValuePair<K, V>>
    {
        private readonly ObjectComparer _objectComparer = new ObjectComparer();

        public int Compare(KeyValuePair<K, V> x, KeyValuePair<K, V> y)
        {
            var value = x.Value as IComparable<V>;
            return value?.CompareTo(y.Value) ?? _objectComparer.Compare(x.Value, y.Value);
        }
    }
}
