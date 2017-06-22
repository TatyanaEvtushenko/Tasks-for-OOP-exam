using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace OOP_ForExam.Tasks
{
    //Разработать контейнер «стек» — Stack, который эмулирует стек процессора, и который:
    //- содержит произвольные данные, представляемые в виде последовательности байтов, +
    //- предоставляет типовые свойства(указатель на вершину стека, указатель на начало «кадра», указатель на дно стека),
    //- предоставляет типовые процедуры, применяемые при работе со стеком(создание нового «кадра», выталкивание «кадра», 
    //загрузка данных заданного размера, выталкивание данных заданного размера),
    //- предоставляет типовые события, происходящие при работе со стеком(загрузка в стек, выталкивание из стека), +
    //Продемонстрировать использование класса.

    class Stack : IEnumerable
    {
        private byte[] _items;

        private readonly System.Collections.Generic.List<int> _indexesOfFrames = new System.Collections.Generic.List<int>();

        private readonly BinaryFormatter _formatter = new BinaryFormatter();

        private int _count;

        public byte Top
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

        public byte Bottom
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

        public byte BeginOfFrame
        {
            get
            {
                var length = _indexesOfFrames.Count;
                if (length <= 0) throw new ArgumentOutOfRangeException();
                var index = _indexesOfFrames[length - 1];
                return _items[index];
            }
        }

        public Stack(int countOfReservedElements = 1)
        {
            _items = new byte[countOfReservedElements];
        }

        public Stack(byte[] items)
        {
            _items = items;
            _count = items.Length;
        }

        private void IncreaseLength()
        {
            var countOfReservedElements = _items.Length;
            if (_count >= countOfReservedElements)
            {
                Array.Resize(ref _items, Math.Max(_count, countOfReservedElements * 2));
            }
        }
        
        public void Push(object item)
        {
            using (var stream = new MemoryStream())
            {
                _formatter.Serialize(stream, item);
                var length = (int)stream.Length;
                var buffer = new byte[length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(buffer, 0, length);
                Push(buffer, length);
            }
        }

        public void Push(byte[] item, int length)
        {
            if (length <= 0) return;
            var oldCount = _count;
            _count += length;
            IncreaseLength();
            for (var i = oldCount; i < oldCount + length; i++)
            {
                _items[i] = item[i - oldCount];
            }
            _indexesOfFrames.Add(oldCount);
            OnPush(this, new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
        }

        public object Pop()
        {
            if (_count <= 0) throw new ArgumentOutOfRangeException();
            var length = _count - _indexesOfFrames.Last();
            var frame = Pop(length);
            using (var stream = new MemoryStream())
            {
                stream.Write(frame, 0, length);
                stream.Seek(0, SeekOrigin.Begin);
                return _formatter.Deserialize(stream);
            }
        }

        public byte[] Pop(int length)
        {
            if (_count <= 0) throw new ArgumentOutOfRangeException();
            var buffer = new byte[length];
            for (var i = _count - length; i < _count; i++)
            {
                buffer[i - (_count - length)] = _items[i];
            }
            _count -= length;
            while (_indexesOfFrames.Count != 0 && _indexesOfFrames.Last() >= _count)
            {
                _indexesOfFrames.RemoveAt(_indexesOfFrames.Count - 1);
            }
            OnPop(this, new CollectionChangeEventArgs(CollectionChangeAction.Remove, buffer));
            return buffer;
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

        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
