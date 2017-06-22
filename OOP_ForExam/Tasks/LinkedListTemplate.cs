using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace OOP_ForExam.Tasks
{
    //Разработать шаблон контейнера «связный список» — LinkedList<T>, который:
    //- содержит элементы типа T, +
    //- предоставляет типовые свойства(первый элемент, следующий элемент), +
    //- предоставляет типовые процедуры, применяемые при работе со списком(вставка элемента в начало и в конец, удаление элемента, 
    //очистка списка, замена элемента, перемещение списка в другой список), +
    //- предоставляет типовые события, происходящие при работе со списком(вставка, удаление, очистка), +
    //Продемонстрировать использование класса.

    class LinkedListNode<T>
    {
        public T Data;

        public LinkedListNode<T> Next;

        public LinkedListNode(T data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }

    class LinkedList<T> : IEnumerable<T>
    {
        public LinkedListNode<T> First { get; private set; }

        public LinkedListNode<T> Next => First?.Next;

        public void AddFirst(T item)
        {
            var node = new LinkedListNode<T>(item) {Next = First};
            First = node;
            OnInsert(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        public void AddLast(T item)
        {
            var node = new LinkedListNode<T>(item);
            if (First == null)
            {
                First = node;
            }
            else
            {
                var last = First;
                while (last.Next != null)
                {
                    last = last.Next;
                }
                last.Next = node;
            }
            OnInsert(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        public void Remove(T item)
        {
            var node = First;
            LinkedListNode<T> prev = null;
            while (node != null && !node.Data.Equals(item))
            {
                prev = node;
                node = node.Next;
            }
            if (node == null) return;
            if (node == First)
            {
                First = node.Next;
            }
            if (prev != null)
            {
                prev.Next = node.Next;
            }
            OnRemove(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
        }

        public void Clear()
        {
            First = null;
            OnClear(this, new CollectionChangeEventArgs(CollectionChangeAction.Refresh, this));
        }

        public void Replace(T oldItem, T newItem)
        {
            var node = First;
            while (node != null && !node.Data.Equals(oldItem))
            {
                node = node.Next;
            }
            if (node == null) return;
            node.Data = newItem;
        }

        public void CopyTo(LinkedList<T> otherList)
        {
            var nodeThis = First;
            var nodeOther = otherList.First;
            while (nodeThis != null && nodeOther != null)
            {
                nodeOther.Data = nodeThis.Data;
                nodeOther = nodeOther.Next;
                nodeThis = nodeThis.Next;
            }
            while (nodeThis != null)
            {
                var node = new LinkedListNode<T>(nodeThis.Data);
                if (otherList.First == null)
                {
                    otherList.First = node;
                }
                else
                {
                    var last = otherList.First;
                    while (last.Next != null)
                    {
                        last = last.Next;
                    }
                    last.Next = node;
                }
                nodeThis = nodeThis.Next;
            }
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

        public IEnumerator<T> GetEnumerator()
        {
            var node = First;
            while (node != null)
            {
                yield return node.Data;
                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
