using System;
using System.ComponentModel;

namespace OOP_ForExam.Tasks
{
    //Разработать шаблон стека — Stack<T>, который:
    //- содержит элементы заданного типа данных T, +
    //- предоставляет типовые свойства(указатель на вершину стека, указатель на дно стека), +
    //- предоставляет типовые процедуры, применяемые при работе со стеком(загрузка элемента на стек, выталкивание элемента из стека, очистка стека), +
    //- предоставляет типовые события, происходящие при работе со стеком(загрузка в стек, выталкивание из стека), +
    //Продемонстрировать использование класса.

    class Stack<T> 
    {
        private T[] _items;

        private int _count;

        public T Top
        {
            get
            {
                if (_count > 0)
                {
                    return _items[_count - 1];
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public T Bottom
        {
            get
            {
                if (_count > 0)
                {
                    return _items[0];
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public Stack(int countOfReservedElements = 1)
        {
            _items = new T[countOfReservedElements];
        }

        public Stack(T[] items)
        {
            _items = items;
            _count = items.Length;
        }

        private void IncreaseLength()
        {
            var countOfReservedElements = _items.Length;
            if (_count == countOfReservedElements)
            {
                Array.Resize(ref _items, countOfReservedElements == 0 ? 2 : countOfReservedElements * 2);
            }
        }

        public void Push(T item)
        {
            IncreaseLength();
            _items[_count] = item;
            _count++;
            OnPush(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        public T Pop()
        {
            if (_count <= 0) throw new ArgumentOutOfRangeException();
            _count--;
            OnPop(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, _items[_count]));
            return _items[_count];
        }

        public void Clear()
        {
            Array.Resize(ref _items, 0);
            _count = 0;
        }

        public event CollectionChangeEventHandler PopEvent;

        public event CollectionChangeEventHandler PushEvent;

        public void OnPush(object sender, CollectionChangeEventArgs e)
        {
            PushEvent?.Invoke(sender, e);
        }

        public void OnPop(object sender, CollectionChangeEventArgs e)
        {
            PopEvent?.Invoke(sender, e);
        }
    }
}
