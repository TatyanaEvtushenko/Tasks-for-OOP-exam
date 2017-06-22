using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OOP_ForExam.Comparers;

namespace OOP_ForExam.Tasks
{
    //Разработать контейнер «коллекция» — Collection, который:
    //- содержит элементы типа CollectionItem, содержащие ссылку на своего владельца, +
    //- увеличивает свой размер при добавлении в него элементов, +
    //- предоставляет типовые свойства(текущее количество элементов, количество зарезервированных элементов), +
    //- предоставляет типовые процедуры, применяемые при работе с коллекцией(вставка, удаление, очистка, 
    //копирование в другую коллекцию, поиск индекса элемента по ссылке, сортировка), +
    //- предоставляет типовые события, происходящие при работе с коллекцией(вставка, удаление, очистка), +
    //- при обращении к элементу коллекции проверяет, что индекс находится в допустимых пределах. +
    //Продемонстрировать использование класса.

    class CollectionItem : IComparable<CollectionItem>
    {
        public Collection Owner { get; set; }

        public object Item { get; set; }

        public CollectionItem(Collection owner, object item)
        {
            Owner = owner;
            Item = item;
        }

        public override string ToString()
        {
            return $"Owner = {this.Owner}, Item = {this.Item}\n";
        }

        private readonly IComparer _comparer = new ObjectComparer();

        public int CompareTo(CollectionItem obj)
        {
            return _comparer.Compare(Item, obj.Item);
        }
    }

    class Collection : IList<CollectionItem>
    {
        private CollectionItem[] _items;

        public int Count { get; private set; }

        public int CountOfReservedElements => _items.Length;

        public bool IsReadOnly => false;

        private bool IsCorrectIndex(int index)
        {
            return index >= 0 && index < Count;
        }

        public CollectionItem this[int index]
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
            _items = new CollectionItem[countOfReservedElements];
        }

        public Collection(CollectionItem[] items)
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

        public void Add(CollectionItem item)
        {
            IncreaseLength();
            _items[Count] = item;
            Count++;
            OnInsert(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        public void Insert(int index, CollectionItem item)
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

        public bool Remove(CollectionItem item)
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

        public bool Contains(CollectionItem item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(Collection otherList)
        {
            otherList.Clear();
            var items = GetCorrectItems().ToArray();
            otherList._items = items;
            otherList.Count = items.Length;
        }

        public void CopyTo(CollectionItem[] array, int arrayIndex)
        {
            Array.Copy(_items, array, Count - arrayIndex - 1);
        }

        public int IndexOf(CollectionItem item)
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

        private IEnumerable<CollectionItem> GetCorrectItems()
        {
            var items = new CollectionItem[Count];
            Array.Copy(_items, items, Count);
            return items;
        }

        public IEnumerator<CollectionItem> GetEnumerator()
        {
            return GetCorrectItems().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetCorrectItems().GetEnumerator();
        }
    }
}
