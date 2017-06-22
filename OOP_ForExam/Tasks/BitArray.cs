using System;
using System.Collections;
using System.Collections.Generic;

namespace OOP_ForExam.Tasks
{
    //Разработать контейнер «битовый массив» — BitArray, который
    //- содержит элементы типа bool так, что на каждый отводится лишь 1 бит, +
    //- увеличивает свой размер при добавлении в него элементов, +
    //- предоставляет типовые свойства(текущее количество элементов, количество зарезервированных элементов), +
    //- предоставляет типовые процедуры, применяемые при работе с массивом(чтение и запись бита по индексу, расширение массива), +
    //- при обращении к биту в массиве проверяет, что индекс находится в допустимых пределах. +
    //Продемонстрировать использование класса.

    class BitArray : IEnumerable<bool>
    {
        private const int COUNT_BIT_IN_BYTE = 8;

        private byte[] _bits;

        public int Count { get; private set; }

        public int CountOfReservedElements => _bits.Length * COUNT_BIT_IN_BYTE;

        public BitArray(bool[] bits)
        {
            Count = bits.Length;
            var countOfBytes = Count / COUNT_BIT_IN_BYTE + Count % COUNT_BIT_IN_BYTE == 0 ? 0 : 1;
            _bits = new byte[countOfBytes];
            for (var i = 0; i < Count; i++)
            {
                SetBit(bits[i], i);
            }
        }

        public BitArray(int length = COUNT_BIT_IN_BYTE)
        {
            Count = 0;
            var countOfBytes = length / COUNT_BIT_IN_BYTE + length % COUNT_BIT_IN_BYTE == 0 ? 0 : 1;
            _bits = new byte[countOfBytes];
        }

        private bool IsCorrectIndex(int index)
        {
            return index >= 0 && index < Count;
        }

        private static byte GetMask(int index)
        {
            var indexInByte = index % COUNT_BIT_IN_BYTE;
            return (byte)Math.Pow(2, COUNT_BIT_IN_BYTE - indexInByte - 1);
        }

        private bool GetBit(int index)
        {
            var indexOfByte = index / COUNT_BIT_IN_BYTE;
            var mask = GetMask(index);
            return (_bits[indexOfByte] & mask) == mask;
        }

        private void SetBit(bool value, int index)
        {
            var indexOfByte = index / COUNT_BIT_IN_BYTE;
            var mask = GetMask(index);
            if (value)
            {
                _bits[indexOfByte] |= mask;
            }
            else
            {
                _bits[indexOfByte] &= (byte)~mask;
            }
        }

        public bool this[int index]
        {
            get
            {
                if (!IsCorrectIndex(index))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return GetBit(index);
            }
            set
            {
                if (IsCorrectIndex(index))
                {
                    SetBit(value, index);
                }

            }
        }

        public void IncreaseArray(int length)
        {
            Count = length;
            IncreaseLength();
        }

        public void Add(bool value)
        {
            IncreaseLength();
            SetBit(value, Count);
            Count++;
        }

        private void IncreaseLength()
        {
            var countOfReservedElements = _bits.Length;
            if (Count >= countOfReservedElements * COUNT_BIT_IN_BYTE)
            {
                Array.Resize(ref _bits, countOfReservedElements == 0 ? 2 : countOfReservedElements * 2);
            }
        }

        public IEnumerator<bool> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return GetBit(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
