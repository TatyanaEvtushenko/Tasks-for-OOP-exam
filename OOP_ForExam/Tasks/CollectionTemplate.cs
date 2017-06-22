using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OOP_ForExam.Tasks
{
    //Разработать шаблон «коллекции» — Collection<T>, который:
    //- содержит элементы типа CollectionItem<T>, содержащие ссылку на своего владельца, +
    //- увеличивает свой размер при добавлении в него элементов, +
    //- предоставляет типовые свойства (текущее количество элементов, количество зарезервированных элементов), +
    //- предоставляет типовые процедуры, применяемые при работе с коллекцией(вставка, удаление, очистка, 
    //копирование в другую коллекцию, поиск индекса элемента по ссылке, сортировка), +
    //- предоставляет типовые события, происходящие при работе с коллекцией(вставка, удаление, очистка), +
    //- при обращении к элементу коллекции проверяет, что индекс находится в допустимых пределах. +
    //Продемонстрировать использование класса.

    class CollectionItem<T> : IComparable<CollectionItem<T>>
        where T: IComparable<T>
    {
        public Collection<T> Owner { get; set; }

        public T Item { get; set; }

        public CollectionItem(Collection<T> owner, T item)
        {
            Owner = owner;
            Item = item;
        }

        public int CompareTo(CollectionItem<T> other)
        {
            return Item.CompareTo(other.Item);
        }

        public override string ToString()
        {
            return $"Owner = {this.Owner}, Item = {this.Item}\n";
        }
    }

    class Collection<T> : IList<CollectionItem<T>>
        where T: IComparable<T>
    {
        private CollectionItem<T>[] _items;

        public int Count { get; private set; }

        public int CountOfReservedElements => _items.Length;

        public bool IsReadOnly => false;

        private bool IsCorrectIndex(int index)
        {
            return index >= 0 && index < Count;
        }

        public CollectionItem<T> this[int index]
        {
            get
            {
                if (IsCorrectIndex(index))
                {
                    return _items[index];
                }
                throw new ArgumentOutOfRangeException();
            }
            set
            {
                if (IsCorrectIndex(index))
                {
                    _items[index] = value;
                }
            }
        }

        public Collection(int countOfReservedElements = 1)
        {
            if (countOfReservedElements <= 0)
            {
                countOfReservedElements = 1;
            }
            _items = new CollectionItem<T>[countOfReservedElements];
        }

        public Collection(CollectionItem<T>[] items)
        {
            _items = items;
            Count = items.Length;
        }

        private void IncreaseLength()
        {
            var countOfReservedElements = _items.Length;
            if (Count == countOfReservedElements)
            {
                Array.Resize(ref _items, countOfReservedElements == 0 ? 2 : countOfReservedElements * 2);
            }
        }

        public void Add(CollectionItem<T> item)
        {
            IncreaseLength();
            _items[Count] = item;
            Count++;
            OnInsert(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        public void Insert(int index, CollectionItem<T> item)
        {
            IncreaseLength();
            if (!IsCorrectIndex(index))
            {
                index = Count;
            }
            for (var i = Count; i > index; i--)
            {
                _items[i] = _items[i - 1];
            }
            _items[index] = item;
            Count++;
            OnInsert(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        public bool Remove(CollectionItem<T> item)
        {
            int i;
            for (i = 0; i < Count && _items[i] == item; i++) { }
            if (i >= Count) return false;
            RemoveAt(i);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (!IsCorrectIndex(index)) return;
            var item = _items[index];
            for (var i = index + 1; i < Count; i++)
            {
                _items[i - 1] = _items[i];
            }
            Count--;
            OnRemove(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
        }

        public void Clear()
        {
            Count = 0;
            Array.Resize(ref _items, 0);
            OnClear(this, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, this));
        }

        public int IndexOf(CollectionItem<T> item)
        {
            for (var i = 0; i < Count; i++)
            {
                if (_items[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Contains(CollectionItem<T> item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(Collection<T> otherList)
        {
            otherList.Clear();
            var items = GetCorrectItems().ToArray();
            otherList._items = items;
            otherList.Count = items.Length;
        }

        public void CopyTo(CollectionItem<T>[] array, int arrayIndex)
        {
            Array.Copy(_items, array, Count - arrayIndex - 1);
        }

        public void Sort()
        {
            Array.Sort(_items, 0, Count);
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

        private IEnumerable<CollectionItem<T>> GetCorrectItems()
        {
            var items = new CollectionItem<T>[Count];
            Array.Copy(_items, items, Count);
            return items;
        }

        public IEnumerator<CollectionItem<T>> GetEnumerator()
        {
            return GetCorrectItems().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetCorrectItems().GetEnumerator();
        }
    }
}
