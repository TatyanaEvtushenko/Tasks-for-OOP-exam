using System;
using System.Collections.Generic;

namespace OOP_ForExam.Comparers
{
    class KeyComparer<K, V> : IComparer<KeyValuePair<K, V>>
    {
        private readonly ObjectComparer _objectComparer = new ObjectComparer();

        public int Compare(KeyValuePair<K, V> x, KeyValuePair<K, V> y)
        {
            var value = x.Key as IComparable<K>;
            return value?.CompareTo(y.Key) ?? _objectComparer.Compare(x.Key, y.Key);
        }
    }
}
