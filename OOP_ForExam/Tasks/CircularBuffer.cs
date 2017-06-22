using System;
using System.Collections;
using System.Collections.Generic;

namespace OOP_ForExam.Tasks
{
    //Разработать класс «кольцевой буфер» – CircularBuffer.Класс:
    //- содержит байты данных, +
    //- предоставляет типовые свойства(указатель чтения, указатель записи, количество данных в буфере), +
    //- предоставляет типовые процедуры, применяемые при работе с кольцевым буфером(запись, чтение, наращивание размера буфера), +
    //- генерирует исключительную ситуацию при переполнении буфера. +
    //Продемонстрировать использование класса.

    class CircularBuffer : IEnumerable<byte>
    {
        private byte[] _items;

        private int _size;

        private int _indexToWrite;

        private int _indexToRead;

        public int IndexToWrite => _indexToWrite % _size;

        public int IndexToRead => _indexToRead % _size;

        public int CountOfData => _indexToWrite > _indexToRead ? _indexToWrite - _indexToRead : _size - _indexToRead + _indexToWrite + 1;

        public CircularBuffer(int size = 255)
        {
            _items = new byte[size];
            _size = size;
        }

        public void Write(byte item)
        {
            if (_indexToWrite == _indexToRead + _size)
            {
                throw new Exception("Buffer overflow!");
            }
            _items[IndexToWrite] = item;
            _indexToWrite++;
        }

        public byte Read()
        {
            var index = IndexToRead;
            _indexToRead++;
            return _items[index];
        }

        public void IncreaseBuffer(int newSize)
        {
            if (newSize <= _size)
            {
                return;
            }
            Array.Resize(ref _items, newSize);
            if (IndexToWrite < IndexToRead || IndexToWrite == IndexToRead && _indexToWrite / _size > _indexToRead / _size)
            {
                var j = 0;
                var i = _size;
                while (j < IndexToWrite)
                {
                    _items[i] = _items[j];
                    j++;
                    i = i == newSize - 1 ? 0 : i + 1;
                }
                _indexToWrite = (_indexToWrite / _size - i < _size ? 0 : 1) * newSize + i;
            }
            else
            {
                _indexToWrite = _indexToWrite / _size * newSize + IndexToWrite;
            }
            _indexToRead = _indexToRead / _size * newSize + IndexToRead;
            _size = newSize;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            return (_items as IEnumerable<byte>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
