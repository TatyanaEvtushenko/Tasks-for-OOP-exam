using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OOP_ForExam.Tasks
{
    //Разработать шаблон «динамический массив» — List<T>, который
    //- содержит элементы заданного типа данных T, +
    //- увеличивает свой размер при добавлении в него элементов, +
    //- предоставляет типовые свойства(текущее количество элементов, количество зарезервированных элементов), +
    //- предоставляет типовые процедуры, применяемые при работе с массивом(вставка по индексу, удаление по индексу, очистка, 
    //копирование в другой массив, поиск индекса элемента по ссылке, сортировка), +
    //- при обращении к элементу массива проверяет, что индекс находится в допустимых пределах. +
    //Продемонстрировать использование класса.

    class List<T> : IList<T> 
        where T: IComparable<T> 
    {
        private T[] _items;

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        public int CountOfReservedElements => _items.Length;

        private bool IsCorrectIndex(int index)
        {
            return index >= 0 && index < Count;
        }

        public T this[int index]
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

        public List(int countOfReservedElements = 1)
        {
            if (countOfReservedElements <= 0)
            {
                countOfReservedElements = 1;
            }
            _items = new T[countOfReservedElements];
        }

        public List(T[] items)
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

        public void Add(T item)
        {
            IncreaseLength();
            _items[Count] = item;
            Count++;
        }

        public void Insert(int index, T item)
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
        }

        public bool Remove(T item)
        {
            int i;
            for (i = 0; i < Count && _items[i].Equals(item); i++) { }
            if (i >= Count) return false;
            RemoveAt(i);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (!IsCorrectIndex(index)) return;
            for (var i = index + 1; i < Count; i++)
            {
                _items[i - 1] = _items[i];
            }
            Count--;
        }

        public void Clear()
        {
            Count = 0;
            Array.Resize(ref _items, 0);
        }

        public bool Contains(T item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, array, Count - arrayIndex - 1);
        }

        public void CopyTo(List<T> otherList)
        {
            otherList.Clear();
            var items = GetCorrectItems().ToArray();
            otherList._items = items;
            otherList.Count = items.Length;
        }

        public int IndexOf(T item)
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

        private IEnumerable<T> GetCorrectItems()
        {
            var items = new T[Count];
            Array.Copy(_items, items, Count);
            return items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return GetCorrectItems().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetCorrectItems().GetEnumerator();
        }
    }
}
