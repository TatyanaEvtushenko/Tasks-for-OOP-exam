using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OOP_ForExam.Comparers;

namespace OOP_ForExam.Tasks
{
    //Разработать класс «словарь» — Dictionary, который представляет собой набор ключей и их значений, и который:
    //- содержит ключи типа string и значения типа object,
    //- увеличивает свой размер при добавлении в него элементов,
    //- предоставляет типовые свойства(количество ключей и значений, признак сортированного «словаря»),
    //- предоставляет типовые процедуры(вставка ключа и значения, удаление значения по ключу, поиск значения по ключу, очистка, сортировка),
    //- предоставляет типовые события, происходящие при работе со «словарем» (вставка, удаление, очистка).
    //Продемонстрировать использование класса.

    class Dictionary : IDictionary<string, object>
    {
        private KeyValuePair<string, object>[] _items = new KeyValuePair<string, object>[1];

        public int Count { get; private set; }

        public bool IsSorted { get; private set; }

        public bool IsReadOnly => false;

        public ICollection<string> Keys => _items.Select(x => x.Key).ToList();

        public ICollection<object> Values => _items.Select(x => x.Value).ToList();

        public object this[string key]
        {
            get
            {
                return _items.First(x => x.Key.Equals(key)).Value;
            }
            set
            {
                var index = IndexOf(key);
                if (index < 0)
                {
                    return;
                }
                _items[index] = new KeyValuePair<string, object>(key, value);
                IsSorted = false;
            }
        }

        private void IncreaseLength()
        {
            var countOfReservedElements = _items.Length;
            if (Count == countOfReservedElements)
            {
                Array.Resize(ref _items, countOfReservedElements == 0 ? 2 : countOfReservedElements * 2);
            }
        }

        private IEnumerable<KeyValuePair<string, object>> GetCorrectItems()
        {
            var items = new KeyValuePair<string, object>[Count];
            Array.Copy(_items, items, Count);
            return items;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return GetCorrectItems().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetCorrectItems().GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            IncreaseLength();
            _items[Count] = item;
            Count++;
            IsSorted = false;
            OnInsert(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        public void Add(string key, object value)
        {
            var pair = new KeyValuePair<string, object>(key, value);
            Add(pair);
        }

        public void Clear()
        {
            Count = 0;
            Array.Resize(ref _items, 0);
            OnInsert(this, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, this));
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _items.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _items.Select(x => x.Key).Contains(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            Array.Copy(_items, array, Count - arrayIndex - 1);
        }

        private int IndexOf(string key)
        {
            for (var i = 0; i < Count; i++)
            {
                if (_items[i].Key.Equals(key))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            var index = IndexOf(item.Key);
            if (index < 0)
            {
                return false;
            }
            for (var i = index + 1; i < Count; i++)
            {
                _items[i - 1] = _items[i];
            }
            Count--;
            OnRemove(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
            return true;
        }

        public bool Remove(string key)
        {
            var index = IndexOf(key);
            if (index < 0)
            {
                return false;
            }
            var item = _items[index];
            for (var i = index + 1; i < Count; i++)
            {
                _items[i - 1] = _items[i];
            }
            Count--;
            OnRemove(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
            return true;
        }

        public bool TryGetValue(string key, out object value)
        {
            try
            {
                var item = _items.First(x => x.Key.Equals(key));
                value = item.Value;
                return true;
            }
            catch
            {
                value = this[key];
                return false;
            }
        }

        public void SortByKey()
        {
            Array.Sort(_items, 0, Count, new KeyComparer<string, object>());
            IsSorted = true;
        }

        public void SortByValue()
        {

            Array.Sort(_items, 0, Count, new ValueComparer<string, object>());
            IsSorted = true;
        }

        public event CollectionChangeEventHandler InsertEvent;

        public event CollectionChangeEventHandler RemoveEvent;

        public event CollectionChangeEventHandler ClearEvent;

        public void OnInsert(object sender, CollectionChangeEventArgs e)
        {
            InsertEvent?.Invoke(sender, e);
        }

        public void OnRemove(object sender, CollectionChangeEventArgs e)
        {
            RemoveEvent?.Invoke(sender, e);
        }

        public void OnClear(object sender, CollectionChangeEventArgs e)
        {
            ClearEvent?.Invoke(sender, e);
        }
    }
}
